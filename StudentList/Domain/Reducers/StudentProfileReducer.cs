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
                default:
                    return state;
            }
        }
    }
}
