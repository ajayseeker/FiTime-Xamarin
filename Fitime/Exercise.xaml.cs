using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fitime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Exercise : ContentPage
    {
        public Exercise()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        override protected void OnAppearing()
        {
            ExerciseViewModel obj = (ExerciseViewModel)this.BindingContext;
            TimeExecute(obj);
        }
        private async void TimeExecute(ExerciseViewModel obj)
        {
            await  obj.TimerExecuteAsync(Navigation);
        }
    }
}