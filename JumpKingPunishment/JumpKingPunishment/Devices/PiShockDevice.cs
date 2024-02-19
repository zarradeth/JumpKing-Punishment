using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace JumpKingPunishment.Devices
{
    /// <summary>
    /// An implementation of a <see cref="IPunishmentDevice"/> backed by a PiShock
    /// </summary>
    public class PiShockDevice : IPunishmentDevice
    {
        private static readonly string ApiEndpoint = "https://do.pishock.com/api/apioperate/";
        private static readonly string ApiRequestName = "JKPM_PiShock";

        private HttpClient client;

        /// <summary>
        /// The ctor for creating a <see cref="PiShockDevice"/>
        /// </summary>
        public PiShockDevice()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(ApiEndpoint)
            };

            // There's no API end point to check if you have valid credentials, you just need to try
            // to send a command and see if it succeeds- even worse even if they aren't valid you get
            // an okay response back from the API, but you need to check the message (which is just a string)
            // Given the above I'm not gonna bother to check if the API response is valid, the user should
            // just check the console output to see the API responses
        }

        /// <summary>
        /// Disposes of the <see cref="PiShockDevice"/>
        /// </summary>
        public void Dispose()
        {
            client?.Dispose();
            client = null;
        }

        /// <inheritdoc/>
        public EFeedbackDevice GetDeviceType()
        {
            return EFeedbackDevice.PiShock;
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
                Shock(Round(duration), Round(intensity));
            }
            else
            {
                // Send a vibration instead for easy mode
                Vibrate(Round(duration), Round(intensity));
            }
        }

        /// <inheritdoc/>
        public void Reward(float intensity, float duration)
        {
            // PiShock only works with ints
            Vibrate(Round(duration), Round(intensity));
        }

        /// <inheritdoc/>
        public void Test(float intensity, float duration)
        {
            // Send a beep for testing, beeps don't support intensity, which is fine
            Beep(Round(duration));
        }

        /// <summary>
        /// Rounds the provided value to send it to the PiShock API
        /// The PiShock API only works with ints so we need to round floats, but we also want to round 0 values up so we still get some feedback
        /// </summary>
        /// <param name="value">The value to round</param>
        /// <returns></returns>
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
        /// <param name="duration">The durtion of the shock in seconds, should be a value 0 to 15</param>
        /// <param name="intensity">The intensity of the shock, should be a value 0 to 100</param>
        private async void Shock(int duration, int intensity)
        {
            // Ignore empty requests, this will make the API return an error anyways
            if ((duration <= 0) || (intensity <= 0))
            {
                return;
            }

            try
            {
                // Clamp the values passed to the API as it will error out (or at least not behave as expected) if they are out of bounds
                duration = Math.Min(duration, 15);
                intensity = Math.Min(intensity, 100);

                var PayloadData = new
                {
                    Username = DeviceManager.PiShockPreferences.Username,
                    Apikey = DeviceManager.PiShockPreferences.APIKey,
                    Code = DeviceManager.PiShockPreferences.ShareCode,
                    Name = ApiRequestName,
                    Op = 0,
                    Duration = duration,
                    Intensity = intensity
                };
                StringContent JsonContent = new StringContent(JsonConvert.SerializeObject(PayloadData), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("", JsonContent);
                HandleResponse(response);
            }
            catch (Exception)
            {
                // Intentionally blank, if a PiShock device is disposed while a request is in flight it will throw as the task will be canceled- this is fine
            }
        }

        /// <summary>
        /// Send a vibration to the PiShock device
        /// </summary>
        /// <param name="duration">The durtion of the vibration in seconds, should be a value greater than 0</param>
        /// <param name="intensity">The intensity of the shock, should be a value 0 to 100</param>
        private async void Vibrate(int duration, int intensity)
        {
            // Ignore empty requests, this will make the API return an error anyways
            if ((duration <= 0) || (intensity <= 0))
            {
                return;
            }

            try
            {
                // Clamp the values passed to the API as it will error out (or at least not behave as expected) if they are out of bounds
                // (Documentation doesn't say vibrates need their duration clamped but testing the device/API it does not behave as expected above 15)
                duration = Math.Min(duration, 15);
                intensity = Math.Min(intensity, 100);

                var PayloadData = new
                {
                    Username = DeviceManager.PiShockPreferences.Username,
                    Apikey = DeviceManager.PiShockPreferences.APIKey,
                    Code = DeviceManager.PiShockPreferences.ShareCode,
                    Name = ApiRequestName,
                    Op = 1,
                    Duration = duration,
                    Intensity = intensity
                };
                StringContent JsonContent = new StringContent(JsonConvert.SerializeObject(PayloadData), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("", JsonContent);
                HandleResponse(response);
            }
            catch (Exception)
            {
                // Intentionally blank, if a PiShock device is disposed while a request is in flight it will throw as the task will be canceled- this is fine
            }
        }

        /// <summary>
        /// Send a beep to the PiShock device
        /// </summary>
        /// <param name="duration">The durtion of the beepin seconds, should be a value greater than 0</param>
        public async void Beep(int duration)
        {
            // Ignore empty requests, this will make the API return an error anyways
            if (duration <= 0)
            {
                return;
            }

            try
            {
                // Clamp the values passed to the API as it will error out (or at least not behave as expected) if they are out of bounds
                // (Documentation doesn't say beeps need their duration clamped but testing the device/API it does not behave as expected above 15)
                duration = Math.Min(duration, 15);

                var PayloadData = new
                {
                    Username = DeviceManager.PiShockPreferences.Username,
                    Apikey = DeviceManager.PiShockPreferences.APIKey,
                    Code = DeviceManager.PiShockPreferences.ShareCode,
                    Name = ApiRequestName,
                    Op = 2,
                    Duration = duration
                };
                StringContent JsonContent = new StringContent(JsonConvert.SerializeObject(PayloadData), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("", JsonContent);
                HandleResponse(response);
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
