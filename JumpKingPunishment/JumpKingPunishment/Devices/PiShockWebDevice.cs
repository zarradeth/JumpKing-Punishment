using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JumpKingPunishment.Devices
{
    /// <summary>
    /// An implementation of a <see cref="IPunishmentDevice"/> backed by a PiShock connected
    /// over the web.
    /// </summary>
    public class PiShockWebDevice : IPunishmentDevice
    {
        // API docs: https://api.pishock.com/swagger/
        private static readonly string ApiEndpointBase = "https://api.pishock.com/";

        private HttpClient client;

        /// <summary>
        /// The ctor for creating a <see cref="PiShockWebDevice"/>
        /// </summary>
        public PiShockWebDevice()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            client = new HttpClient
            {
                BaseAddress = new Uri(ApiEndpointBase)
            };
        }

        /// <summary>
        /// Disposes of the <see cref="PiShockWebDevice"/>
        /// </summary>
        public void Dispose()
        {
            client?.Dispose();
            client = null;
        }

        /// <inheritdoc/>
        public EFeedbackDevice GetDeviceType()
        {
            return EFeedbackDevice.PiShockWeb;
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
                // Send a vibration instead for easy mode
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
            // Send a beep for testing, beeps don't support intensity, which is fine
            Beep(Round(duration * 1000.0f));
        }

        /// <summary>
        /// Rounds the provided value to send it to the PiShock API
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
        /// <param name="duration">The durtion of the shock in ms, should be a value 0 to 15000</param>
        /// <param name="intensity">The intensity of the shock, should be a value 0 to 100</param>
        private async void Shock(int duration, int intensity)
        {
            // Ignore empty requests, this will make the API return an error anyways
            if ((duration <= 0) || (intensity <= 0))
            {
                return;
            }

            // Clamp the values passed to the API as it will error out (or at least not behave as expected) if they are out of bounds
            duration = Math.Max(Math.Min(duration, 15000), 16);
            intensity = Math.Min(intensity, 100);

            // Note: Documentation calls out an 'AgentName' field that might be replaced and required in the future.
            //  The parameter "AgentName" will be removed in a future version however is still present for now -- Expect this to be replaced
            //  by the name of the actioning API Key in future."
            var PayloadData = new
            {
                Operation = 0,
                Duration = duration,
                Intensity = intensity
            };

            try
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"Shockers/{DeviceManager.PiShockWebPreferences.DeviceId}"))
                {
                    request.Headers.Add("X-PiShock-API-Key", DeviceManager.PiShockWebPreferences.APIKey);
                    request.Content = new StringContent(JsonConvert.SerializeObject(PayloadData), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        HandleResponse(response);
                    }
                }
            }
            catch (Exception)
            {
                // Intentionally blank, if a PiShock device is disposed while a request is in flight it will throw as the task will be canceled- this is fine
            }
        }

        /// <summary>
        /// Send a vibration to the PiShock device
        /// </summary>
        /// <param name="duration">The durtion of the vibration in ms, should be a value greater than 0</param>
        /// <param name="intensity">The intensity of the shock, should be a value 0 to 100</param>
        private async void Vibrate(int duration, int intensity)
        {
            // Ignore empty requests, this will make the API return an error anyways
            if ((duration <= 0) || (intensity <= 0))
            {
                return;
            }

            // Clamp the values passed to the API as it will error out (or at least not behave as expected) if they are out of bounds
            // (Documentation doesn't say vibrates need their duration clamped but testing the device/API it does not behave as expected above 15)
            duration = Math.Max(Math.Min(duration, 15000), 16);
            intensity = Math.Min(intensity, 100);

            var PayloadData = new
            {
                Operation = 1,
                Duration = duration,
                Intensity = intensity
            };

            try
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"Shockers/{DeviceManager.PiShockWebPreferences.DeviceId}"))
                {
                    request.Headers.Add("X-PiShock-API-Key", DeviceManager.PiShockWebPreferences.APIKey);
                    request.Content = new StringContent(JsonConvert.SerializeObject(PayloadData), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        HandleResponse(response);
                    }
                }
            }
            catch (Exception)
            {
                // Intentionally blank, if a PiShock device is disposed while a request is in flight it will throw as the task will be canceled- this is fine
            }
        }

        /// <summary>
        /// Send a beep to the PiShock device
        /// </summary>
        /// <param name="duration">The durtion of the beep in ms, should be a value greater than 0</param>
        public async void Beep(int duration)
        {
            // Ignore empty requests, this will make the API return an error anyways
            if (duration <= 0)
            {
                return;
            }

            // Clamp the values passed to the API as it will error out (or at least not behave as expected) if they are out of bounds
            // (Documentation doesn't say beeps need their duration clamped but testing the device/API it does not behave as expected above 15)
            duration = Math.Max(Math.Min(duration, 15000), 16);

            var PayloadData = new
            {
                Operation = 2,
                Duration = duration
            };

            try
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"Shockers/{DeviceManager.PiShockWebPreferences.DeviceId}"))
                {
                    request.Headers.Add("X-PiShock-API-Key", DeviceManager.PiShockWebPreferences.APIKey);
                    request.Content = new StringContent(JsonConvert.SerializeObject(PayloadData), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        HandleResponse(response);
                    }
                }
            }
            catch (Exception)
            {
                // Intentionally blank, if a PiShock device is disposed while a request is in flight it will throw as the task will be canceled- this is fine
            }
        }

        /// <summary>
        /// Handles the response from the PiShock API
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> returned by the PiShockAPI http request"</param>
        public /*async*/ void HandleResponse(HttpResponseMessage response)
        {
            //if (response.IsSuccessStatusCode)
            //{
            //    string responseContent = await response.Content.ReadAsStringAsync();
            //}
        }
    }
}
