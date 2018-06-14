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
using StudentList.Model;

namespace StudentList.Providers.Interfaces
{
    interface IStudentRepository
    {
        void AddNewStudent(Student student);

        int Count { get; }
        Student this[int index] { get; set; }
    }
}