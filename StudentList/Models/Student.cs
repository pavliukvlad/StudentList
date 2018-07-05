﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Media;

namespace StudentList.Model
{
    public class Student
    {
        public string Id { get; set; }

        public DateTime Birthdate { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }

        public string University { get; set; }

        public string Phone { get; set; }

        public Uri ProfilePhoto { get; set; }
    }
}
