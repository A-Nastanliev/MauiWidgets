using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWidgets.Platforms.Android
{
    [BroadcastReceiver(Label = "Username Widget", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/username_widget_provider")]
    public class username_widget_class : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            foreach (var widgetId in appWidgetIds)
            {
                RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.username_widget_layout);

                // Set current username from SharedPreferences
                views.SetTextViewText(Resource.Id.username_text_view, UserPreferences.Username);

                appWidgetManager.UpdateAppWidget(widgetId, views);
            }
        }

    }
}
