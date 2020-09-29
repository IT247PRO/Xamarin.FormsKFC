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
    public partial class ProductDetailPage : ContentPage
    {
        private readonly ApiService _service;
        private readonly int _productId;
        public ProductDetailPage(int productId)
        {
            InitializeComponent();
            _service = new ApiService();
            _productId = productId;
            GetProductDetail(productId);
        }

        private async void GetProductDetail(int productId)
        {
            var product  = await _service.GetProductById(productId);
            LblName.Text = product.name;
            LblDetail.Text = product.detail;
            ImgProduct.Source = product.FullImageUrl;
            LblPrice.Text = product.price.ToString();
            LblTotalPrice.Text = LblPrice.Text;
        }

        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void TapDecrement_Tapped(object sender, EventArgs e)
        {
            var qty = Convert.ToInt32(LblQty.Text);
            qty--;
            if (qty == 0)
                qty = 1;
            LblQty.Text = qty.ToString();
            LblTotalPrice.Text = (qty * Convert.ToInt16(LblPrice.Text)).ToString();
        }

        private void TapIncrement_Tapped(object sender, EventArgs e)
        {
            var qty = Convert.ToInt16(LblQty.Text);
            qty++;
            LblQty.Text = qty.ToString();
            LblTotalPrice.Text = (qty * Convert.ToInt16(LblPrice.Text)).ToString();
        }

        private async void BtnAddToCart_Clicked(object sender, EventArgs e)
        {
            var addToCart = new AddToCart
            {
                CustomerId = Preferences.Get("userId", 0),
                Price = LblPrice.Text,
                TotalAmount = LblTotalPrice.Text,
                ProductId = _productId,
                Qty = LblQty.Text
            };

            var result= await _service.AddItemInCart(addToCart);
            if (result)
                await DisplayAlert("", $"{LblName.Text} added to your cart.", "Alright");
            else
                await DisplayAlert("Ooops", "Someting went wrong.", "Alright");
        }
    }
}