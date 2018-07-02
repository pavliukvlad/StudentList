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
    public interface IStudentRepository
    {
        Task AddNewStudent(Student student);

        Task ChangeStudentById(string studentId, string name, DateTime birthdate, string group, string uni, string phone);

        Task<Student> GetStudentById(string id);

        Task<IList<Student>> GetStudentsAsync(StudentFilter studentFilter);
    }
}
