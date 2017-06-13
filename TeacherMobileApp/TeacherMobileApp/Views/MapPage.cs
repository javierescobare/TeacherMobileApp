using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeacherMobileApp.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Acr.UserDialogs;

namespace TeacherMobileApp.Views
{
    public class MapPage : ContentPage
    {
        public byte[] MapImage { get; private set; }
        public Pin MyPositionPin { get; private set; }
        private ExtendedMap _map;
        private Button _acceptBtn;

        public MapPage(double latitude, double longitude)
        {
            Title = "Escoge una ubicación";

            var actualPosition = new Position(latitude, longitude);
            var mapRegion = MapSpan.FromCenterAndRadius(actualPosition, Distance.FromMiles(0.3));
            _map = new ExtendedMap(mapRegion)
            {
                MapType = MapType.Street
            };
            _map.Tap += Map_Tap;

            BuildPin(actualPosition);
            _map.Pins.Add(MyPositionPin);

            _acceptBtn = new Button()
            {
                Text = "Confirmar",
                BackgroundColor = (Color)Application.Current.Resources["colorPrimaryDark"],
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.End
            };
            _acceptBtn.Clicked += Pin_Clicked;

            Content = new Grid
            {
                Children = {
                    _map,
                    _acceptBtn
                }
            };
        }

        private async void Pin_Clicked(object sender, EventArgs e)
        {
            await ConfirmLocation();
        }

        private async System.Threading.Tasks.Task ConfirmLocation()
        {
            var shouldSelect = await DisplayAlert("Seleccionar ubicación", "Estás seguro de seleccionar esta ubicación?", "Sí", "No");
            if (!shouldSelect)
                return;

            UserDialogs.Instance.ShowSuccess("Ubicación elegida correctamente");

            MapImage = await _map.Snapshot.Invoke();
            SendBackButtonPressed();
        }

        private void Map_Tap(object sender, TapEventArgs e)
        {
            if(_map.Pins.Count == 0)
            {
                BuildPin(e.Position);
                _map.Pins.Add(MyPositionPin);
                return;
            }

            MovePin(e.Position);
        }

        private void BuildPin(Position position)
        {
            NewPin(position);
        }


        private void MovePin(Position position)
        {
            NewPin(position);

            Device.BeginInvokeOnMainThread(() =>
            {
                _map.Pins.Clear();
                _map.Pins.Add(MyPositionPin);
            });

        }
        private void NewPin(Position position)
        {
            MyPositionPin = new Pin()
            {
                Address = $"{position.Latitude}, {position.Longitude}",
                Label = "Punto de reunión para la clase",
                Position = position,
                Type = PinType.Generic
            };
            MyPositionPin.Clicked += Pin_Clicked;
        }

    }
}