using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TeacherMobileApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public static Models.Course Course;
        public static INavigation Navigation { get; set; }
        public static MasterDetailPage MasterDetail { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged("IsBusy"); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged("Title"); }
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task NavigateTo(Page pageView)
        {
            await Navigation.PushModalAsync(pageView);
        }

        public async Task MasterNavigateTo(Page pageView)
        {
            MasterDetail.IsPresented = false;
            await MasterDetail.Detail.Navigation.PushAsync(pageView);
        }

        public void NavigateToPageCurrent(Page pageView)
        {
            Application.Current.MainPage = pageView;
        }

        public void NavigateGoBack()
        {
            Navigation.PopAsync();
        }
    }
}
