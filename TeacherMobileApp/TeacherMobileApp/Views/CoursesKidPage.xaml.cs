using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursesKidPage : ContentPage
    {
        CoursesViewModel viewModel;
        public CoursesKidPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CoursesViewModel();
        }

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
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