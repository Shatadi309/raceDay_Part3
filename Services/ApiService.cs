
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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

        // Get all events
        public async Task<List<EventViewModel>> GetEventsAsync()
        {
            var response = await _httpClient.GetAsync("api/events");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<EventViewModel>>()
                   ?? new List<EventViewModel>();
        }

        // Create a new event

        // Create a new event
        public async Task<string> CreateEventAsync(
            EventViewModel model, string? cookie)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "api/events");

            request.Content = JsonContent.Create(new
            {
                EventName = model.EventName,
                EventDescription = model.EventDescription,
                EventDate = model.EventDate,
                Location = model.Location,
                EntryFee = model.EntryFee,
                CategoryID = model.CategoryID
            });

            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }


        // Update an existing event
        public async Task<string> UpdateEventAsync(
            EventViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/events/{model.EventID}",
                new
                {
                    EventName = model.EventName,
                    EventDescription = model.EventDescription,
                    EventDate = model.EventDate,
                    Location = model.Location,
                    EntryFee = model.EntryFee
                });

            return await response.Content.ReadAsStringAsync();
        }

        // Delete an event
        public async Task<string> DeleteEventAsync(int eventID)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/events/{eventID}");

            return await response.Content.ReadAsStringAsync();
        }

        // Get one event by ID
        public async Task<EventViewModel?> GetEventByIdAsync(int eventID)
        {
            var response = await _httpClient.GetAsync(
                $"api/events/{eventID}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<EventViewModel>();
        }

        // Get all event categories
        public async Task<List<CategoryViewModel>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("api/categories");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>()
                   ?? new List<CategoryViewModel>();
        }

        // Enrol a participant in an event
        public async Task<(bool Success, string Message)> EnrolInEventAsync(
            int eventID, int categoryID, decimal paymentAmount, string? cookie)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"api/events/{eventID}/enrolment");

            request.Content = JsonContent.Create(new
            {
                EventCategoryID = categoryID,
                PaymentAmount = paymentAmount,
                PaymentStatus = "Pending"
            });

            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await _httpClient.SendAsync(request);

            string responseBody = await response.Content.ReadAsStringAsync();

            return (response.IsSuccessStatusCode, responseBody);
        }

        // Get a participant's own enrolments
        public async Task<List<EnrolmentViewModel>> GetMyEnrolmentsAsync(string? cookie)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/enrolment/me");

            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<EnrolmentViewModel>>()
                   ?? new List<EnrolmentViewModel>();
        }//end of participant's enrolment method

    }//end of class
}//end of namespace
