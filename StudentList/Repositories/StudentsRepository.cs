using System;
using System.Collections;
using System.Collections.Generic;
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

        public async Task AddNewStudent(Student student)
        {
            students.Add(student);
        }

        public async Task ChangeStudentById(string studentId, string name, DateTime birthdate, string group, string uni, string phone)
        {
            var student = students.Where(s => s.Id == studentId).FirstOrDefault();

            if (student is null)
            {
                return;
            }

            student.Name = name;
            student.Birthdate = birthdate;
            student.GroupName = group;
            student.University = uni;
            student.Phone = phone;
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
