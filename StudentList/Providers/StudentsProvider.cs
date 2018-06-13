using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StudentList
{
    class StudentsProvider
    {
        private static List<Student> students = new List<Student>() {
            new Student() { Age = 18, Name = "Vlad", GroupName = "MN", University = "Lviv Polytechnic" },
            new Student() { Age = 19, Name = "Sasha", GroupName = "MN", University = "Lviv Polytechnic" },
            new Student() { Age = 19, Name = "Dima", GroupName = "MN", University = "Lviv Polytechnic" },
            new Student() { Age = 19, Name = "Taras", GroupName = "MN", University = "Lviv Polytechnic" }
        };
        
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

        public void AddNewStudent(Student student)
        {
            students.Add(student);
        }
        public static StudentsProvider NewInstance()
        {
            return new StudentsProvider();
        }
    }

    class Student   
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string University { get; set; }
    }
}