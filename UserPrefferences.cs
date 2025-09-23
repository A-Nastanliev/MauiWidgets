#if ANDROID
using Android.Content;
using Android.Appwidget;
using Android.Widget; 
using Android.Graphics;
using MauiWidgets.Platforms.Android;

namespace MauiWidgets
{
    public static class UserPreferences
    {
        private const string PrefsName = "my_widget_prefs";
        private const string NameKey = "username";
        private const string ImageKey = "user_image";

        public static string Username
        {
            get
            {
                var context = Android.App.Application.Context;
                var prefs = context.GetSharedPreferences(PrefsName, FileCreationMode.Private);
                return prefs.GetString(NameKey, "User");
            }
            set
            {
                var context = Android.App.Application.Context;
                var prefs = context.GetSharedPreferences(PrefsName, FileCreationMode.Private);
                var editor = prefs.Edit();
                editor.PutString(NameKey, value);
                editor.Apply();

                RefreshUsernameWidgets(context);
            }
        }

        public static string ImagePath
        {
            get
            {
                var context = Android.App.Application.Context;
                var prefs = context.GetSharedPreferences(PrefsName, FileCreationMode.Private);
                return prefs.GetString(ImageKey, null);
            }
            set
            {
                var context = Android.App.Application.Context;
                var prefs = context.GetSharedPreferences(PrefsName, FileCreationMode.Private);
                var editor = prefs.Edit();
                editor.PutString(ImageKey, value);
                editor.Apply();

                RefreshImageWidgets(context);
            }
        }

        private static void RefreshUsernameWidgets(Context context)
        {
            var appWidgetManager = AppWidgetManager.GetInstance(context);
            var componentName = new ComponentName(context, Java.Lang.Class.FromType(typeof(username_widget_class)));
            var widgetIds = appWidgetManager.GetAppWidgetIds(componentName);

            foreach (var widgetId in widgetIds)
            {
                RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.username_widget_layout);
                views.SetTextViewText(Resource.Id.username_text_view, Username);
                appWidgetManager.UpdateAppWidget(widgetId, views);
            }
        }

        private static void RefreshImageWidgets(Context context)
        {
            var appWidgetManager = AppWidgetManager.GetInstance(context);
            var componentName = new ComponentName(context, Java.Lang.Class.FromType(typeof(picked_image_widget_class)));
            var widgetIds = appWidgetManager.GetAppWidgetIds(componentName);

            foreach (var widgetId in widgetIds)
            {
                RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.picked_image_widget_layout);

                // Update only the image
                var fileName = ImagePath;
                if (!string.IsNullOrEmpty(fileName))
                {
                    var localPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, fileName);
                    if (File.Exists(localPath))
                    {
                        var bitmap = DecodeAndScaleImage(localPath, 300, 300); // adjust to widget size
                        if (bitmap != null)
                        {
                            views.SetImageViewBitmap(Resource.Id.user_image_view, bitmap);
                        }
                    }
                }
                appWidgetManager.UpdateAppWidget(widgetId, views);
            }
        }

        /// <summary>
        /// Decode and scale image to avoid exceeding RemoteViews bitmap memory limit.
        /// </summary>
        public static Bitmap DecodeAndScaleImage(string path, int targetWidth, int targetHeight)
        {
            // Decode bounds only
            var options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(path, options);

            int photoW = options.OutWidth;
            int photoH = options.OutHeight;

            // Compute scale factor
            int scaleFactor = Math.Min(photoW / targetWidth, photoH / targetHeight);
            if (scaleFactor < 1) scaleFactor = 1;

            // Decode scaled bitmap
            options.InJustDecodeBounds = false;
            options.InSampleSize = scaleFactor;

            return BitmapFactory.DecodeFile(path, options);
        }
    }
}

#endif
