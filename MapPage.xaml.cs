using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Permissions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SQLite;
using TravelRecodrApp.Model;

namespace TravelRecodrApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private bool hasLocationPermission = false;

        [Obsolete]
        public MapPage()
        {
            InitializeComponent();
            GetPermissions();
        }

        [Obsolete]
        private async void GetPermissions()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.LocationWhenInUse);
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    await DisplayAlert("Need location", "Need to access location", " OK");

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.LocationWhenInUse);
                    if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.LocationWhenInUse))
                        status = results[Plugin.Permissions.Abstractions.Permission.LocationWhenInUse];
                }

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    hasLocationPermission = true;
                    locationsMap.IsShowingUser = true;

                    GetLocation();
                }
                else
                {
                    await DisplayAlert("Location Permission Denied", "No Permission", "OK");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (hasLocationPermission)
            {
                var locator = CrossGeolocator.Current;
                locator.PositionChanged += Locator_PositionChanged;
                await locator.StartListeningAsync(TimeSpan.Zero, 100);
            }

            GetLocation();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                var posts = conn.Table<Post>().ToList();

                DisplayInMap(posts);
            }
        }

        private void DisplayInMap(List<Post> posts)
        {
            try
            {
                foreach (var post in posts)
                {
                    var position = new Xamarin.Forms.Maps.Position(post.Latitude, post.Longitude);
                    var pin = new Xamarin.Forms.Maps.Pin()
                    {
                        Type = Xamarin.Forms.Maps.PinType.SavedPin,
                        Position = position,
                        Label = post.VenueName,
                        Address = post.Address
                    };

                    locationsMap.Pins.Add(pin);
                }
            }
            catch (NullReferenceException nre)
            {

            }
            catch (Exception ex)
            {

            }
            
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            CrossGeolocator.Current.StopListeningAsync();
            CrossGeolocator.Current.PositionChanged -= Locator_PositionChanged;
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            MoveMap(e.Position);
        }

        private async void GetLocation()
        {
            if (hasLocationPermission)
            {
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync();

                MoveMap(position);

            }
        }

        private void MoveMap(Position position)
        {
            //locationsMap.MoveToRegion(new Xamarin.Forms.Maps.MapSpan(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), 1, 1));
            var center = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
            var span = new Xamarin.Forms.Maps.MapSpan(center, 1, 1);
            locationsMap.MoveToRegion(span);
        }
    }
}