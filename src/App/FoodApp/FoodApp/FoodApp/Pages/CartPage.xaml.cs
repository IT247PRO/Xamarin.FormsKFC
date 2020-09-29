using FoodApp.Models;
using FoodApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        private readonly ApiService _service;

        public ObservableCollection<ShoppingCartItem> _shippoingCartCollection;
        public CartPage()
        {
            InitializeComponent();
            _service = new ApiService();
            _shippoingCartCollection = new ObservableCollection<ShoppingCartItem>();
            GetShoppingCartItems();
            GetTotalPrice();
        }

        private async void GetTotalPrice()
        {
            var userid = Preferences.Get("userId", 0);
            var subTotal = await _service.GetCartSubTotal(userid);
            LblTotalPrice.Text = subTotal.subTotal.ToString();
        }

        private async void GetShoppingCartItems()
        {
            var userid = Preferences.Get("userId", 0);
            var shoppingCartItems = await _service.GetShoppingCartItems(userid);            
            foreach(var item in shoppingCartItems)
            {
                _shippoingCartCollection.Add(item);                
            }
            LvShoppingCart.ItemsSource = _shippoingCartCollection;

        }

        private async void TapBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void TapClearCart_Tapped(object sender, EventArgs e)
        {
            var userid = Preferences.Get("userId", 0);
            var result = await _service.ClearShoppingCart(userid);
            if (result)
            {
                await DisplayAlert("", $"Your cart has been cleared", "Alright");
                LvShoppingCart.ItemsSource = null;
                LblTotalPrice.Text = "0";
            }
            else
                await DisplayAlert("Ooops", $"Someting went wrong", "Alright");

        }

        private void BtnProceed_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PlaceOrderPage(Convert.ToDouble(LblTotalPrice.Text)));
        }
    }
}