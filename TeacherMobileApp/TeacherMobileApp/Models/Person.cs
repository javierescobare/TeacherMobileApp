namespace TeacherMobileApp.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string FullName
        {
            get
            {
                return $"{SurName}, {Name}";
            }
        }
    }
}
