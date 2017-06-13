using Acr.UserDialogs;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeacherMobileApp.Helpers;
using TeacherMobileApp.Models;
using TeacherMobileApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TeacherMobileApp.ViewModels
{
    public class ContractTeacherViewModel : BaseViewModel
    {
        private const string DefaultLocationMessage = "Ubicación no establecida";

        public string CourseName { get; set; }
        public Teacher Teacher { get; set; }
        public ObservableCollection<Schedule> Schedules { get; set; }
        public IGeolocator Locator { get; set; }
        public ICommand GetLocation { get; set; }
        public ICommand PickSchedules { get; set; }
        public MapPage LocationPage { get; set; }
        public SelectMultipleBasePage<Schedule> SchedulesPickerPage { get; private set; }
        public Plugin.Geolocator.Abstractions.Position LastPosition { get; set; }

        private string _output;
        public string SchedulesOutput
        {
            get { return _output; }
            set { _output = value; RaisePropertyChanged(); }
        }

        private double _total;
        public double Total
        {
            get { return _total; }
            set { _total = value; RaisePropertyChanged(); }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set { _location = value; RaisePropertyChanged(); }
        }



        public ContractTeacherViewModel(Teacher teacher) : this(teacher, CrossGeolocator.Current) { }
        public ContractTeacherViewModel(Teacher teacher, IGeolocator locator)
        {
            this.Teacher = teacher;

            Schedules = new ObservableCollection<Schedule>();
            SchedulesOutput = ReturnOutput();
            Schedules.CollectionChanged += Schedules_CollectionChanged;

            Locator = locator;
            GetLocation = new Command(async () => await GetUserLocation());
            PickSchedules = new Command(async () => await SelectSchedules());
            Location = DefaultLocationMessage;
        }



        private void Schedules_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SchedulesOutput = ReturnOutput();
            Total = CalculateTotal();
        }

        private async Task GetUserLocation()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            if (!Locator.IsGeolocationEnabled)
            {
                await App.Current.MainPage.DisplayAlert("Primero lo primero", "Debes activar tu ubicación GPS.", "OK");
                IsBusy = false;
                return;
            }

            try
            {
                if (LastPosition == null)
                {
                    UserDialogs.Instance.ShowLoading(title: "Obteniendo ubicación");

                    LastPosition = await Locator.GetPositionAsync((int)TimeSpan.FromSeconds(8).TotalMilliseconds);
                    if (LastPosition == null)
                    {
                        UserDialogs.Instance.HideLoading();
                        await App.Current.MainPage.DisplayAlert("¡Ups!", "No se pudo obtener la ubicación, inténtalo nuevamente.", "OK");
                        Location = DefaultLocationMessage;
                        return;
                    }
                }

                LocationPage = new MapPage(LastPosition.Latitude, LastPosition.Longitude);
                await NavigateTo(LocationPage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Location = "Hubo un error, intenta otra vez.";
            }
            finally
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }
        }

        public async Task GetAddressForActualPositionAsync()
        {
            var selectedPosition = LocationPage.MyPositionPin.Position;
            await GetAddressForPositionAsync(selectedPosition.Latitude, selectedPosition.Longitude);
        }

        public async Task GetAddressForPositionAsync(double latitude, double longitude)
        {
            try
            {
                Location = "Ubicación establecida";
                var addresses = await new Geocoder().GetAddressesForPositionAsync(new Xamarin.Forms.Maps.Position(latitude, longitude));
                var address = addresses.FirstOrDefault();
                if (address == null)
                    return;
                Location = address;
            }
            catch
            {
                Location = "Ubicación establecida, dirección no disponible";
            }
        }

        public async Task SelectSchedules()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var items = new List<Schedule>
            {
                new Schedule { Fee = 20, Name = "Lunes 4:00 pm - 5:00 pm" },
                new Schedule { Fee = 20, Name = "Lunes 5:30 pm - 6:30 pm" },
                new Schedule { Fee = 20, Name = "Viernes 2:00 pm - 3:00 pm" },
                new Schedule { Fee = 20, Name = "Viernes 3:00 pm - 4:00 pm" }
            };
            if (SchedulesPickerPage == null)
                SchedulesPickerPage = new SelectMultipleBasePage<Schedule>(items) { Title = "Horarios a escoger" };

            await MasterNavigateTo(SchedulesPickerPage);

            IsBusy = false;
        }

        private double CalculateTotal()
        {
            double total = 0;
            foreach (var schedule in Schedules)
            {
                total += schedule.Fee;
            }
            return total;
        }

        private string ReturnOutput()
        {
            if (NoSchedules)
                return "No has escogido ningún horario";

            var builder = new StringBuilder();
            foreach (var schedule in Schedules)
            {
                builder.Append(schedule.Name);
                builder.Append(", ");
            }
            return builder.ToString(0, builder.Length -2);
        }

        public bool NoSchedules
        {
            get { return Schedules.Count == 0; }
        }
    }
}
