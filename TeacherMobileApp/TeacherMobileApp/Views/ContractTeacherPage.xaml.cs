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

    }
}