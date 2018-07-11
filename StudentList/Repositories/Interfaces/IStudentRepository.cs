using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentList.Models;

namespace StudentList.Providers.Interfaces
{
    public interface IStudentRepository
    {
        Task<ValidationResult> AddNewStudentAsync(string name, SavingPhotoResult photoResult, string birthdate, string group, string uni, string phone);

        Task<ValidationResult> ChangeStudentById(string studentId, SavingPhotoResult photoResult, string name, string birthdate, string group, string uni, string phone);

        Task<Student> GetStudentById(string id);

        Task<IList<Student>> GetStudentsAsync(StudentFilter studentFilter);
    }
}
