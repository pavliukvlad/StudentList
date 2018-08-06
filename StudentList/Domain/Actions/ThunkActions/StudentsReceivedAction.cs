using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using StudentList.Common.Dialogs;
using StudentList.Domain.States;
using StudentList.Models;

namespace StudentList.Domain.Actions.ThunkActions
{
    public class StudentsReceivedAction
    {
        private readonly Activity activity;
        private readonly Task<IList<Student>> studentsTask;

        public StudentsReceivedAction(Activity activity, Task<IList<Student>> studentsTask)
        {
            this.activity = activity;
            this.studentsTask = studentsTask;
            this.Action = this.GetStudents;
        }

        public ThunkAction<ApplicationState> Action { get; set; }

        private async void GetStudents(Dispatcher dispatcher, Func<ApplicationState> getState)
        {
            var loadingDialog = new LoadingDialog(this.activity);
            loadingDialog.Show();
            var students = await this.studentsTask;
            loadingDialog.Hide();

            dispatcher(new StudentsReceived() { StudentList = students });
        }
    }
}
