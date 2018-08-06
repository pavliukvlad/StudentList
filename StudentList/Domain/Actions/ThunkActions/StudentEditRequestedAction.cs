using System;
using System.Linq;
using StudentList.Domain.States;

namespace StudentList.Domain.Actions.ThunkActions
{
    public class StudentEditRequestedAction
    {
        public StudentEditRequestedAction(string studentId)
        {
            this.StudentID = studentId;
            this.Action = this.EditStudent;
        }

        public string StudentID { get; }

        public ThunkAction<ApplicationState> Action { get; set; }

        public void EditStudent(Dispatcher dispatcher, Func<ApplicationState> getState)
        {
            var selectedStudent = getState().StudentList.FirstOrDefault(student => student.Id == this.StudentID);
            dispatcher(new StudentSelected() { Student = selectedStudent.ToStudent() });
        }
    }
}
