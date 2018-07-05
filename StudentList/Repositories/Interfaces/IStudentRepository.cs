using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentList.Model;
using StudentList.Models;

namespace StudentList.Providers.Interfaces
{
    public interface IStudentRepository
    {
        Task<ValidationResult> AddNewStudentAsync(string name, Uri profilePhotoUri, string birthdate, string group, string uni, string phone);

        Task<ValidationResult> ChangeStudentById(string studentId, Uri profilePhotoUri, string name, string birthdate, string group, string uni, string phone);

        Task<Student> GetStudentById(string id);

        Task<IList<Student>> GetStudentsAsync(StudentFilter studentFilter);
    }
}
