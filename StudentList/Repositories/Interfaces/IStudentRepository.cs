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
using StudentList.Models;

namespace StudentList.Providers.Interfaces
{
    public interface IStudentRepository
    {
        Task<ValidationResult> AddNewStudentAsync(string name, string birthdate, string group, string uni, string phone);

        Task<ValidationResult> ChangeStudentById(string studentId, string name, string birthdate, string group, string uni, string phone);

        Task<Student> GetStudentById(string id);

        Task<IList<Student>> GetStudentsAsync(StudentFilter studentFilter);
    }
}
