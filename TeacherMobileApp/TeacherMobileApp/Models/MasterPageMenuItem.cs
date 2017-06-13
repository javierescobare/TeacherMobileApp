using System;

namespace TeacherMobileApp.Models
{

    public class MasterPageMenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Type TargetType { get; set; }
        public string IconPath { get; set; }
    }
}