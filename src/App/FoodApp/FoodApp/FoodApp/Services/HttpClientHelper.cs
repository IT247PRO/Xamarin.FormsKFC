using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace FoodApp.Services
{
    public class HttpClientHelper<T>
    {
        private static readonly HttpClient Client = new HttpClient();
        /// <summary>
        /// For getting a single item from a web api uaing GET
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api get method, e.g. "products/1" to get a product with an id of 1</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The item requested</returns>
        public async Task<T> GetSingleItemRequest(string apiUrl, CancellationToken cancellationToken)
        {
            var result = default(T);
            var response = await Client.GetAsync(apiUrl, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith(x =>
                {
                    if (typeof(T).Namespace != "System")
                    {
                        result = JsonConvert.DeserializeObject<T>(x?.Result);
                    }
                    else result = (T)Convert.ChangeType(x?.Result, typeof(T));
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        /// <summary>
        /// For getting multiple (or all) items from a web api using GET
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api get method, e.g. "products?page=1" to get page 1 of the products</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The items requested</returns>
        public async Task<T[]> GetMultipleItemsRequest(string apiUrl, CancellationToken cancellationToken)
        {
            T[] result = null;
            var response = await Client.GetAsync(apiUrl, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<T[]>(x.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        /// <summary>
        /// For creating a new item over a web api using POST
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api post method, e.g. "products" to add products</param>
        /// <param name="postObject">The object to be created</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The item created</returns>
        public async Task<T> PostRequest(string apiUrl, T postObject, CancellationToken cancellationToken)
        {
            T result = default(T);
            var response = await Client.PostAsync(apiUrl, 
                                                    new StringContent(JsonConvert.SerializeObject(postObject), Encoding.UTF8, "application/json"),
                                                    cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<T>(x.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        /// <summary>
        /// For updating an existing item over a web api using PUT
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api put method, e.g. "products/3" to update product with id of 3</param>
        /// <param name="putObject">The object to be edited</param>
        /// <param name="cancellationToken"></param>
        public async Task PutRequest(string apiUrl, T putObject, CancellationToken cancellationToken)
        {
            var response = await Client.PutAsync(apiUrl, 
                                                 new StringContent(JsonConvert.SerializeObject(putObject), Encoding.UTF8, "application/json"),
                                                 cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
        }

        /// <summary>
        /// For deleting an existing item over a web api using DELETE
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api delete method, e.g. "products/3" to delete product with id of 3</param>
        /// <param name="cancellationToken"></param>
        public async Task DeleteRequest(string apiUrl, CancellationToken cancellationToken)
        {
            var response = await Client.DeleteAsync(apiUrl, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
        }
    }
}
