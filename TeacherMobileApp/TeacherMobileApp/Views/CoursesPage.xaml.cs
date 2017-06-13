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
    public partial class CoursesPage : ContentPage
    {
        CoursesViewModel viewModel;
        public CoursesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CoursesViewModel();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //Deselect Item
            ((ListView)sender).SelectedItem = null;

            BaseViewModel.Course = e.Item as Models.Course;
            await viewModel.MasterNavigateTo(new AvailableTeachersPage());
        }
    }
}