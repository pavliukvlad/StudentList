using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content.Res;
using StudentList.Constants;
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

        private readonly LoadingDelays delays;
        private readonly IStringProvider stringProvider;

        public StudentsRepository(LoadingDelays loadingDelays, IStringProvider stringProvider)
        {
            this.delays = loadingDelays;
            this.stringProvider = stringProvider;
        }

        public async Task<ValidationResult> AddNewStudentAsync(string name, Uri photoUri, DateTime birthdate, string group, string uni, string phone)
        {
            var validationResult = this.Validate(name, birthdate, group, uni, phone);

            if (validationResult.IsValid)
            {
                var student = new Student()
                {
                    ProfilePhoto = photoUri,
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Birthdate = birthdate,
                    GroupName = group,
                    University = uni,
                    Phone = phone
                };

                students.Add(student);
            }

            await Task.Delay(this.delays.AddStudentDelay);

            return validationResult;
        }

        public async Task<ValidationResult> ChangeStudentById(string studentId, Uri photoUri, string name, DateTime birthdate, string group, string uni, string phone)
        {
            var validationResult = this.Validate(name, birthdate, group, uni, phone);

            if (validationResult.IsValid)
            {
                var student = students.Where(s => s.Id == studentId).FirstOrDefault();

                if (student != null)
                {
                    student.ProfilePhoto = photoUri;
                    student.Name = name;
                    student.Birthdate = birthdate;
                    student.GroupName = group;
                    student.University = uni;
                    student.Phone = phone;
                }
            }

            await Task.Delay(this.delays.ChangeStudentDelay);

            return validationResult;
        }

        public async Task<Student> GetStudentById(string id)
        {
            await Task.Delay(this.delays.GetStudentDelay);

            return students.Where(s => s.Id == id).FirstOrDefault();
        }

        public async Task<IList<Student>> GetStudentsAsync(StudentFilter studentFilter)
        {
            IEnumerable<Student> temp = students;

            if (studentFilter != StudentFilter.Default)
            {
                if (!string.IsNullOrWhiteSpace(studentFilter.Name))
                {
                    temp = temp.Where(s => s.Name.ToUpperInvariant() == studentFilter.Name.ToUpperInvariant()
                    || s.Name.ToUpperInvariant().Contains(studentFilter.Name.ToUpperInvariant()));
                }

                if (!string.IsNullOrWhiteSpace(studentFilter.Group))
                {
                    temp = temp.Where(s => s.GroupName.ToUpperInvariant() == studentFilter.Group.ToUpperInvariant()
                    || studentFilter.Group == this.stringProvider.GroupFilter);
                }

                if (studentFilter.Birthdate != default(DateTime))
                {
                    temp = temp.Where(s => s.Birthdate == studentFilter.Birthdate);
                }
            }

            await Task.Delay(this.delays.GetStudentsDelay);

            return temp.ToList();
        }

        private ValidationResult Validate(string name, DateTime birthdate, string group, string uni, string phone)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrWhiteSpace(name))
            {
                validationResult.Errors.Add(nameof(name), new List<string>() { this.stringProvider.NameError });
            }

            if (birthdate == DateTime.MinValue)
            {
                validationResult.Errors.Add(nameof(birthdate), new List<string>() { this.stringProvider.BirthdateError });
            }

            if (string.IsNullOrWhiteSpace(group))
            {
                validationResult.Errors.Add(nameof(group), new List<string>() { this.stringProvider.GroupError });
            }

            if (string.IsNullOrWhiteSpace(uni))
            {
                validationResult.Errors.Add(nameof(uni), new List<string>() { this.stringProvider.UniversityError });
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!Regex.Match(phone, PatternConstants.PhoneNumber).Success)
                {
                    validationResult.Errors.Add(nameof(phone), new List<string> { this.stringProvider.PhoneError });
                }
            }

            return validationResult;
        }
    }
}
