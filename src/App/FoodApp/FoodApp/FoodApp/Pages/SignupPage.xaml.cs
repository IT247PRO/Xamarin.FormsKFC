using FoodApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        private readonly ApiService _service;
        public SignupPage()
        {
            InitializeComponent();
            _service = new ApiService();
        }

        private async void BtnSignUp_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!EntPassword.Text.Equals(EntConfirmPassword.Text))
                {
                    await DisplayAlert("Password mismatch", "Password and Confirm Password din't match", "Alright");
                }
                else
                {
                    var response = await _service.RegisterUser(EntName.Text, EntEmail.Text, EntPassword.Text);
                    if (response)
                    {
                        await DisplayAlert("Hi", "Your account has been created", "Alright");
                        await Navigation.PushModalAsync(new LoginPage());
                    }
                    else
                    {
                        await DisplayAlert("Oops", "Something went wrong", "Alright");
                    }
                }
            }catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Alright");
            }
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }
    }
}