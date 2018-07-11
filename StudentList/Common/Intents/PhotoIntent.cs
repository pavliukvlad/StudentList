using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;
using Android.Provider;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace StudentList.Common.Intents
{
    public static class PhotoIntent
    {
        public static async Task<Intent> CreateImageChooserIntentAsync(Context context, params Intent[] options)
        {
            if (options == null || !options.Any())
            {
                var optionsList = new List<Intent>();

                if (IsGaleryAppsInstalled(context))
                {
                    optionsList.Add(CreateGaleryPickIntent());
                }

                if (await IsCameraAppsInstalledAsync(context))
                {
                    optionsList.Add(CreateCameraPickIntent());
                }

                options = optionsList.ToArray();
            }

            var intent = Intent.CreateChooser(options[0], context.GetString(Resource.String.choose_picture_intent_title));

            if (options.Length > 1)
            {
                intent.PutExtra(Intent.ExtraInitialIntents, options.Skip(1).ToArray());
            }

            return intent;
        }

        private static Intent CreateCameraPickIntent()
        {
            var intent = new Intent(MediaStore.ActionImageCapture);
            return intent;
        }

        private static Intent CreateGaleryPickIntent()
        {
            var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            return intent;
        }

        private static async Task<bool> IsCameraAppsInstalledAsync(Context context)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
            if (status == PermissionStatus.Granted)
            {
                var intent = new Intent(MediaStore.ActionImageCapture);
                var availableActivities = context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);

                return availableActivities != null && availableActivities.Any();
            }

            return false;
        }

        private static bool IsGaleryAppsInstalled(Context context)
        {
            var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            var availableACtivities = context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);

            return availableACtivities != null && availableACtivities.Any();
        }
    }
}
