using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Java.IO;

namespace StudentList.Services
{
    public static class PhotoService
    {
        public static async Task<Uri> SavePhotoAsync(Bitmap bitmap, string fileName, Context context)
        {
            string path = string.Empty;

            try
            {
                var root = context.GetDir("Images", FileCreationMode.Private);
                var file = new Java.IO.File(root, fileName);

                using (var stream = new FileOutputStream(file))
                {
                    byte[] arr;

                    using (var stream2 = new MemoryStream())
                    {
                        await bitmap.CompressAsync(Bitmap.CompressFormat.Png, 100, stream2);
                        arr = stream2.ToArray();
                    }

                    bitmap.Recycle();
                    path = file.Path;

                    await stream.WriteAsync(arr);
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Can't save a profile photo!");
            }

            return new Uri(path);
        }
    }
}
