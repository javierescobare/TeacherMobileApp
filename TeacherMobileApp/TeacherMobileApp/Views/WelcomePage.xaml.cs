using TeacherMobileApp.Models;
using TeacherMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        WelcomeViewModel viewModel;
        public WelcomePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new WelcomeViewModel();            
        }

        private async void ClassesLvw_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //Deselect Item
            ((ListView)sender).SelectedItem = null;

            var selectedClass = e.Item as Class;
            await viewModel.MasterNavigateTo(new TeacherProfilePage(selectedClass.Teacher));
        }
    }
}