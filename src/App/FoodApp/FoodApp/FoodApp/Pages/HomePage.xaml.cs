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
    public partial class HomePage : ContentPage
    {
        private readonly ApiService _service;
        public ObservableCollection<PopularProduct> _ProductCollection;
        public ObservableCollection<Category> _CategoryCollection;
        public HomePage()
        {
            InitializeComponent();
            _service = new ApiService();
            _ProductCollection = new ObservableCollection<PopularProduct>();
            _CategoryCollection = new ObservableCollection<Category>();
            GetPopularroducts();
            GetCategories();
            LblUserName.Text = Preferences.Get("userName", string.Empty);
        }

        private async void GetCategories()
        {
            var categories = await _service.GetCategories();
            foreach(var cat in categories)
            {
                _CategoryCollection.Add(cat);
            }
            CvCategories.ItemsSource = _CategoryCollection;
        }

        private async void GetPopularroducts()
        {
            var products = await _service.GetPopularProducts();
            foreach(var prod in products)
            {
                _ProductCollection.Add(prod);
            }
            CvProducts.ItemsSource = _ProductCollection;
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {

            GridOverlay.IsVisible = true;
            await SlMenu.TranslateTo(0, 0, 400, Easing.Linear);          

        }

        private void TapCloseMenu_Tapped(object sender, EventArgs e)
        {
            CloseHamBurgerMenu();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var response = await _service.GetTotalCartItems(Preferences.Get("userId", 0));
            LblTotalItems.Text = response.totalItems.ToString();
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CloseHamBurgerMenu();

        }
        private void CvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection  = e.CurrentSelection.FirstOrDefault() as Category;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ProductListPage(currentSelection.id, currentSelection.name));
            ((CollectionView)sender).SelectedItem = null;
        }

        private void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as PopularProduct;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ProductDetailPage(currentSelection.id));
            ((CollectionView)sender).SelectedItem = null;
        }

        private async void TapCartIcon_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CartPage());
        }

        private async void TapOrders_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new OrdersPage());

        }
        private async void CloseHamBurgerMenu()
        {
            await SlMenu.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }

        private async void TapContact_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ContactPage());
        }

        private async void TapCart_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CartPage());
        }

        private void TapLogout_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("accessToken", string.Empty);
            Preferences.Set("expiration_Time", 0);
            Application.Current.MainPage = new NavigationPage(new SignupPage());
        }
    }
}