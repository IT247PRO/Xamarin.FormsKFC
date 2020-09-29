using FoodApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnixTimeStamp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodApp.Services
{
   public class ApiService
    {
        public string baseUrl { get; set; }
        public ApiService()
        {
            baseUrl = AppSettings.ApiUrl + "api/";
        }

        #region Account
        public async Task<bool> RegisterUser(string name, string email, string password)
        {
            var register = new Register()
            {
                Name = name,
                Email = email,
                Password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(register);
            var contant  = new StringContent(json, Encoding.UTF8, "application/json");
           var response =  await httpClient.PostAsync(baseUrl + "Accounts/Register", contant);
            
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public async Task<bool> Login(string email, string password)
        {
            var token = new Token();
            var login = new Login()
            {
                Email = email,
                Password = password

            };

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(login);
            var contant = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(baseUrl + "Accounts/Login", contant);
            if (!response.IsSuccessStatusCode) return false;


            var jsonResult = await response.Content.ReadAsStringAsync();
            token = JsonConvert.DeserializeObject<Token>(jsonResult);
            Preferences.Set("email", email);
            Preferences.Set("password", password);
            Preferences.Set("accessToken", token.access_token);
            Preferences.Set("userId", token.user_Id);
            Preferences.Set("userName", token.user_name);
            Preferences.Set("expiration_Time", token.expiration_Time);
            Preferences.Set("currentTime", UnixTime.GetCurrentTime());

            return true;

        }
        #endregion

        #region Product
        public async Task<List<Category>> GetCategories()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var response = await httpClient.GetAsync(baseUrl + "Categories");            
            if (!response.IsSuccessStatusCode) return null;


            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Category>>(jsonResult);

        }

        public async Task<Product> GetProductById(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var response = await httpClient.GetAsync(baseUrl + "Products/" + id);
            if (!response.IsSuccessStatusCode) return null;


            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(jsonResult);

        }

        public async Task<List<Product>> GetProductByCategory(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var response = await httpClient.GetAsync(baseUrl + "Products/ProductsByCategory/" + id);
            if (!response.IsSuccessStatusCode) return null;


            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Product>>(jsonResult);

        }

        public async Task<List<PopularProduct>> GetPopularProducts()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var response = await httpClient.GetAsync(baseUrl + "Products/PopularProducts");
            if (!response.IsSuccessStatusCode) return null;


            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PopularProduct>>(jsonResult);

        }
        #endregion

        #region Cart
        public async Task<bool> AddItemInCart(AddToCart entity)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var json = JsonConvert.SerializeObject(entity);
            var contant = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(baseUrl + "ShoppingCartItems", contant);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public async Task<CartSubTotal> GetCartSubTotal(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var response = await httpClient.GetAsync(baseUrl + "ShoppingCartItems/SubTotal/" + id);
            if (!response.IsSuccessStatusCode) return null;


            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CartSubTotal>(jsonResult);

        }
        public async Task<List<ShoppingCartItem>> GetShoppingCartItems(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var response = await httpClient.GetAsync(baseUrl + "ShoppingCartItems/" + id);
            if (!response.IsSuccessStatusCode) return null;


            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ShoppingCartItem>>(jsonResult);

        }

        public async Task<TotalCartItem> GetTotalCartItems(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var response = await httpClient.GetAsync(baseUrl + "ShoppingCartItems/TotalItems/" + id);
            if (!response.IsSuccessStatusCode) return null;


            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TotalCartItem>(jsonResult);

        }

        public async Task<bool> ClearShoppingCart(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var response = await httpClient.DeleteAsync(baseUrl + "ShoppingCartItems/" + id);
            if (!response.IsSuccessStatusCode) return false;

            return true;
            

        }
        #endregion

        #region Order
        public async Task<OrderResponse> PlaceOrder(Order entity)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var json = JsonConvert.SerializeObject(entity);
            var contant = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(baseUrl + "Orders", contant);
            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OrderResponse>(jsonResult);
        }

        public async Task<List<OrderByUser>> GetOrdersByUser(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var response = await httpClient.GetAsync(baseUrl + "Orders/OrdersByUser/" + id);
            if (!response.IsSuccessStatusCode) return null;


            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<OrderByUser>>(jsonResult);

        }
        public async Task<List<Order>> GetOrderDetails(int id)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", String.Empty));
            var response = await httpClient.GetAsync(baseUrl + "Orders/OrderDetails/" + id);
            if (!response.IsSuccessStatusCode) return null;


            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Order>>(jsonResult);

        }
        #endregion
    }

    public static class TokenValidator
    {
        public static async Task CheckTokenValidity()
        {
            var expiration_Time = Preferences.Get("expiration_Time", 0);
            Preferences.Set("currentTime", UnixTime.GetCurrentTime());
            var currentTime = Preferences.Get("currentTime", 0);
            if (expiration_Time < currentTime) ;
            {
                var email = Preferences.Get("email", string.Empty);
                var password = Preferences.Get("password", string.Empty);
                await new ApiService().Login(email, password);
            }
        }
    }
}
