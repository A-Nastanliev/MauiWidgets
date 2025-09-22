#if ANDROID
using Android.Content;
using Android.Appwidget;
using Android.Widget; 
using MauiWidgets.Platforms.Android;

namespace MauiWidgets
{
    public static class UserPreferences
    {
        private const string PrefsName = "my_widget_prefs";
        private const string NameKey = "username";

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

                RefreshAllWidgets(context);
            }
        }

        private static void RefreshAllWidgets(Context context)
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
    }
}

#endif
