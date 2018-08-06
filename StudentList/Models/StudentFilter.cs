using System;

namespace StudentList.Models
{
    public class StudentFilter
    {
        public StudentFilter(string name, string group, DateTime birthdate)
        {
            this.Name = name;
            this.Group = group;
            this.Birthdate = birthdate;
        }

        public static StudentFilter Default => new StudentFilter(
            null, null, DateTime.MinValue);

        public string Name { get; }

        public string Group { get; }

        public DateTime Birthdate { get; }

        public bool IsDefault()
        {
            return this.Name == Default.Name
                && this.Birthdate == Default.Birthdate
                && this.Group == Default.Group;
        }

        public StudentFilter Clone()
        {
            return (StudentFilter)this.MemberwiseClone();
        }
    }
}
