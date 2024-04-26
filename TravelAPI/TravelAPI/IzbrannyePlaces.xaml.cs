//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelAPI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IzbrannyePlaces : ContentPage
    {
        public static string PATH = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "p.json");
        public IzbrannyePlaces()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            List<Feature>? features = new List<Feature>();

            try
            {
                using (FileStream fs = new FileStream(PATH, FileMode.OpenOrCreate))
                {
                    features = await JsonSerializer.DeserializeAsync<List<Feature>?>(fs);
                }
            }
            catch (JsonException ex)
            {}
            finally
            {
                placesList.ItemsSource = features;
            }

            
            base.OnAppearing();
        }

        private async void SelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            Feature selectedFeature = (Feature)e.SelectedItem;
            PlacePage placePage = new PlacePage(selectedFeature, false);
            placePage.BindingContext = selectedFeature;
            await Navigation.PushAsync(placePage);
        }
    }
}