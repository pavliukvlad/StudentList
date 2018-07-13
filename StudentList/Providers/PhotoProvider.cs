using Android.Graphics;
using StudentList.Models;
using StudentList.Providers.Interfaces;

namespace StudentList.Providers
{
    public class PhotoProvider : IUserPhotoProvider
    {
        public Bitmap GetUserPhoto(Student student)
        {
            Bitmap image = null;

            if (student.ProfilePhoto != null)
            {
                 image = BitmapFactory.DecodeFile(student.ProfilePhoto.AbsolutePath);
            }

            return image;
        }
    }
}
