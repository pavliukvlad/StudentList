using System.Collections.Generic;
using StudentList.Domain.Actions;
using StudentList.Models;

namespace StudentList.Domain.Reducers
{
    public static class StudentListReducer
    {
        public static IEnumerable<Student> Reduce(IEnumerable<Student> state, object action)
        {
            switch (action)
            {
                case StudentReceived studentReceivedAction:
                    return Reduce(studentReceivedAction);
                case StudentListChanged studentListUpdated:
                    return null;
                default:
                    return state;
            }
        }

        private static IEnumerable<Student> Reduce(StudentReceived studentReceivedAction)
        {
            return studentReceivedAction.StudentList;
        }
    }
}
