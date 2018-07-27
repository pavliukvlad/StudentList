using StudentList.Domain.Actions;
using StudentList.Domain.States;

namespace StudentList.Domain.Reducers
{
    public static class StudentListReducer
    {
        public static StudentListState Reduce(StudentListState state, object action)
        {
            switch (action)
            {
                default:
                    return state;
            }
        }
    }
}
