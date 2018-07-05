using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StudentList.Model;
using StudentList.Models;
using StudentList.Providers.Interfaces;

namespace StudentList
{
    public class StudentsRepository : IStudentRepository
    {
        private static IList<Student> students = new List<Student>()
        {
            new Student() { Id = Guid.NewGuid().ToString(), Birthdate = new DateTime(1999, 08, 01), Name = "Vlad", GroupName = "MN", University = "Lviv Polytechnic", Phone = "+380995323774" },
            new Student() { Id = Guid.NewGuid().ToString(), Birthdate = new DateTime(1998, 01, 19), Name = "Sasha", GroupName = "MN", University = "Lviv Polytechnic", Phone = null },
            new Student() { Id = Guid.NewGuid().ToString(), Birthdate = new DateTime(1998, 12, 25), Name = "Dima", GroupName = "MN", University = "Lviv Polytechnic", Phone = null },
            new Student() { Id = Guid.NewGuid().ToString(), Birthdate = new DateTime(1998, 11, 17), Name = "Taras", GroupName = "MN", University = "Lviv Polytechnic", Phone = "+380987573264" }
        };

        public async Task<ValidationResult> AddNewStudentAsync(string name, Uri profilePhotoUri, string birthdate, string group, string uni, string phone)
        {
            var validationResult = this.Validate(name, birthdate, group, uni, phone);

            if (validationResult.IsValid)
            {
                var student = new Student()
                {
                    ProfilePhoto = profilePhotoUri,
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Birthdate = Convert.ToDateTime(birthdate, CultureInfo.InvariantCulture),
                    GroupName = group,
                    University = uni,
                    Phone = phone
                };

                students.Add(student);
            }

            await Task.Delay(300);

            return validationResult;
        }

        public async Task<ValidationResult> ChangeStudentById(string studentId, Uri profilePhotoUri, string name, string birthdate, string group, string uni, string phone)
        {
            var validationResult = this.Validate(name, birthdate, group, uni, phone);

            if (validationResult.IsValid)
            {
                var student = students.Where(s => s.Id == studentId).FirstOrDefault();

                if (student != null)
                {
                    student.ProfilePhoto = profilePhotoUri;
                    student.Name = name;
                    student.Birthdate = DateTime.ParseExact(
                        birthdate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    student.GroupName = group;
                    student.University = uni;
                    student.Phone = phone;
                }
            }

            await Task.Delay(300);

            return validationResult;
        }

        public async Task<Student> GetStudentById(string id)
        {
            await Task.Delay(300);

            return students.Where(s => s.Id == id).FirstOrDefault();
        }

        public async Task<IList<Student>> GetStudentsAsync(StudentFilter studentFilter)
        {
            IEnumerable<Student> temp = students;

            if (studentFilter != default(StudentFilter))
            {
                if (!string.IsNullOrWhiteSpace(studentFilter.Name))
                {
                    temp = temp.Where(s => s.Name.ToUpperInvariant() == studentFilter.Name.ToUpperInvariant());
                }

                if (!string.IsNullOrWhiteSpace(studentFilter.Group))
                {
                    temp = temp.Where(s => s.GroupName.ToUpperInvariant() == studentFilter.Group.ToUpperInvariant() | studentFilter.Group == "Group Any");
                }

                if (studentFilter.Birthdate != default(DateTime))
                {
                    temp = temp.Where(s => s.Birthdate == studentFilter.Birthdate);
                }
            }

            await Task.Delay(1000);

            return temp.ToList();
        }

        private ValidationResult Validate(string name, string birthdate, string group, string uni, string phone)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrWhiteSpace(name))
            {
                validationResult.Errors.Add(nameof(name), new List<string>() { " Name cannot be empty" });
            }

            if (string.IsNullOrWhiteSpace(birthdate))
            {
                validationResult.Errors.Add(nameof(birthdate), new List<string>() { " Birthdate cannot be empty" });
            }

            if (string.IsNullOrWhiteSpace(group))
            {
                validationResult.Errors.Add(nameof(group), new List<string>() { " Group name cannot be empty" });
            }

            if (string.IsNullOrWhiteSpace(uni))
            {
                validationResult.Errors.Add(nameof(uni), new List<string>() { " University name cannot be empty" });
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!Regex.Match(phone, @"^\+380\d{9}").Success)
                {
                    validationResult.Errors.Add(nameof(phone), new List<string> { " Wrong phone number format"});
                }
            }

            return validationResult;
        }
    }
}
