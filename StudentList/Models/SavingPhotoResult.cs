using System;

namespace StudentList.Models
{
    public class SavingPhotoResult
    {
        public Uri ProfilePhotoUri { get; set; }

        public bool IsPhotoSelected { get; set; }

        public bool IsError { get; set; }
    }
}
