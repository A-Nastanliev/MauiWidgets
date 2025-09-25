using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.Widget;

namespace MauiWidgets.Platforms.Android
{
    [BroadcastReceiver(Label = "Display Picked Image", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/picked_image_widget_provider")]
    internal class picked_image_widget_class: AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            base.OnUpdate(context, appWidgetManager, appWidgetIds);

            foreach (var widgetId in appWidgetIds)
            {
                RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.picked_image_widget_layout);

                var fileName = UserPreferences.ImagePath;
                if (!string.IsNullOrEmpty(fileName))
                {
                    var localPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, fileName);
                    if (File.Exists(localPath))
                    {
                        int targetWidth = 300;  // fallback
                        int targetHeight = 300; // fallback

                        // Try to get widget size info
                        var options = appWidgetManager.GetAppWidgetOptions(widgetId);
                        if (options != null)
                        {
                            int maxWidth = options.GetInt(AppWidgetManager.OptionAppwidgetMaxWidth);
                            int maxHeight = options.GetInt(AppWidgetManager.OptionAppwidgetMaxHeight);

                            if (maxWidth > 0) targetWidth = UserPreferences.DpToPx(context, maxWidth);
                            if (maxHeight > 0) targetHeight = UserPreferences.DpToPx(context, maxHeight);
                        }

                        // Scale image properly
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
}
