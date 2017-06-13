using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TeacherMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AvailableTeachersPage : ContentPage
    {
        AvailableTeachersViewModel viewModel;
        public AvailableTeachersPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AvailableTeachersViewModel();
            Title = "Profesores";
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //Deselect Item
            ((ListView)sender).SelectedItem = null;

            var teacher = e.Item as Models.Teacher;
            await viewModel.MasterNavigateTo(new TeacherProfilePage(teacher));
        }
    }
}