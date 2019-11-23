using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using TravelRecodrApp.Model;

namespace TravelRecodrApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    var postTable = conn.Table<Post>().ToList();

                    postCountLabel.Text = (from post in postTable
                                           where post.CategoryName != null
                                           select post).ToList().Count.ToString();                                           

                    var categories = (from p in postTable
                                      orderby p.CategoryId
                                      select p.CategoryName).Distinct().ToList();

                    var categories2 = postTable.OrderBy(p => p.CategoryId).Select(p => p.CategoryName).Distinct().ToList();

                    Dictionary<string, int> categoriesCount = new Dictionary<string, int>();

                    foreach (var category in categories2)
                    {
                        var count = (from post in postTable
                                     where post.CategoryName == category
                                     select post).ToList().Count;

                        var count2 = postTable.Where(p => p.CategoryName == category).ToList().Count;

                        categoriesCount.Add(category, count2);
                    }

                    categoriesListView.ItemsSource = categoriesCount;

                    postCountLabel.Text = postTable.Count.ToString();
                }
            }

            catch (NullReferenceException nre)
            {

            }

            catch (Exception ex)
            {

            }

        }

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    using (SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation))
        //    {
        //        connection.DeleteAll<Post>();
        //    }
        //}
    }
}
