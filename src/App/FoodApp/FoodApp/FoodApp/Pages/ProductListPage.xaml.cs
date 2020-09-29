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
    public partial class ProductListPage : ContentPage
    {
        private readonly ApiService _service;
        public ObservableCollection<Product> _ProductCollection;
        public ProductListPage(int categoryId, string categoryName)
        {
            InitializeComponent();
            _service = new ApiService();
            _ProductCollection = new ObservableCollection<Product>();
            LblCategoryName.Text = categoryName;
            GetProducts(categoryId);
        }

        private async void GetProducts(int categoryId)
        {
           var products = await _service.GetProductByCategory(categoryId);
            foreach(var prod in products)
            {
                _ProductCollection.Add(prod);
            }
            CvProducts.ItemsSource = _ProductCollection;
        }

        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as Product;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ProductDetailPage(currentSelection.id));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}