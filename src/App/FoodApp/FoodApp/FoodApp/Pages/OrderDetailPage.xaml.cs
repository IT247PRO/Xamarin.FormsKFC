using FoodApp.Models;
using FoodApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailPage : ContentPage
    {
        private readonly ApiService _service;
        private ObservableCollection<OrderDetail> _orderDetailCollection;
        public OrderDetailPage(int orderId)
        {
            InitializeComponent();
            _service = new ApiService();
            _orderDetailCollection = new ObservableCollection<OrderDetail>();
            GetOrderDetail(orderId);
        }

        private async void GetOrderDetail(int orderId)
        {

            var orders = await _service.GetOrderDetails(orderId);
            var order = orders.FirstOrDefault();
            foreach (var ord in order.orderDetails)
            {
                _orderDetailCollection.Add(ord);
            }
            LvOrderDetail.ItemsSource = _orderDetailCollection;
            LblTotalPrice.Text = order.OrderTotal.ToString("c");
        }

        private async void TapBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}