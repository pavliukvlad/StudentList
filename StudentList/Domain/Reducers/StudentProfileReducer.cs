using StudentList.Domain.Actions;
using StudentList.Domain.States;
using StudentList.Models;

namespace StudentList.Domain.Reducers
{
    public static class StudentProfileReducer
    {
        public static StudentProfileState Reduce(StudentProfileState state, object action)
        {
            switch (action)
            {
                case StudentSelected studentSelected:
                    return Reduce(studentSelected);
                case AddNewStudent addNewStudent:
                    return Reduce(addNewStudent);
                default:
                    return state;
            }
        }

        private static StudentProfileState Reduce(AddNewStudent action)
        {
            return new StudentProfileState(action.Student.ToStudentImmutable());
        }

        private static StudentProfileState Reduce(StudentSelected action)
        {
            return new StudentProfileState(action.Student.ToStudentImmutable());
        }
    }
}
