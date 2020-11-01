using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

using FitnessTrackerApi.Models;
namespace FitnessTracker
{
    public class EntryHttpMethods
    {
        public static async Task<List<Entry>> GetEntries(HttpClient client, int userId, int entryCount)
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:5000/api/entry/");

            List<Entry> entries = JsonConvert.DeserializeObject<List<Entry>>(await response.Content.ReadAsStringAsync());
            int currentCount = 0;
            List<Entry> returnEntries = new List<Entry>();
            foreach (Entry entry in entries)
            {
                if (entry.UserId == userId)
                {
                    returnEntries.Add(entry);
                    currentCount++;

                    if (currentCount == entryCount)
                    {
                        break;
                    }
                }
            }

            return returnEntries;
        }

        public static async Task<string> AddEntry(HttpClient client, Entry entry)
        {
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(entry), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:5000/api/entry/", stringContent);

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> UpdateEntry(HttpClient client, Entry entry)
        {
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(entry), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("https://localhost:5000/api/entry/", stringContent);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
