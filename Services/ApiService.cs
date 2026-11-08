
using System.Net.Http.Json;
using raceDay_Part3.Models;

namespace raceDay_Part3.Services
{//start of namespace
    public class ApiService
    {//start of class
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> RegisterAsync(RegisterViewModel model)
        {//start of task method
            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/register",
                new
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    Role = model.Role
                });

            return await response.Content.ReadAsStringAsync();
        }//end of task method

        public async Task<(bool Success, string Message, string? Cookie, string? UserData)>
            LoginAsync(LoginViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/login",
                new
                {
                    Email = model.Email,
                    Password = model.Password
                });

            string message = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return (false, message, null, null);
            }

            string? cookie = null;

            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                cookie = cookies
                    .FirstOrDefault(c => c.StartsWith(".AspNetCore.Session="))
                    ?.Split(';')[0];
            }

            return (true, message, cookie, message);
        }

        public async Task<string> LogoutAsync(string? cookie)
        {//start of logout method
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "api/auth/logout");

            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }//end of logout method
    }//end of class
}//end of namespace
