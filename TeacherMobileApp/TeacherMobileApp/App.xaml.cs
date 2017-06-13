using System.Collections.ObjectModel;
using TeacherMobileApp.Helpers;
using TeacherMobileApp.Models;
using Xamarin.Forms;

namespace TeacherMobileApp
{
    public partial class App : Application
    {
        public static ObservableCollection<Class> Classes { get; set; }

        public App()
        {
            InitializeComponent();
            
            if (Settings.LoggedIn)
                MainPage = new Views.ShellPage();
            else
                MainPage = new Views.LoginPage();

            Classes = new ObservableCollection<Class>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
