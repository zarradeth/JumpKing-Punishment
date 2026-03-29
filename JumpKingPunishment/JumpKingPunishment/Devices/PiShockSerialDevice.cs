using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;

namespace JumpKingPunishment.Devices
{
    /// <summary>
    /// An implementation of a <see cref="IPunishmentDevice"/> backed by a PiShock connected
    /// via serial to the actual host device.
    /// </summary>
    public class PiShockSerialDevice : IPunishmentDevice
    {
        // API Docs: https://docs.pishock.com/pishock/api-documentation/serial-api-documentation.html

        private static SerialPort serialPort;
        private static int instanceCount = 0;

        /// <summary>
        /// The ctor for creating a <see cref="PiShockSerialDevice"/>
        /// </summary>
        public PiShockSerialDevice()
        {
            // We sometimes make multiple devices but only one thing across the system can have a COM port open
            // so we need to share the port if we do make multiple devices. Note this approach isn't thread safe
            // but I don't believe we are creating/disposing from multiple threads.
            instanceCount++;
            OpenCOMPort();
        }

        /// <summary>
        /// Disposes of the <see cref="PiShockSerialDevice"/>
        /// </summary>
        public void Dispose()
        {
            if (--instanceCount == 0)
            {
                if (serialPort?.IsOpen == true)
                {
                    serialPort?.Close();
                }
                serialPort?.Dispose();
                serialPort = null;
            }
        }

