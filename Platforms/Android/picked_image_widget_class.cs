using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Widget;

namespace MauiWidgets.Platforms.Android
{
    [BroadcastReceiver(Label = "Picked Image", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/picked_image_widget_provider")]
    internal class picked_image_widget_class: AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            base.OnUpdate(context, appWidgetManager, appWidgetIds);

            foreach (var widgetId in appWidgetIds)
            {
                UpdateWidget(context, appWidgetManager, widgetId);
            }
        }

        public override void OnDeleted(Context context, int[] appWidgetIds)
        {
            base.OnDeleted(context, appWidgetIds);

            var appWidgetManager = AppWidgetManager.GetInstance(context);
            var componentName = new ComponentName(context, Java.Lang.Class.FromType(typeof(picked_image_widget_class)));
            var remainingIds = appWidgetManager.GetAppWidgetIds(componentName);

            // If there are no more widgets from the "picked_image_widget"s then delete the image
            if (remainingIds == null || remainingIds.Length == 0)
            {
                var fileName = UserPreferences.ImagePath;
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

                UserPreferences.ImagePath = null;
            }
        }

        public override void OnAppWidgetOptionsChanged(Context context, AppWidgetManager appWidgetManager, int appWidgetId, Bundle newOptions)
        {
            base.OnAppWidgetOptionsChanged(context, appWidgetManager, appWidgetId, newOptions);

            UpdateWidget(context, appWidgetManager, appWidgetId);
        }

        private void UpdateWidget(Context context, AppWidgetManager appWidgetManager, int widgetId)
        {
            RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.picked_image_widget_layout);

            var fileName = UserPreferences.ImagePath;
            if (!string.IsNullOrEmpty(fileName))
            {
                var localPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, fileName);
                if (File.Exists(localPath))
                {
                    int targetWidth = 300;
                    int targetHeight = 300;

                    var options = appWidgetManager.GetAppWidgetOptions(widgetId);
                    if (options != null)
                    {
                        int maxWidth = options.GetInt(AppWidgetManager.OptionAppwidgetMaxWidth);
                        int maxHeight = options.GetInt(AppWidgetManager.OptionAppwidgetMaxHeight);

                        if (maxWidth > 0) targetWidth = UserPreferences.DpToPx(context, maxWidth);
                        if (maxHeight > 0) targetHeight = UserPreferences.DpToPx(context, maxHeight);
                    }

                    var bitmap = UserPreferences.DecodeAndScaleImage(localPath, targetWidth, targetHeight);
                    if (bitmap != null)
                    {
                        views.SetImageViewBitmap(Resource.Id.user_image_view, bitmap);
                    }
                }
            }

            appWidgetManager.UpdateAppWidget(widgetId, views);
        }
    }
}
