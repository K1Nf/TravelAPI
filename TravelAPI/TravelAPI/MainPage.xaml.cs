using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TravelAPI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public static readonly HttpClient client = new HttpClient();
        private static string DefUrl = "https://api.geoapify.com/v2/places?apiKey=1135c26d1d0c4f9eb9c506f9f0ccb795&language=en&filter=place:"; //&categories=
        private static string Url;
        //public static string PATH = System.IO.Path.Combine(
        //    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "places.json");


        private async void Button_Clicked(object sender, EventArgs e)
        {
            
            Url = DefUrl;
            GetParams();
            try
            {
                HttpResponseMessage response = await client.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseBody);
                List<Feature> features = myDeserializedClass.features;
                


                foreach(var i in features)
                {
                    try
                    {
                        string[] address = i.properties.address_line2.Split(',');
                        i.properties.address_line2 = address[0];
                        
                    }
                    catch { }
                    try
                    {
                        string[] websites = features[0].properties.datasource.raw.website.Split('%');
                        i.properties.datasource.raw.website = websites[0];
                    }
                    catch { }
                }

                placesList.ItemsSource = features;

                //articles = myDeserializedClass.articles;
                //articles.ForEach(art => art.data = art.publishedAt.ToString());
                //
                //articlesList.ItemsSource = articles;


                //string json = JsonConvert.SerializeObject(articles);
                //var per = JObject.FromObject(articlesList);
                //System.IO.File.WriteAllText(PATH, json);


            }
            catch (HttpRequestException httpErr) { Console.WriteLine($"HTTP error occurred: {httpErr.Message}"); }
            catch (Exception err) { Console.WriteLine($"An error occurred: {err.Message}"); }
        }

        public void GetParams()
        {
            string city;
            switch (CityFilter.SelectedIndex)
            {
                case 0: // Ханты-Мансийск
                    city = "51d7fd3275fa495140598ed5788be5814e40f00101f9019de01a0000000000c00209920339d0b3d0bed180d0bed0b4d181d0bad0bed0b920d0bed0bad180d183d0b320d0a5d0b0d0bdd182d18b2dd09cd0b0d0bdd181d0b8d0b9d181d0ba";
                    break;
                case 1: // Москва
                    city = "51197f918609cf4240597a26eabb11e04b40f00101f901fdfc260000000000c00208";
                    break;
                default:
                    city = "51d7fd3275fa495140598ed5788be5814e40f00101f9019de01a0000000000c00209920339d0b3d0bed180d0bed0b4d181d0bad0bed0b920d0bed0bad180d183d0b320d0a5d0b0d0bdd182d18b2dd09cd0b0d0bdd181d0b8d0b9d181d0ba";
                    break;
            };

            Url += city;

            string category;
            switch (CategoryFilter.SelectedIndex)
            {
                case 0: // супермаркет
                    category = "commercial.supermarket";
                    break;
                case 1: // одежда
                    category = "commercial.clothing";
                    break;
                case 2: // рестораны/кафе
                    category = "catering";
                    break;
                case 3: // развлечения
                    category = "entertainment";
                    break;
                default:
                    category = "commercial.supermarket";
                    break;
            };
            Url += "&categories=" + category;
            Url += "&limit=15";
        }

        private async void SelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            Feature selectedArtice = (Feature)e.SelectedItem;
            PlacePage placePage = new PlacePage(selectedArtice, true);
            placePage.BindingContext = selectedArtice;
            await Navigation.PushAsync(placePage);
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IzbrannyePlaces());
        }
    }


    public class Building
    {
        public int levels { get; set; }
        public string start_date { get; set; }
        public string type { get; set; }
    }

    public class Contact
    {
        public string phone { get; set; }
        public string email { get; set; }
    }

    public class Datasource
    {
        public string sourcename { get; set; }
        public string attribution { get; set; }
        public string license { get; set; }
        public string url { get; set; }
        public Raw raw { get; set; }
    }

    public class Facilities
    {
        public bool wheelchair { get; set; }
    }

    public class Feature
    {
        [PrimaryKey, AutoIncrement]
        [JsonIgnore]
        public int Id { get; set; }
        public string? type { get; set; }
        public Properties properties { get; set; }
        public Geometry? geometry { get; set; }
    }

    public class Geometry
    {
        public string? type { get; set; }
        public List<double?>? coordinates { get; set; }
    }

    public class NameInternational
    {
        public string ru { get; set; }
    }

    public class NameOther
    {
        public string official_name { get; set; }
    }

    public class Properties
    {
        public string? name { get; set; }
        public string? country { get; set; }
        public string? country_code { get; set; }
        public string? region { get; set; }
        public string? state { get; set; }
        public string? county { get; set; }
        public string? city { get; set; }
        public string? postcode { get; set; }
        public string? street { get; set; }
        public string? housenumber { get; set; }
        public double? lon { get; set; }
        public double? lat { get; set; }
        public string? formatted { get; set; }
        public string? address_line1 { get; set; }
        public string? address_line2 { get; set; }
        public List<string?>? categories { get; set; }
        public List<string?>? details { get; set; }
        public Datasource? datasource { get; set; }
        public string? website { get; set; }
        public string? opening_hours { get; set; }
        public string? @operator { get; set; }
        public NameInternational? name_international { get; set; }
        public Contact? contact { get; set; }
        public string? place_id { get; set; }
        public Building? building { get; set; }
        public string? description { get; set; }
        public NameOther? name_other { get; set; }
        public Facilities? facilities { get; set; }
        public string? suburb { get; set; }
    }

    public class Raw
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public object osm_id { get; set; }
        public string amenity { get; set; }

        [JsonProperty("name:ru")]
        public string nameru { get; set; }
        public string website { get; set; }
        public string @operator { get; set; }
        public string osm_type { get; set; }
        public string opening_hours { get; set; }

        [JsonProperty("ref:mkrf_theaters")]
        public int refmkrf_theaters { get; set; }
        public string tourism { get; set; }

        [JsonProperty("addr:city")]
        public string addrcity { get; set; }

        [JsonProperty("addr:street")]
        public string addrstreet { get; set; }

        [JsonProperty("addr:postcode")]
        public int? addrpostcode { get; set; }

        [JsonProperty("addr:housenumber")]
        public object addrhousenumber { get; set; }
        public string building { get; set; }
        public string start_date { get; set; }

        [JsonProperty("addr:country")]
        public string addrcountry { get; set; }

        [JsonProperty("building:levels")]
        public int? buildinglevels { get; set; }
        public string description { get; set; }
        public string official_name { get; set; }
        public string leisure { get; set; }
        public string wheelchair { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }


}
//client.DefaultRequestHeaders.Add("Accept", "application/json");
//client.DefaultRequestHeaders.Add("User-Agent", "NewsAPI.Android");