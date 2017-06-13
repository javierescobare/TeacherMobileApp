using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using TeacherMobileApp.Controls;
using System.Net;
using TeacherMobileApp.Droid.Controls;

[assembly: ExportRenderer(typeof(PDFViewer), typeof(PDFViewerRenderer))]
namespace TeacherMobileApp.Droid.Controls
{
    public class PDFViewerRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var pdfWebView = Element as PDFViewer;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                Control.LoadUrl(
                    string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", 
                        string.Format("file:///android_asset/Content/{0}", 
                            WebUtility.UrlEncode(pdfWebView.Uri))));

            }
        }
    }
}