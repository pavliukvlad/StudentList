using System;
using System.Collections.Generic;
using System.Linq;
using StudentList.Domain.Actions;
using StudentList.Models;

namespace StudentList.Domain.Reducers
{
    public static class StudentListReducer
    {
        public static IEnumerable<StudentImmutable> Reduce(IEnumerable<StudentImmutable> state, object action)
        {
            switch (action)
            {
                case StudentsReceived studentReceivedAction:
                    return Reduce(studentReceivedAction);
                case StudentListChanged studentListUpdated:
                    return Reduce();
                default:
                    return state;
            }
        }

        private static IEnumerable<StudentImmutable> Reduce()
        {
            return new List<StudentImmutable>();
        }

        private static IEnumerable<StudentImmutable> Reduce(StudentsReceived studentReceivedAction)
        {
            return studentReceivedAction.StudentList.Select(s => s.ToStudentImmutable()).ToList();
        }
    }
}
