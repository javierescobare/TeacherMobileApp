using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherMobileApp.Models;

namespace TeacherMobileApp.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public ObservableCollection<Class> Classes
        {
            get
            {
                return App.Classes;
            }
        }
    }
}
