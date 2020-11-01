using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

using FitnessTrackerApi.Models;

namespace FitnessTracker
{
    public class UserHttpMethods
    {
        public static async Task<string> GetUsers(HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:5000/api/user/");

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> GetUserById(HttpClient client, int userId)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:5000/api/user/{userId}");

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> AddUser(HttpClient client, User user)
        {
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:5000/api/user/", stringContent);

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> UpdateUser(HttpClient client, User user)
        {
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"https://localhost:5000/api/user/{user.UserId}", stringContent);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
