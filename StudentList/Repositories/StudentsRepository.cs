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
    class StudentsRepository : IStudentRepository
    {
        private static IList<Student> students = new List<Student>() {
            new Student() { Id = Guid.NewGuid().ToString(), Birthdate = new DateTime(1999, 08, 01), Name = "Vlad", GroupName = "MN", University = "Lviv Polytechnic" },
            new Student() { Id = Guid.NewGuid().ToString(), Birthdate = new DateTime(1998, 01, 19), Name = "Sasha", GroupName = "MN", University = "Lviv Polytechnic" },
            new Student() { Id = Guid.NewGuid().ToString(), Birthdate = new DateTime(1998, 12, 25), Name = "Dima", GroupName = "MN", University = "Lviv Polytechnic" },
            new Student() { Id = Guid.NewGuid().ToString(), Birthdate = new DateTime(1998, 11, 17), Name = "Taras", GroupName = "MN", University = "Lviv Polytechnic" }
        };

        public IList<Student> Students => students;

        public int Count => students.Count;

        public Student this[string index]
        {
            get
            {
                return students.Where(s=>s.Id==index).FirstOrDefault();
            }
            set
            {
                var student = students.Where(s=>s.Id==index).FirstOrDefault();
                student = value;
            }
        }

        public void AddNewStudent(Student student)
        {
            students.Add(student);
        }

        public void ChangeStudentById(string studentId, string name, DateTime birthdate, string group, string uni)
        {
            var student = students.Where(s => s.Id == studentId).FirstOrDefault();
            student.Name = name;
            student.Birthdate = birthdate;
            student.GroupName = group;
            student.University = uni;
        }

        public async Task<IList<Student>> GetStudentsAsync()
        {
            await Task.Delay(5000);
            return students;
        }

        public async Task<IList<Student>> GetStudentsAsync(string name, string group, DateTime birthdate)
        {
            IEnumerable<Student> temp = students;

            if (!String.IsNullOrWhiteSpace(name))
                temp = temp.Where(s => s.Name.ToLower() == name.ToLower());
            if (!String.IsNullOrWhiteSpace(group))
                temp = temp.Where(s => s.GroupName.ToLower() == group.ToLower());
            if(birthdate!=default(DateTime))
                temp = temp.Where(s => s.Birthdate == birthdate);
            await Task.Delay(1000);
            return temp.ToList();
        }
    }
}