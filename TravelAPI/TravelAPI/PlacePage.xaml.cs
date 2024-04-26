//using Newtonsoft.Json;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    
    public partial class PlacePage : ContentPage
    {
        public static string PATH = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "p.json");
        Feature feature;
        bool flag;
        public PlacePage(Feature _feature, bool _flag)
        {
            InitializeComponent();
            feature = _feature;
            flag = _flag;
            if (flag)
                RemovePlace.IsVisible = false;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string classid = button.ClassId;

            // чтение
            List<Feature>? features = new List<Feature>();

            try
            {
                using (FileStream fs = new FileStream(PATH, FileMode.OpenOrCreate))
                {
                    features = await JsonSerializer.DeserializeAsync<List<Feature>?>(fs);
                }
            }
            catch(JsonException ex) 
            { 

            }
            finally
            {
                if (classid == "AddPlace")
                {
                    if (features.Any(f => f.properties.address_line2 == feature.properties.address_line2))
                    {
                        labelClick.Text = "Ужe добавлено в избранное!";
                    }
                    else
                    {
                        features.Add(feature);
                        labelClick.Text = "Успешно добавлено в избранное!";
                    }
                }
                else if (classid == "RemovePlace")
                {

                    if (features.Any(f => f.properties.address_line2 == feature.properties.address_line2))
                    {

                        //while(features.Any(f => f.properties.))

                        features.RemoveAll(f => f.properties.address_line2 == feature.properties.address_line2 &&
                        f.properties.name == feature.properties.name);
                        labelClick.Text = "Удалено из избранного!";
                    }
                    else labelClick.Text = "Уже удалено из избранного!";
                }
                else labelClick.Text = "unluko";
            }

            
            // запись
            using (FileStream fs = new FileStream(PATH, FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync<List<Feature>?>(fs, features);
            }

        }
    }
}