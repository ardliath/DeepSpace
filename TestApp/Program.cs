using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            PostDataAsync();

            Console.ReadKey();
        }

        static async void PostDataAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //var response = await client.PostAsync("http://localhost:51530/api/ship", new StringContent("{name: Adam}"));

                    //var response = await client.PostAsync("http://localhost:51530/api/ship", new StringContent("{name: Adam}", Encoding.UTF8, "application/json"));
                    //var text = response.EnsureSuccessStatusCode();

                    StringContent content = new StringContent("{name: 'Adam'}", Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://localhost:51530/api/ship/create", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        //product = JsonConvert.DeserializeObject<Product>(data);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }   
}
