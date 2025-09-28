using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Widget;

namespace MauiWidgets.Platforms.Android
{
    [BroadcastReceiver(Label = "Image", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/image_widget_provider")]
    internal class image_widget_class: AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            foreach (int appWidgetId in appWidgetIds)
            {
                UpdateAppWidget(context, appWidgetManager, appWidgetId);
            }
        }

        public static void UpdateAppWidget(Context context, AppWidgetManager appWidgetManager, int appWidgetId)
        {
            var prefs = context.GetSharedPreferences("WidgetPrefs", FileCreationMode.Private);
            var fileName = prefs.GetString($"img_{appWidgetId}", null);

            RemoteViews views = new(context.PackageName, Resource.Layout.image_widget_layout);

            if (!string.IsNullOrEmpty(fileName))
            {
                var localPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, fileName);
                if (File.Exists(localPath))
                {
                    int targetWidth = 300;
                    int targetHeight = 300;

                    var options = appWidgetManager.GetAppWidgetOptions(appWidgetId);
                    if (options != null)
                    {
                        int maxWidth = options.GetInt(AppWidgetManager.OptionAppwidgetMaxWidth);
                        int maxHeight = options.GetInt(AppWidgetManager.OptionAppwidgetMaxHeight);

                        if (maxWidth > 0) targetWidth = UserPreferences.DpToPx(context, maxWidth);
                        if (maxHeight > 0) targetHeight = UserPreferences.DpToPx(context, maxHeight);
                    }

                    var bitmap = UserPreferences.DecodeAndScaleImage(localPath, targetWidth, targetHeight);
                    if (bitmap != null)
                        views.SetImageViewBitmap(Resource.Id.image_view, bitmap);
                }
                else
                {
                    views.SetImageViewResource(Resource.Id.image_view, Resource.Drawable.placeholder_image);
                }
            }
            else
            {
                views.SetImageViewResource(Resource.Id.image_view, Resource.Drawable.placeholder_image);
            }

            // Tap to open picker activity
            Intent intent = new(context, typeof(image_config_activity));
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetId, appWidgetId);
            intent.SetFlags(ActivityFlags.NewTask);

            PendingIntent pendingIntent = PendingIntent.GetActivity(
                context,
                appWidgetId, // unique per widget
                intent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
            );

            views.SetOnClickPendingIntent(Resource.Id.image_view, pendingIntent);

            appWidgetManager.UpdateAppWidget(appWidgetId, views);
        }

        public override void OnAppWidgetOptionsChanged(Context context, AppWidgetManager appWidgetManager, int appWidgetId, Bundle newOptions)
        {
            base.OnAppWidgetOptionsChanged(context, appWidgetManager, appWidgetId, newOptions);

            // Re-render this widget with correct dimensions
            UpdateAppWidget(context, appWidgetManager, appWidgetId);
        }

        public override void OnDeleted(Context context, int[] appWidgetIds)
        {
            base.OnDeleted(context, appWidgetIds);

            var prefs = context.GetSharedPreferences("WidgetPrefs", FileCreationMode.Private);
            var editor = prefs.Edit();

            foreach (var appWidgetId in appWidgetIds)
            {
                // Delete the stored image file
                var fileName = prefs.GetString($"img_{appWidgetId}", null);
                if (!string.IsNullOrEmpty(fileName))
                {
                    var localPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, fileName);
                    if (File.Exists(localPath))
                    {
                        try 
                        { 
                            File.Delete(localPath); 
                        } 
                        catch 
                        { 
                            
                        }
                    }
                }

                // Remove from prefs
                editor.Remove($"img_{appWidgetId}");
            }

            editor.Apply();
        }
    }
}
