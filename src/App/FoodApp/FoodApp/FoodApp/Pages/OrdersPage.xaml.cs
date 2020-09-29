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
    public partial class OrdersPage : ContentPage
    {
        private readonly ApiService _service;
        private ObservableCollection<OrderByUser> _orderByUserCollection;
        public OrdersPage()
        {
            InitializeComponent();
            _service = new ApiService();
            _orderByUserCollection = new ObservableCollection<OrderByUser>();
            GetUserOrders();
        }

        private async void GetUserOrders()
        {
            var userid = Preferences.Get("userId", 0);
            var ordersByUser = await _service.GetOrdersByUser(userid);
            foreach(var order in ordersByUser)
            {
                _orderByUserCollection.Add(order);
            }
            LvOrders.ItemsSource = _orderByUserCollection;
        }

        private async void TapBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void LvOrders_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //var currentSelection = e..FirstOrDefault() as PopularProduct;
            //if (currentSelection == null) return;
            //Navigation.PushModalAsync(new ProductDetailPage(currentSelection.id));
            //((CollectionView)sender).SelectedItem = null;
        }

        private void LvOrders_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var currentSelection = e.SelectedItem as OrderByUser;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new OrderDetailPage(currentSelection.id));
            ((ListView)sender).SelectedItem = null;
        }
    }
}