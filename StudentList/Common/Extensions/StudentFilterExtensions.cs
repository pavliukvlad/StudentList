using StudentList.Models;

namespace StudentList.Common.Extensions
{
    public static class StudentFilterExtensions
    {
        public static bool IsDefault(this StudentFilter studentFilter)
        {
            if (studentFilter.Name == StudentFilter.Default.Name
                && studentFilter.Group == StudentFilter.Default.Group
                && studentFilter.Birthdate == StudentFilter.Default.Birthdate)
            {
                return true;
            }

            return false;
        }
    }
}
