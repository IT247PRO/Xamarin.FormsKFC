using FoodApp.Models;
using FoodApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceOrderPage : ContentPage
    {
        private readonly ApiService _service;
        private double _totalPrice;
        public PlaceOrderPage(double totalPrice)
        {
            InitializeComponent();
            _service = new ApiService();
            _totalPrice = totalPrice;
        }

        private async void BtnPlaceOrder_Clicked(object sender, EventArgs e)
        {
            var order = new Order()
            {
                FullName = EntName.Text,
                Phone = EntPhone.Text,
                Address = EntAddress.Text,
                OrderTotal = _totalPrice,
                UserId = Preferences.Get("userId", 0)
            };
            var response = await _service.PlaceOrder(order);
            if (response != null)
            {
                await DisplayAlert("", $"Your Order Placed Successfully and Your Order ID is {response.orderId}", "Alright");
                Application.Current.MainPage = new NavigationPage(new HomePage());

            }
            else
                await DisplayAlert("Oops", $"Something went wrong", "Alright");
        }

        
    }
}