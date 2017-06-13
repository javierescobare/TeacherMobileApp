using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace TeacherMobileApp.Droid
{
    [Activity(Label = "Yo Profe", NoHistory = true, Theme = "@style/SplashTheme", MainLauncher = true)]
    public class SplashScreenActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
        }
    }
}