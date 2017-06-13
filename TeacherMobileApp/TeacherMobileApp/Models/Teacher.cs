namespace TeacherMobileApp.Models
{
    public class Teacher : Person
    {
        public string Description { get; set; }
        public double Rating { get; set; }
        public string Schedule { get; set; }
        public string Payment { get; set; }
        public string Phone { get; set; }
        public bool Unemployed { get; set; } = true;
    }
}
