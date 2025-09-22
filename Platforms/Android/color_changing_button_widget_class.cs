using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;

namespace MauiWidgets.Platforms.Android
{
    [BroadcastReceiver(Label = "Color Changing Button", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/color_changing_button_widget_provider")]
    internal class color_changing_button_widget_class : AppWidgetProvider
    {
        private const string ACTION_TAP = "color_button_tap";

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(color_changing_button_widget_class)).Name);
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context));
        }

        private RemoteViews BuildRemoteViews(Context context)
        {
            var views = new RemoteViews(context.PackageName, Resource.Layout.color_changing_button_widget_layout);

            // Set up PendingIntent for button click
            Intent intent = new Intent(context, typeof(color_changing_button_widget_class));
            intent.SetAction(ACTION_TAP);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(
                context,
                0,
                intent,
                PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent
            );

            views.SetOnClickPendingIntent(Resource.Id.color_changing_button_widget_button, pendingIntent);

            return views;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == ACTION_TAP)
            {
                RemoteViews widgetView = new RemoteViews(context.PackageName, Resource.Layout.color_changing_button_widget_layout);

                // Generate a random opaque color
                var random = new System.Random();
                int color = unchecked((int)(0xFF000000 | random.Next(0xFFFFFF + 1)));

                // Apply random color to the button background
                widgetView.SetInt(Resource.Id.color_changing_button_widget_button, "setBackgroundColor", color);

                // Update widget
                ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(color_changing_button_widget_class)).Name);
                var appWidgetManager = AppWidgetManager.GetInstance(context);
                appWidgetManager.UpdateAppWidget(me, widgetView);
            }

            base.OnReceive(context, intent);
        }
    }
}
