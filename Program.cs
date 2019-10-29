using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientSample
{

    /*Project methods return status code for successful request and for response.IsSuccessStatusCode
       console information*/

    /// <summary>
    /// Helper class for project with Json basic parameters
    /// </summary>
    public class UsersDetails
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }        
    }


    /// <summary>
    /// This class is for to put Json to obj, it's not clear for now.
    /// </summary>
    public class Users
    {
        public List<UsersDetails> result { get; set; }
    }

    /// <summary>
    /// Main class for project
    /// </summary>
    class Program
    {        
        static HttpClient client = new HttpClient();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        static async Task<Uri> CreateProductAsync(UsersDetails details)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "public-api/users", details);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("POST method completed.");
            }
            return response.Headers.Location;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static async Task<UsersDetails> GetProductAsync(string path)
        {
            UsersDetails details = null;
            HttpResponseMessage response = await client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("GET method completed.");
            }
            return details;
        }

        static async Task<HttpStatusCode> UpdateProductAsync()
        {
            UsersDetails details = new UsersDetails();
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"public-api/users/7", details.last_name = "Stevens");
            response.EnsureSuccessStatusCode();            
            details = await response.Content.ReadAsAsync<UsersDetails>();
            return response.StatusCode;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"public-api/users/{id}");
            return response.StatusCode;
        }        

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }        

        static async Task RunAsync()
        {///            
            client.BaseAddress = new Uri("http://gorest.co.in/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            

            try
            {
                // Create a new product
                UsersDetails details = new UsersDetails
                {
                    id = "99",
                    first_name = "Maria",
                    last_name = "Hubs"
                };

                var url = await CreateProductAsync(details);                

                // Get the product
                details = await GetProductAsync("public-api/users");

                // Update the product              
                var statusUpdatedCode = await UpdateProductAsync();
                Console.WriteLine($"Updated (HTTP Status = {(int)statusUpdatedCode})");

                // Delete the product
                string userid = "5";
                var statusCode = await DeleteProductAsync(userid);
                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }        
    }
}