using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StudentList.Model
{
    [Serializable]
    public class StudentFilter
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
