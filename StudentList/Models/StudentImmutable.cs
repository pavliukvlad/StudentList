using System;

namespace StudentList.Models
{
    public class StudentImmutable
    {
        public StudentImmutable()
            : this(
                  string.Empty,
                  DateTime.MinValue,
                  string.Empty,
                  string.Empty,
                  string.Empty,
                  string.Empty,
                  null)
        {
        }

        public StudentImmutable(
            string id,
            DateTime birthdate,
            string name,
            string group,
            string university,
            string phone,
            Uri profilePhoto)
        {
            this.Id = id;
            this.Birthdate = birthdate;
            this.Name = name;
            this.GroupName = group;
            this.University = university;
            this.Phone = phone;
            this.ProfilePhoto = profilePhoto;
        }

        public string Id { get; }

        public DateTime Birthdate { get; }

        public string Name { get; }

        public string GroupName { get; }

        public string University { get; }

        public string Phone { get; }

        public Uri ProfilePhoto { get; }

        public Student ToStudent()
        {
            return new Student
            {
                Id = this.Id,
                Birthdate = this.Birthdate,
                Name = this.Name,
                GroupName = this.GroupName,
                University = this.University,
                Phone = this.Phone,
                ProfilePhoto = this.ProfilePhoto
            };
        }
    }
}
