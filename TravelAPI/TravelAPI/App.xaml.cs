using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelAPI
{
    public partial class App : Application
    {
        public const string DATABASE_NAME = "placess.db";
        public static PlaceRepository database;
        public static PlaceRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new PlaceRepository(
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
