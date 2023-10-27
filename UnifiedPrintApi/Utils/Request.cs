using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class Request
    {
        public static byte[] Get(Uri uri) => Get(uri, new());

        public static async Task<byte[]> GetAsync(Uri uri) => await GetAsync(uri, new());

        public static byte[] Get(Uri uri, Dictionary<string, string> headers)
        {
            using (var client = new WebClient())
            {
                foreach (var kv in headers)
                    client.Headers[kv.Key] = kv.Value;
                return client.DownloadData(uri);
            }
        }

        public static async Task<byte[]> GetAsync(Uri uri, Dictionary<string, string> headers)
        {
            using (var client = new WebClient())
            {
                foreach (var kv in headers)
                    client.Headers[kv.Key] = kv.Value;
                
                return await client.DownloadDataTaskAsync(uri);
            }  
        }

        public static string GetString(Uri uri) => GetString(uri, new());

        public static async Task<string> GetStringAsync(Uri uri) => await GetStringAsync(uri, new());

        public static string GetString(Uri uri, Dictionary<string, string> headers)
        {
            Console.WriteLine($"Sending request to {uri}");

            using (var client = new HttpClient())
            {
                foreach (var kv in headers)
                    client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                
                var response = client.GetAsync(uri).GetAwaiter().GetResult();
                Console.WriteLine($"Response ({response.StatusCode})");
                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
        }

        public static async Task<string> GetStringAsync(Uri uri, Dictionary<string, string> headers)
        {
            using (var client = new WebClient())
            {
                foreach (var kv in headers)
                    client.Headers[kv.Key] = kv.Value;

                return await client.DownloadStringTaskAsync(uri);
            }
        }
        
        public static string PostString(Uri uri, string data)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers["Content-Type"] = "application/json";
                    return client.UploadString(uri, data);
                }
            }
            catch
            {
                ProxySender proxySender = ProxySender.GetDefault();
                return proxySender.Post(uri, data).GetAwaiter().GetResult();
            }
        }

        public static async Task<string> PostStringAsync(Uri uri, string data)
        {
            using (var client = new WebClient())
            {
                client.Headers["Content-Type"] = "application/json";
                return await client.UploadStringTaskAsync(uri, data);
            }
        }
    }
}
