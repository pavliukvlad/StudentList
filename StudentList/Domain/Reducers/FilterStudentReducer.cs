using System;
using StudentList.Domain.States;

namespace StudentList.Domain.Reducers
{
    public static class FilterStudentReducer
    {
        public static FilterStudentsState Reduce(FilterStudentsState state, object action)
        {
            switch (action)
            {
                default:
                    return state;
            }
        }
    }
}
