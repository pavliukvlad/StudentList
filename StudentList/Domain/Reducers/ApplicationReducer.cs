using StudentList.Domain.Reducers;
using StudentList.Domain.States;

namespace StudentList.Domain
{
    public static class ApplicationReducer
    {
        public static ApplicationState Reducer(ApplicationState state, object action)
        {
            return new ApplicationState(
                FilterStudentReducer.Reduce(state.FilterStudentState, action),
                StudentListReducer.Reduce(state.StudentList, action),
                StudentProfileReducer.Reduce(state.StudentProfileState, action));
        }
    }
}
