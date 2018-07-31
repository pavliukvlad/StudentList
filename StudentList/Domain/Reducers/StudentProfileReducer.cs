using System;
using StudentList.Domain.Actions;
using StudentList.Domain.States;

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

        private static StudentProfileState Reduce(AddNewStudent addNewStudent)
        {
            return new StudentProfileState(null);
        }

        private static StudentProfileState Reduce(StudentSelected studentSelected)
        {
            return new StudentProfileState(studentSelected.Student);
        }
    }
}
