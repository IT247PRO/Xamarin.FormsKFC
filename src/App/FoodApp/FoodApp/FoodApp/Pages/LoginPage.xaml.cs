using FoodApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly ApiService _service;
        public LoginPage()
        {
            InitializeComponent();
            _service = new ApiService();
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            var response = await _service.Login(EntEmail.Text, EntPassword.Text);
            if (response)
            {
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                await DisplayAlert("Oops", "somthing ent wrong", "Cancel");
            }
        }

        private void TapBackArrow_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}