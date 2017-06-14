using Android.Gms.Maps;
using TeacherMobileApp.Controls;
using TeacherMobileApp.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ExtendedMap), typeof(ExtendedMapRenderer))]
namespace TeacherMobileApp.Droid.Controls
{
    public class ExtendedMapRenderer : MapRenderer, GoogleMap.ISnapshotReadyCallback
    {
        private byte[] _snapShot;
        private bool _mapReady;
        private bool _isDrawn;


        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            if (NativeMap != null)
                NativeMap.MapClick -= GoogleMap_MapClick;

            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("VisibleRegion") && !_isDrawn)
            {
                _isDrawn = true;
                OnGoogleMapReady();
            }
        }

        private void OnGoogleMapReady()
        {
            if (_mapReady) return;

            NativeMap.MapClick += GoogleMap_MapClick;
            ((ExtendedMap)Element).Snapshot = GetSnapshot;

            _mapReady = true;
        }

        private void GoogleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            ((ExtendedMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
        }

        public async Task<byte[]> GetSnapshot()
        {
            if (NativeMap == null) return null;

            _snapShot = null;
            NativeMap.Snapshot(this);

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
    }
}