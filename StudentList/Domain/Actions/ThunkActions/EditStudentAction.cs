using System;
using System.Linq;
using StudentList.Domain.States;

namespace StudentList.Domain.Actions.ThunkActions
{
    public class EditStudentAction
    {
        public EditStudentAction(string studentId)
        {
            this.StudentID = studentId;
            this.Action = this.EditStudent;
        }

        public string StudentID { get; }

        public ThunkAction<ApplicationState> Action { get; set; }

        private void EditStudent(Dispatcher dispatcher, Func<ApplicationState> getState)
        {
            var selectedStudent = getState().StudentList.Where(student => student.Id == this.StudentID).FirstOrDefault();
            dispatcher(new StudentSelected() { Student = selectedStudent });
        }
    }
}
