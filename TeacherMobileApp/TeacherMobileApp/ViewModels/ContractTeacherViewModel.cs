using Acr.UserDialogs;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        public IGeolocator GeoLocator { get; set; }
        public ICommand GetLocation { get; set; }
        public ICommand PickSchedules { get; set; }
        public ICommand ProcessContract { get; set; }
        public MapPage LocationPage { get; set; }
        public SelectMultipleBasePage<Schedule> SchedulesPickerPage { get; private set; }
        public Plugin.Geolocator.Abstractions.Position LastPosition { get; set; }

        private string _output;
        public string SchedulesOutput
        {
            get { return GetSchedulesOutput(); }
            set { _output = value; RaisePropertyChanged(); }
        }

        private double _total;
        public double Total
        {
            get { return CalculateTotal(); }
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
            Schedules.CollectionChanged += Schedules_CollectionChanged;

            GeoLocator = locator;
            GetLocation = new Command(async () => await GetUserLocationAsync());
            PickSchedules = new Command(async () => await SelectSchedulesAsync());
            ProcessContract = new Command(async () => await ProcessContractAsync());
            Location = DefaultLocationMessage;
        }



        private void Schedules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(SchedulesOutput));
            RaisePropertyChanged(nameof(Total));
        }

        public async Task GetUserLocationAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            if (!GeoLocator.IsGeolocationEnabled)
            {
                await Application.Current.MainPage.DisplayAlert("Primero lo primero", "Debes activar tu ubicación GPS.", "OK");
                IsBusy = false;
                return;
            }

            try
            {
                if (LastPosition == null)
                {
                    UserDialogs.Instance.ShowLoading(title: "Obteniendo ubicación");

                    LastPosition = await GeoLocator.GetPositionAsync((int)TimeSpan.FromSeconds(8).TotalMilliseconds);
                    if (LastPosition == null)
                    {
                        UserDialogs.Instance.HideLoading();
                        await Application.Current.MainPage.DisplayAlert("¡Ups!", "No se pudo obtener la ubicación, inténtalo nuevamente.", "OK");
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
                var position = new Xamarin.Forms.Maps.Position(latitude, longitude);
                var addresses = await new Geocoder().GetAddressesForPositionAsync(position);
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

        public async Task SelectSchedulesAsync()
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

        private string GetSchedulesOutput()
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

        public async Task ProcessContractAsync()
        {
            if (NoSchedules)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Un momento", 
                    "Debes seleccionar por lo menos un horario para la atención.", 
                    "OK");
                return;
            }

            var accepted = await Application.Current.MainPage.DisplayAlert(
                    "Atención", 
                    "¿Estás seguro de proceder con la solicitud?", 
                    "Sí", "Cancelar");
            if (!accepted)
                return;

            Teacher.Unemployed = false;
            App.Classes.Add(new Class() { CourseName = Course.Name, Teacher = Teacher, Schedules = SchedulesOutput });

            NavigateToPageCurrent(new ShellPage());
        }

        public bool NoSchedules
        {
            get { return Schedules.Count == 0; }
        }
    }
}
