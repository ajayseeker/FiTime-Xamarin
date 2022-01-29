using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fitime
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }
    }

}
