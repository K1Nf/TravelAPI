using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TravelAPI
{
    public class PlaceRepository
    {
        SQLiteConnection database;
        public PlaceRepository(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Feature>();
        }
        public IEnumerable<Feature> GetFeatures()
        {
            return database.Table<Feature>().ToList();
        }
        public Feature GetFeature(int id)
        {
            return database.Get<Feature>(id);
        }
        public int DeleteFeature(int id)
        {
            return database.Delete<Feature>(id);
        }
        public int SaveFeature(Feature feature)
        {
            if (feature.Id != 0)
            {
                database.Update(feature);
                return feature.Id;
            }
            else
            {
                return database.Insert(feature);
            }
        }
    }
}
