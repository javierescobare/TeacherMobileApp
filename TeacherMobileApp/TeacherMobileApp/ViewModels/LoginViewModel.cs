using Acr.UserDialogs;
using System.Threading.Tasks;
using System.Windows.Input;
using TeacherMobileApp.Helpers;
using TeacherMobileApp.Models;
using TeacherMobileApp.Views;
using Xamarin.Forms;

namespace TeacherMobileApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private User _user;
        public User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public ICommand LoginCommand { get; set; }


        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login());
            User = new User();
        }


        private async Task Login()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                string email = _user.Email;
                string password = _user.Password;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "¡Ups! Se te olvidó algo",
                        "Al parecer has dejado algún campo incompleto, por favor, verifica los datos que ingresaste.",
                        "OK");
                    return;
                }

                using (UserDialogs.Instance.Loading("Verificando credenciales"))
                {
                    await Task.Delay(3000);
                }
                Settings.LoggedIn = true;
                NavigateToPageCurrent(new ShellPage());
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
