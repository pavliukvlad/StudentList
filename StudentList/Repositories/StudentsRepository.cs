using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            new Student() { Birthdate = new DateTime(1999, 08, 01), Name = "Vlad", GroupName = "MN", University = "Lviv Polytechnic" },
            new Student() { Birthdate = new DateTime(1998, 01, 19), Name = "Sasha", GroupName = "MN", University = "Lviv Polytechnic" },
            new Student() { Birthdate = new DateTime(1998, 12, 25), Name = "Dima", GroupName = "MN", University = "Lviv Polytechnic" },
            new Student() { Birthdate = new DateTime(1998, 11, 17), Name = "Taras", GroupName = "MN", University = "Lviv Polytechnic" }
        };

        public IList<Student> Students => students;

        public int Count => students.Count;

        public Student this[int index]
        {
            get
            {
                return students[index];
            }
            set
            {
                students[index] = value;
            }
        }

        public IList<Student> GetFilteringStudents(string name, string group, DateTime birthdate)
        {
            IEnumerable<Student> temp = students;

            if (!String.IsNullOrEmpty(name))
                temp = temp.Where(s => s.Name == name);
            if (!String.IsNullOrEmpty(group))
                temp = temp.Where(s => s.GroupName == group);
           
            temp = temp.Where(s => s.Birthdate == birthdate);
            return temp.ToList<Student>();
        }

        public void AddNewStudent(Student student)
        {
            students.Add(student);
        }
        public void ChangeStudentById(int studentId, string name, DateTime birthdate, string group, string uni)
        {
            Student student = new Student() { Name = name, Birthdate = birthdate, GroupName = group, University = uni };
            students[studentId] = student;
        }
    }
}