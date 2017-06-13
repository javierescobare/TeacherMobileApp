using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherMobileApp.Helpers;
using TeacherMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeacherProfilePage : ContentPage
    {
        Models.Teacher _teacher;
        public TeacherProfilePage(Models.Teacher teacher)
        {
            InitializeComponent();
            BindingContext = _teacher = teacher;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {           
            await BaseViewModel.MasterDetail.Detail.Navigation.PushAsync(new ContractTeacherPage(_teacher));
        }

        private async void DownloadPdf_Clicked(object sender, EventArgs e)
        {
            await BaseViewModel.MasterDetail.Detail.Navigation.PushAsync(new PDFPage());
        }

        private async void CallButton_Clicked(object sender, EventArgs e)
        {
            var shouldCall = await DisplayAlert("Llamar a docente", $"¿Está seguro que desea llamar al profesor {_teacher.SurName}?", "Sí", "Cancelar");
            if (!shouldCall)
                return;

            Device.OpenUri(new Uri("tel:044420193"));
        }
    }
}