﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecodrApp;
using TravelRecodrApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLitePCL;
using SQLite;
using Plugin.Geolocator;
using TravelRecodrApp.Logic;

namespace TravelRecodrApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTravelPage : ContentPage
    {
        public NewTravelPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            var venues = await VenueLogic.GetVenues(position.Latitude, position.Longitude);
            venueListView.ItemsSource = venues;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
            var selectedVenue = venueListView.SelectedItem as Venue;
            var firstCategory = selectedVenue.categories.FirstOrDefault();
            Post post = new Post()
            {
                
                Experience = experienceEntry.Text,
                CategoryId = firstCategory.id,
                CategoryName = firstCategory.name,
                Address = selectedVenue.location.address,
                Distance = selectedVenue.location.distance,
                Latitude = selectedVenue.location.lat,
                Longitude = selectedVenue.location.lng,
                VenueName = selectedVenue.name
            };

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                int rows = conn.Insert(post);


                if (rows > 0)
                    DisplayAlert("Success", "Insert operation successful", "Yep");
                else
                    DisplayAlert("Failure", "Insert operation failed", "Nope");

            }

            }

            catch(NullReferenceException nre)
            {

            }

            catch (Exception ex)
            {

            }

        }
    }
}