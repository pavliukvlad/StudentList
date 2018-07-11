using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
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

        private Activity activity;
        private LoadingDelays delays;

        public StudentsRepository(Activity activity, LoadingDelays loadingDelays)
        {
            this.activity = activity;
            this.delays = loadingDelays;
        }

        public async Task<ValidationResult> AddNewStudentAsync(string name, SavingPhotoResult photoResult, string birthdate, string group, string uni, string phone)
        {
            var validationResult = this.Validate(photoResult, name, birthdate, group, uni, phone);

            if (validationResult.IsValid)
            {
                var student = new Student()
                {
                    ProfilePhoto = photoResult.ProfilePhotoUri,
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Birthdate = DateTime.ParseExact(
                        birthdate, FormatConstants.DateTimeFormat, CultureInfo.InvariantCulture),
                    GroupName = group,
                    University = uni,
                    Phone = phone
                };

                students.Add(student);
            }

            await Task.Delay(this.delays.AddStudentDelay);

            return validationResult;
        }

        public async Task<ValidationResult> ChangeStudentById(string studentId, SavingPhotoResult photoResult, string name, string birthdate, string group, string uni, string phone)
        {
            var validationResult = this.Validate(photoResult, name, birthdate, group, uni, phone);

            if (validationResult.IsValid)
            {
                var student = students.Where(s => s.Id == studentId).FirstOrDefault();

                if (student != null)
                {
                    student.ProfilePhoto = photoResult.ProfilePhotoUri;
                    student.Name = name;
                    student.Birthdate = DateTime.ParseExact(
                        birthdate, FormatConstants.DateTimeFormat, CultureInfo.InvariantCulture);
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

            await Task.Delay(this.delays.GetStudentsDelay);

            return temp.ToList();
        }

        private ValidationResult Validate(SavingPhotoResult photo, string name, string birthdate, string group, string uni, string phone)
        {
            var validationResult = new ValidationResult();

            if (photo.IsError)
            {
                validationResult.Errors.Add(nameof(photo), new List<string> { this.activity.GetString(Resource.String.photo_toast_error) });
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                validationResult.Errors.Add(nameof(name), new List<string>() { this.activity.GetString(Resource.String.name_field_error) });
            }

            if (string.IsNullOrWhiteSpace(birthdate))
            {
                validationResult.Errors.Add(nameof(birthdate), new List<string>() { this.activity.GetString(Resource.String.birthdate_field_error) });
            }

            if (string.IsNullOrWhiteSpace(group))
            {
                validationResult.Errors.Add(nameof(group), new List<string>() { this.activity.GetString(Resource.String.group_field_error) });
            }

            if (string.IsNullOrWhiteSpace(uni))
            {
                validationResult.Errors.Add(nameof(uni), new List<string>() { this.activity.GetString(Resource.String.uni_field_error) });
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!Regex.Match(phone, PatternConstants.PhoneNumber).Success)
                {
                    validationResult.Errors.Add(nameof(phone), new List<string> { this.activity.GetString(Resource.String.phone_field_error) });
                }
            }

            return validationResult;
        }
    }
}
