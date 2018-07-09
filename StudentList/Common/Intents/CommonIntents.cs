using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Content.PM;
using Android.Provider;

namespace StudentList.Common.Intents
{
    public static class CommonIntents
    {
        public static Intent CreateImageChooserIntent(Context context, params Intent[] options)
        {
            if (options == null || !options.Any())
            {
                var optionsList = new List<Intent>();

                if (IsGaleryAppsInstalled(context))
                {
                    optionsList.Add(CreateGaleryPickIntent());
                }

                if (IsCameraAppsInstalled(context))
                {
                    optionsList.Add(IsCameraPickIntent());
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

        private static Intent IsCameraPickIntent()
        {
            var intent = new Intent(MediaStore.ActionImageCapture);
            return intent;
        }

        private static Intent CreateGaleryPickIntent()
        {
            var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            return intent;
        }

        private static bool IsCameraAppsInstalled(Context context)
        {
            var intent = new Intent(MediaStore.ActionImageCapture);
            var availableActivities = context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);

            return availableActivities != null && availableActivities.Any();
        }

        private static bool IsGaleryAppsInstalled(Context context)
        {
            var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            var availableACtivities = context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);

            return availableACtivities != null && availableACtivities.Any();
        }
    }
}
