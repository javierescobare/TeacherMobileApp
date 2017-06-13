using System.Collections.ObjectModel;
using TeacherMobileApp.Models;
using TeacherMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        ObservableCollection<Class> Classes;
        public WelcomePage()
        {
            InitializeComponent();
            BindingContext = this;
            Classes = new ObservableCollection<Class>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ClassesLvw.ItemsSource = App.Classes;
            ClassesLvw.IsVisible = (App.Classes.Count > 0);
            NoClassesLabel.IsVisible = !ClassesLvw.IsVisible;
        }

        private async void ClassesLvw_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //Deselect Item
            ((ListView)sender).SelectedItem = null;

            var selectedClass = e.Item as Class;
            await BaseViewModel.MasterDetail.Detail.Navigation.PushAsync(new TeacherProfilePage(selectedClass.Teacher));
        }
    }
}