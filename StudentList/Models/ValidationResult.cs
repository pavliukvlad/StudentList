using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StudentList.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            this.Errors = new Dictionary<string, IEnumerable<string>>();
        }

        public bool IsValid => !this.Errors.Any();

        public Dictionary<string, IEnumerable<string>> Errors { get; private set; }
    }
}
