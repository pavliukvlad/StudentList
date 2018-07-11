using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Java.IO;
using StudentList.Constants;
using StudentList.Models;

namespace StudentList.Services
{
    public static class PhotoService
    {
        public static async Task<SavingPhotoResult> SavePhotoAsync(Bitmap bitmap, string fileName, Context context)
        {
            if (bitmap != null)
            {
                try
                {
                    var root = context.GetDir(StudentListDirNames.Image, FileCreationMode.Private);
                    var file = new Java.IO.File(root, fileName);

                    file.Delete();

                    using (var fileStream = new FileOutputStream(file))
                    {
                        byte[] arr;

                        using (var memoryStream = new MemoryStream())
                        {
                            await bitmap.CompressAsync(Bitmap.CompressFormat.Png, 100, memoryStream);
                            arr = memoryStream.ToArray();
                        }

                        await fileStream.WriteAsync(arr);

                        return new SavingPhotoResult()
                        {
                            ProfilePhotoUri = new Uri(file.Path),
                            IsPhotoSelected = true
                        };
                    }
                }
                catch
                {
                    return new SavingPhotoResult() { IsError = true };
                }
            }

            return new SavingPhotoResult() { IsPhotoSelected = false };
        }
    }
}
