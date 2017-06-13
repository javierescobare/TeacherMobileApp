using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TeacherMobileApp.Helpers;
using TeacherMobileApp.Models;
using TeacherMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContractTeacherPage : ContentPage
    {
        ContractTeacherViewModel viewModel;

        public ContractTeacherPage(Teacher teacher)
        {
            InitializeComponent();
            BindingContext = viewModel = new ContractTeacherViewModel(teacher);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.SchedulesPickerPage != null)
            {
                var answers = viewModel.SchedulesPickerPage.GetSelection();
                viewModel.Schedules.Clear();
                foreach (var schedule in answers)
                {
                    viewModel.Schedules.Add(schedule);
                }
            }

            if (viewModel.LocationPage != null && viewModel.LocationPage.MapImage != null)
            {
                LocationImage.Source = ImageSource.FromStream(() => new MemoryStream(viewModel.LocationPage.MapImage));
                LocationImage.IsVisible = true;
                await viewModel.GetAddressForActualPositionAsync();
            }
        }     

        private async void NextClick(object sender, EventArgs e)
        {
            if (viewModel.NoSchedules)
            {
                await DisplayAlert("Un momento", "Debes seleccionar por lo menos un horario para la atención.", "OK");
                return;
            }

            var accepted = await DisplayAlert("Atención", "¿Estás seguro de proceder con la solicitud?", "Sí", "Cancelar");
            if (!accepted)
                return;

            viewModel.Teacher.Unemployed = false;
            App.Classes.Add(new Class() { CourseName = BaseViewModel.Course.Name, Teacher = viewModel.Teacher, Schedules = viewModel.SchedulesOutput });

            viewModel.NavigateToPageCurrent(new ShellPage());
        }

    }
}