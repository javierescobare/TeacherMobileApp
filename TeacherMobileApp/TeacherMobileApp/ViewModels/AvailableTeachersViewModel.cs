using System.Collections.ObjectModel;
using TeacherMobileApp.Models;

namespace TeacherMobileApp.ViewModels
{
    public class AvailableTeachersViewModel : BaseViewModel
    {
        public ObservableCollection<Teacher> Teachers { get; set; }

        public AvailableTeachersViewModel()
        {
            Teachers = new ObservableCollection<Teacher>
            {
                new Teacher() { Name = "Julio", SurName = "Moreno García", Description = "Bachiller de la Universidad Nacional de Trujillo, docente con 22 años de experiencia en educación secundaria." },
                new Teacher() { Name = "Hector", SurName = "Luyo Chumpitaz", Description = "Ingeniero egresado de la Universidad Nacional de Trujillo y docente con mucha pasión por el aprendizaje. "},
                new Teacher() { Name = "Henry", SurName = "Mendoza Puertas", Description = "Magíster egresado de la Universidad Privada Antenor Orrego de Trujillo, docente en UPAO y UPC - Monterrico" }
            };
        }
    }
}
