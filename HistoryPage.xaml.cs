using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using SQLitePCL;
using TravelRecodrApp.Model;

namespace TravelRecodrApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                using (var conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Post>();
                    var posts = conn.Table<Post>().ToList();
                    postListView.ItemsSource = posts;
                    conn.DeleteAll<Post>();
                }
            }

            catch (NullReferenceException nre)
            {
               
            }

            catch (Exception ex)
            {

            }


        }
    }
}