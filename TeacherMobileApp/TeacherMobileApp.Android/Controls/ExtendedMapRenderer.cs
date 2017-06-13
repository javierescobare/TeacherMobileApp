using Android.Gms.Maps;
using System;
using TeacherMobileApp.Controls;
using TeacherMobileApp.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using System.Threading.Tasks;
using System.IO;

[assembly: ExportRenderer(typeof(ExtendedMap), typeof(ExtendedMapRenderer))]
namespace TeacherMobileApp.Droid.Controls
{
    public class ExtendedMapRenderer : MapRenderer, IOnMapReadyCallback, GoogleMap.ISnapshotReadyCallback
    {
        private GoogleMap _map;
        private byte[] _snapShot;

        public void OnMapReady(GoogleMap googleMap)
        {
            InvokeOnMapReadyBaseClassHack(googleMap);
            _map = googleMap;
            if (_map != null)
                _map.MapClick += GoogleMap_MapClick;
            ((ExtendedMap)Element).Snapshot = GetSnapshot;
        }

        public async Task<byte[]> GetSnapshot()
        {
            if (_map == null) return null;

            _snapShot = null;
            _map.Snapshot(this);

            while (_snapShot == null) await Task.Delay(10);

            return _snapShot;
        }

        public void OnSnapshotReady(Bitmap snapshot)
        {
            using (var strm = new MemoryStream())
            {
                snapshot.Compress(Bitmap.CompressFormat.Png, 100, strm);
                this._snapShot = strm.ToArray();
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            if (_map != null)
                _map.MapClick -= GoogleMap_MapClick;

            base.OnElementChanged(e);

            Control?.GetMapAsync(this);
        }

        private void GoogleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            ((ExtendedMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
        }


        //  Hack to make IOnMapReadyCallback work correctly after updating Maps nuget
        private void InvokeOnMapReadyBaseClassHack(GoogleMap googleMap)
        {
            System.Reflection.MethodInfo onMapReadyMethodInfo = null;

            Type baseType = typeof(MapRenderer);
            foreach (var currentMethod in baseType.GetMethods(System.Reflection.BindingFlags.NonPublic |
                                                             System.Reflection.BindingFlags.Instance |
                                                              System.Reflection.BindingFlags.DeclaredOnly))
            {

                if (currentMethod.IsFinal && currentMethod.IsPrivate)
                {
                    if (string.Equals(currentMethod.Name, "OnMapReady", StringComparison.Ordinal))
                    {
                        onMapReadyMethodInfo = currentMethod;

                        break;
                    }

                    if (currentMethod.Name.EndsWith(".OnMapReady", StringComparison.Ordinal))
                    {
                        onMapReadyMethodInfo = currentMethod;

                        break;
                    }
                }
            }

            if (onMapReadyMethodInfo != null)
            {
                onMapReadyMethodInfo.Invoke(this, new[] { googleMap });
            }
        }
    }
}