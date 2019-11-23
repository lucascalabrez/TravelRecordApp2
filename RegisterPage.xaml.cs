using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecodrApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void registerButton_Clicked(object sender, EventArgs e)
        {
            if(passwordEntry.Text == confirmPasswordEntry.Text)
            {
                //Register user
            }
            else
            {
                DisplayAlert("Error", "Passwords dont match", "OK");
            }
        }
    }
}