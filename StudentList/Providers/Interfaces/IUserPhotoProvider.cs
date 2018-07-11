using Android.Graphics;
using StudentList.Models;

namespace StudentList.Providers.Interfaces
{
    public interface IUserPhotoProvider
    {
        Bitmap GetUserPhoto(Student student);
    }
}
