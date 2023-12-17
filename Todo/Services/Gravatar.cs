using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Todo.Models.Gravatar;

namespace Todo.Services
{
    public static class Gravatar
    {
        private static readonly HttpClient Client = new(); 
        
        /// <summary>
        ///     A helper method which attempts to retrieve the Gravatar profile for the given email address
        ///     and extract the Display Name from the profile.
        /// 
        ///     If it fails the method just returns the input.
        /// </summary>
        /// <param name="emailAddress">Email address to check the Gravatar Profile of.</param>
        /// <returns>Display Name of Gravatar profile or passes through input.</returns>
        public static async Task<string> GetUsername(string emailAddress)
        {
            var displayName = emailAddress; // default to email address

            try
            {
                var jsonProfileUrl = $"https://www.gravatar.com/{GetHash(emailAddress)}.json";
                Client.DefaultRequestHeaders.Add("User-Agent","Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 1.1.4322; .NET CLR 3.5.20404)");

                var response = await Client.GetAsync(jsonProfileUrl);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<GravatarResponse>(responseBody);
                displayName = json.Entry[0].DisplayName;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to retrieve Gravatar{e.Message}");
            }
            
            return displayName; // Return email address if failed to read user name data
        }

        public static string GetHash(string emailAddress)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.Default.GetBytes(emailAddress.Trim().ToLowerInvariant());
                var hashBytes = md5.ComputeHash(inputBytes);

                var builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("X2"));
                }
                return builder.ToString().ToLowerInvariant();
            }
        }
    }
}