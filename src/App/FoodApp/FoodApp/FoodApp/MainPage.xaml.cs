using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {

            Preferences.Set("userName", entUserName.Text);
        }

        private void btnRetrive_Clicked(object sender, EventArgs e)
        {
            lblRetrive.Text = Preferences.Get("userName",String.Empty);
        }
    }
}