        private static readonly string[] PiShockPIDs = { "PID_7523", "PID_55D4" };
        private static readonly string PiShockVID = "VID_1A86";
        /// <summary>
        /// Enumerates COM ports that match known PiShock USB VID/PID values using WMI
        /// </summary>
        /// <returns>An enumerable of COM port names (e.g. "COM3") that appear to be PiShock hubs based on their USB VID/PID</returns>
        private IEnumerable<string> GetPiShockCOMPorts()
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%(COM%)'"))
            {
                foreach (ManagementObject device in searcher.Get())
                {
                    using (device)
                    {
                        string deviceId = device["DeviceID"]?.ToString() ?? "";
                        string name = device["Name"]?.ToString() ?? "";

                        if (deviceId.Contains(PiShockVID) && PiShockPIDs.Any(pid => deviceId.Contains(pid)))
                        {
                            // Extract COM port name from e.g. "USB-SERIAL CH340 (COM3)"
                            int start = name.IndexOf("(COM") + 1;
                            int end = name.IndexOf(")", start);
                            if (start > 0 && end > start)
                            {
                                yield return name.Substring(start, end - start);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Searches all PiShock COM ports for one that has a shocker matching the device ID in <see cref="Preferences.PiShockSerialPreferences.DeviceId"/>
        /// by sending an info command and parsing the TERMINALINFO response. Sets <see cref="serialPort"/> if a match is found.
        /// If <see cref="serialPort"/> is already open, returns true immediately without searching.
        /// </summary>
        /// <returns>True if a matching port was found and <see cref="serialPort"/> is open, false otherwise</returns>
        private bool OpenCOMPort()
        {
            if (serialPort?.IsOpen == true)
            {
                return true;
            }

            var ports = GetPiShockCOMPorts();
            foreach (string port in ports)
            {
                SerialPort testPort = null;
                try
                {
                    testPort = new SerialPort(port, 115200);
                    testPort.ReadTimeout = 2000;
                    testPort.WriteTimeout = 2000;
                    testPort.Open();

                    var PayloadData = new
                    {
                        cmd = "info"
                    };
                    testPort.WriteLine(JsonConvert.SerializeObject(PayloadData));

                    // Read lines until we find TERMINALINFO or timeout
                    while (true)
                    {
                        string line = testPort.ReadLine();
                        if (!line.StartsWith("TERMINALINFO"))
                        {
                            continue;
                        }

                        string json = line.Substring("TERMINALINFO".Length).TrimStart(':', ' ');
                        JObject info = JObject.Parse(json);
                        JArray shockers = info["shockers"] as JArray;
                        if (shockers != null)
                        {
                            foreach (JObject shocker in shockers)
                            {
                                if (shocker["id"].Value<string>() == DeviceManager.PiShockSerialPreferences.DeviceId)
                                {
                                    serialPort?.Dispose();
                                    serialPort = testPort;
                                    return true;
                                }
                            }
                        }
                        break;
                    }
                }
                catch (Exception)
                {
                    // If anything went wrong this likely isn't a valid device/something went wrong- just skip it
                }

                if (testPort?.IsOpen == true)
                {
                    testPort.Close();
                }
                testPort?.Dispose();
            }

            return false;
        }

        /// <inheritdoc/>
        public EFeedbackDevice GetDeviceType()
        {
            return EFeedbackDevice.PiShockSerial;
        }

        /// <inheritdoc/>
        public void Update(float p_delta)
        {
            // Intentionally blank
        }

        /// <inheritdoc/>
        public void Punish(float intensity, float duration, bool easyMode)
        {
            if (!easyMode)
            {
                Shock(Round(duration * 1000.0f), Round(intensity));
            }
            else
            {
                Vibrate(Round(duration * 1000.0f), Round(intensity));
            }
        }

        /// <inheritdoc/>
        public void Reward(float intensity, float duration)
        {
            Vibrate(Round(duration * 1000.0f), Round(intensity));
        }

        /// <inheritdoc/>
        public void Test(float intensity, float duration)
        {
            Beep(Round(duration * 1000.0f));
        }

        /// <summary>
        /// Rounds the provided intensity value to send it to the PiShock serial API
        /// The PiShock API only works with ints so we need to round floats, but we also want to round 0 values up so we still get some feedback
        /// </summary>
        /// <param name="value">The value to round</param>
        private int Round(float value)
        {
            if (value > 0.0f && value <= 0.5f)
            {
                value += 0.5f;
            }
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Send a shock to the PiShock device
        /// </summary>
        /// <param name="duration">The duration of the shock in ms, should be a value 0 to 15000</param>
        /// <param name="intensity">The intensity of the shock, should be a value 0 to 100</param>
        private void Shock(int duration, int intensity)
        {
            // Check the port is open/attempt to open it at the top of each operation- this allows the device to be removed/
            // reattached at runtime without needing to remake the whole device instance
            if (!OpenCOMPort())
            {
                return;
            }

            var PayloadData = new
            {
                cmd = "operate",
                value = new
                {
                    id = DeviceManager.PiShockSerialPreferences.DeviceId,
                    op = "shock",
                    duration,
                    intensity
                }
            };
            serialPort.WriteLine(JsonConvert.SerializeObject(PayloadData));
        }

        /// <summary>
        /// Send a vibration to the PiShock device
        /// </summary>
        /// <param name="duration">The duration of the vibration in ms, should be a value greater than 0</param>
        /// <param name="intensity">The intensity of the vibration, should be a value 0 to 100</param>
        private void Vibrate(int duration, int intensity)
        {
            if (!OpenCOMPort())
            {
                return;
            }

            var PayloadData = new
            {
                cmd = "operate",
                value = new
                {
                    id = DeviceManager.PiShockSerialPreferences.DeviceId,
                    op = "vibrate",
                    duration,
                    intensity
                }
            };
            serialPort.WriteLine(JsonConvert.SerializeObject(PayloadData));
        }

        /// <summary>
        /// Send a beep to the PiShock device
        /// </summary>
        /// <param name="duration">The duration of the beep in ms, should be a value greater than 0</param>
        private void Beep(int duration)
        {
            if (!OpenCOMPort())
            {
                return;
            }

            var PayloadData = new
            {
                cmd = "operate",
                value = new
                {
                    id = DeviceManager.PiShockSerialPreferences.DeviceId,
                    op = "beep",
                    duration
                }
            };
            serialPort.WriteLine(JsonConvert.SerializeObject(PayloadData));
        }
    }
}
