using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StudentList.Model;

namespace StudentList.Providers.Interfaces
{
    interface IStudentRepository
    {
        IList<Student> Students { get; }
        int Count { get; }
        Student this[string index] { get; set; }

        void AddNewStudent(Student student);
        void ChangeStudentById(string studentId, string name, DateTime birthdate, string group, string uni);

        Task<IList<Student>> GetStudentsAsync(StudentFilter studentFilter);
    }
}