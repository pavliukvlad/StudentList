using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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

        public async Task<ValidationResult> AddNewStudentAsync(string name, string birthdate, string group, string uni)
        {
            var validationResult = new ValidationResult();

            this.Validate(name, birthdate, group, uni, validationResult);

            if (validationResult.IsValid)
            {
                var student = new Student()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Birthdate = Convert.ToDateTime(birthdate, CultureInfo.InvariantCulture),
                    GroupName = group,
                    University = uni
                };

                students.Add(student);
            }

            return validationResult;
        }

        private void Validate(string name, string birthdate, string group, string uni, ValidationResult validationResult)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                validationResult.Errors.Add(nameof(name), new List<string>() { "Empty field" });
            }

            if (string.IsNullOrWhiteSpace(birthdate))
            {
                validationResult.Errors.Add(nameof(birthdate), new List<string>() { "Empty field" });
            }

            if (string.IsNullOrWhiteSpace(group))
            {
                validationResult.Errors.Add(nameof(group), new List<string>() { "Empty field" });
            }

            if (string.IsNullOrWhiteSpace(uni))
            {
                validationResult.Errors.Add(nameof(uni), new List<string>() { "Empty field" });
            }
        }

        public async Task<ValidationResult> ChangeStudentById(string studentId, string name, string birthdate, string group, string uni, string phone)
        {
            var validationResult = new ValidationResult();
            this.Validate(name, birthdate, group, uni, validationResult);

            if (validationResult.IsValid)
            {
                var student = students.Where(s => s.Id == studentId).FirstOrDefault();

                if (student != null)
                {
                    student.Name = name;
                    student.Birthdate = Convert.ToDateTime(birthdate, CultureInfo.InvariantCulture);
                    student.GroupName = group;
                    student.University = uni;
                    student.Phone = phone;
                }
            }

            return validationResult;
        }

        public async Task<Student> GetStudentById(string id)
        {
            return students.Where(s => s.Id == id).FirstOrDefault();
        }

        public async Task<IList<Student>> GetStudentsAsync(StudentFilter studentFilter)
        {
            IEnumerable<Student> temp = students;

            if (studentFilter != null)
            {
                if (!string.IsNullOrWhiteSpace(studentFilter.Name))
                {
                    temp = temp.Where(s => s.Name.ToLower() == studentFilter.Name.ToLower());
                }

                if (!string.IsNullOrWhiteSpace(studentFilter.Group))
                {
                    temp = temp.Where(s => s.GroupName.ToLower() == studentFilter.Group.ToLower() | studentFilter.Group == "Any");
                }

                if (studentFilter.Birthdate != default(DateTime))
                {
                    temp = temp.Where(s => s.Birthdate == studentFilter.Birthdate);
                }
            }

            await Task.Delay(1000);

            return temp.ToList();
        }
    }
}
