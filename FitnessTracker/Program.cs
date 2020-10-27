using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

using FitnessTrackerApi.Models;

namespace FitnessTracker
{
    class Program
    {
        private static HttpClient _client = new HttpClient();

        static async Task Main(string[] args)
        {
            int userId;
            do
            {
                userId = await Login();
                if (userId == -1)
                {
                    Console.WriteLine("End of application.");
                    return;
                }
            }
            while (userId == 0);

            await OptionsMenu(userId);
        }


        private static async Task<int> Login()
        {
            string username, password;
            do
            {
                Console.WriteLine("Please enter a valid username");
                username = Console.ReadLine();

                Console.WriteLine("Please enter a valid password");
                password = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password));

            List<User> users = JsonConvert.DeserializeObject<List<User>>(await UserHttpMethods.GetUsers(_client));
            foreach (User user in users)
            {
                if (username == user.Username)
                {
                    int userId = user.UserId;
                    if (password == user.Password)
                    {
                        if (user.CurrentlySubscribed)
                        {
                            return user.UserId;
                        }
                        else
                        {
                            Console.WriteLine("You are currently unsubscribed. Would you like to resubscribe? (y/n)");
                            string resubscribeUser = Console.ReadLine().ToLower();

                            while (string.IsNullOrWhiteSpace(resubscribeUser))
                            {
                                Console.WriteLine("Invalid response. Would you like to resubscribe? (y/n)");
                                resubscribeUser = Console.ReadLine().ToLower();
                            }

                            if (resubscribeUser == "y")
                            {
                                await Resubscribe(user);

                                return user.UserId;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect password.");
                        return 0;
                    }
                }
            }

            Console.WriteLine("You are not in the database, would you like to be added? (y/n)");
            string addUser = Console.ReadLine().ToLower();

            while (string.IsNullOrWhiteSpace(addUser))
            {
                Console.WriteLine("Invalid response. Would you like to be added to the database? (y/n)");
                addUser = Console.ReadLine().ToLower();
            }

            if (addUser == "y")
            {
                await Subscribe(users);

                List<User> newUsers = JsonConvert.DeserializeObject<List<User>>(await UserHttpMethods.GetUsers(_client));
                return newUsers[newUsers.Count - 1].UserId;
            }
            return -1;
        }


        private static async Task Subscribe(List<User> users)
        {
            Console.WriteLine("Please enter a username");
            string username = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Invalid response. Please enter a valid username.");
                username = Console.ReadLine();
            }

            bool isUniqueUsername = false;
            while (!isUniqueUsername)
            {
                isUniqueUsername = true;
                foreach (User user in users)
                {
                    if (user.Username == username)
                    {
                        isUniqueUsername = false;

                        Console.WriteLine("That username is taken. Please enter another username");
                        username = Console.ReadLine();

                        while (string.IsNullOrWhiteSpace(username))
                        {
                            Console.WriteLine("Invalid response. Please enter a valid username.");
                            username = Console.ReadLine();
                        }
                        break;
                    }
                }
            }
            
            Console.WriteLine("Please enter a password");
            string password = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Invalid response. Please enter a valid password.");
                password = Console.ReadLine();
            }

            User newUser = new User() { Username = username, Password = password };
            await UserHttpMethods.AddUser(_client, newUser);
        }

        private static async Task Resubscribe(User user)
        {
            user.CurrentlySubscribed = true;

            await UserHttpMethods.UpdateUser(_client, user);
            Console.WriteLine("Successfully resubscribed.");
        }

        private static async Task Unsubscribe(int userId)
        {
            User currentUser = JsonConvert.DeserializeObject<User>(await UserHttpMethods.GetUserById(_client, userId));
            currentUser.CurrentlySubscribed = false;

            await UserHttpMethods.UpdateUser(_client, currentUser);
            Console.WriteLine("Successfully unsubscribed.");
        }


        private static async Task OptionsMenu(int userId)
        {
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("(1) Update fitness log");
            Console.WriteLine("(2) Check fitness log");
            Console.WriteLine("(3) Unsubscribe");
            Console.WriteLine("(4) Logout");
            Console.WriteLine();

            string selection;
            do
            {
                Console.WriteLine("Type a number corresponding with your action");
                selection = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(selection))
                {
                    Console.WriteLine("Invalid response. Please enter a valid selection.");
                    selection = Console.ReadLine();
                }

                if (selection == "1")
                {
                    await WriteEntry(userId);
                }
                else if (selection == "2")
                {
                    await GetEntry(userId);
                }
                else if (selection == "3")
                {
                    await Unsubscribe(userId);
                    break;
                }

                Console.WriteLine();
            }
            while (selection != "4");

            Console.WriteLine("Logged out.");
        }

        private static async Task WriteEntry(int userId)
        {
            Console.WriteLine("What activity?");
            string activity = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(activity))
            {
                Console.WriteLine("Invalid response. Please enter a valid activity.");
                activity = Console.ReadLine();
            }

            Console.WriteLine("Amount of the activity?");
            string amount = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(amount))
            {
                Console.WriteLine("Invalid response. Please enter a valid username.");
                amount = Console.ReadLine();
            }

            Entry newEntry = new Entry() { UserId = userId, Activity = activity, Amount = amount };
            await EntryHttpMethods.AddEntry(_client, newEntry);
        }

        private static async Task GetEntry(int userId)
        {
            int entryCount;
            do
            {
                Console.WriteLine("How many entries would you like?");
            }
            while (!Int32.TryParse(Console.ReadLine(), out entryCount));

            List<Entry> entries = await EntryHttpMethods.GetEntries(_client, userId, entryCount);

            if (entries.Count == 0)
            {
                Console.WriteLine("No entries found by this user.");
            }
            else
            {
                foreach (Entry entry in entries)
                {
                    Console.WriteLine($"{entry.Activity} for {entry.Amount} on {entry.Timestamp}");
                }
            }
        }
    }
}
