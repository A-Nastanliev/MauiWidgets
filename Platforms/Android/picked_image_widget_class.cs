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
                   /* if (File.Exists(localPath))
                    {
                        var bitmap = BitmapFactory.DecodeFile(localPath);
                        views.SetImageViewBitmap(Resource.Id.user_image_view, bitmap);
                    }*/
                    if (File.Exists(localPath))
                    {
                        // ✅ Use scaled image
                        var bitmap = UserPreferences.DecodeAndScaleImage(localPath, 300, 300); // adjust to widget size
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
