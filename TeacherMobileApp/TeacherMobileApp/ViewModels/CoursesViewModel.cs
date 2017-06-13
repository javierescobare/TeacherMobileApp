using System.Collections.ObjectModel;
using TeacherMobileApp.Models;

namespace TeacherMobileApp.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        public ObservableCollection<Course> KidCourses { get; set; }
        public ObservableCollection<Course> Courses { get; set; }

        public CoursesViewModel()
        {
            KidCourses = new ObservableCollection<Course>
            {
                new Course() { Name = "Matemáticas", IconPath = "" },
                new Course() { Name = "Comunicación", IconPath = "" },
                new Course() { Name = "Personal social", IconPath = "" },
                new Course() { Name = "Computación", IconPath = "" },
            };

            Courses = new ObservableCollection<Course>
            {
                new Course() { Name = "Cálculo", IconPath = "" },
                new Course() { Name = "Econometría", IconPath = "" },
                new Course() { Name = "Base de datos", IconPath = "" }
            };            
        }
    }
}
