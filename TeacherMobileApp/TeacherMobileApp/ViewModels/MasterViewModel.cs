using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TeacherMobileApp.Helpers;
using TeacherMobileApp.Models;
using TeacherMobileApp.Views;
using Xamarin.Forms;

namespace TeacherMobileApp.ViewModels
{
    public class MasterViewModel : BaseViewModel
    {
        public ObservableCollection<MasterPageMenuItem> MenuItems { get; set; }
        public ICommand SignOut { get; set; }

        public MasterViewModel()
        {
            MenuItems = new ObservableCollection<MasterPageMenuItem>(new[]
            {
                new MasterPageMenuItem { Id = 0, Title = "Inicio", TargetType = typeof(WelcomePage), IconPath = "ic_view_dashboard.png" },
                new MasterPageMenuItem { Id = 1, Title = "Primaria", TargetType = typeof(CoursesKidPage), IconPath = "ic_face.png" },
                new MasterPageMenuItem { Id = 2, Title = "Secundaria", TargetType = typeof(CoursesPage), IconPath = "ic_book_open_page_variant.png" },
                new MasterPageMenuItem { Id = 3, Title = "Pregrado", TargetType = typeof(CoursesPage), IconPath = "ic_school.png" }
            });

            SignOut = new Command(async () => await DeleteSession());
        }

        private async Task DeleteSession()
        {
            MasterDetail.IsPresented = false;

            var shouldClose = await Application.Current.MainPage.DisplayAlert(
                "Cerrar sesión",
                "¿Estás seguro que deseas cerrar sesión?",
                "OK", "Cancelar");

            if (!shouldClose)
                return;

            Settings.LoggedIn = false;
            NavigateToPageCurrent(new LoginPage());
        }
    }
}