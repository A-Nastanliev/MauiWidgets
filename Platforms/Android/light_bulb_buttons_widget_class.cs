using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;

namespace MauiWidgets.Platforms.Android {

    [BroadcastReceiver(Label = "Light Bulb Buttons", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/light_bulb_buttons_widget_provider")]
    [Service(Exported = true)]
    internal class light_bulb_buttons_widget_class : AppWidgetProvider {
        /// <summary>
        /// Something like a constructor, usually the first 2 lines stay the same.
        /// </summary>
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds) {
            ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(light_bulb_buttons_widget_class)).Name);
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context));
        }

        /// <summary>
        /// The BuildRemoteViews method creates (updates) the look and functions of the widget.
        /// </summary>
        private RemoteViews BuildRemoteViews(Context context) {
            RemoteViews widgetView = new RemoteViews(context.PackageName, Resource.Layout.light_bulb_buttons_widget_layout);

            // Create intents for individual buttons
            Intent intent_on = new Intent(context, typeof(light_bulb_buttons_widget_class));
            Intent intent_off = new Intent(context, typeof(light_bulb_buttons_widget_class));

            // Set actions for individual buttons
            intent_on.SetAction("set_on");
            intent_off.SetAction("set_off");

            // Create PendingIntent for individual buttons
            PendingIntent pendingIntent_on = PendingIntent.GetBroadcast(context, 0, intent_on, PendingIntentFlags.Immutable);
            PendingIntent pendingIntent_off = PendingIntent.GetBroadcast(context, 0, intent_off, PendingIntentFlags.Immutable);

            // Assign onClick PendingIntent to individual buttons
            widgetView.SetOnClickPendingIntent(Resource.Id.light_bulb_buttons_widget_on, pendingIntent_on);
            widgetView.SetOnClickPendingIntent(Resource.Id.light_bulb_buttons_widget_off, pendingIntent_off);

            return widgetView;
        }

        /// <summary>
        /// The OnReceive method reacts to clicks on the widget.
        /// </summary>
        public override void OnReceive(Context context, Intent intent) {
            RemoteViews widgetView = BuildRemoteViews(context);
            ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(light_bulb_buttons_widget_class)).Name);

            switch (intent.Action) {
                case "set_on":
                    // Action for clicking the "on" button
                    widgetView.SetImageViewResource(Resource.Id.light_bulb_buttons_widget_light_bulb, Resource.Drawable.light_bulb_buttons_and_switch_widget_image_on);
                    break;
                case "set_off":
                    // Action for clicking the "off" button
                    widgetView.SetImageViewResource(Resource.Id.light_bulb_buttons_widget_light_bulb, Resource.Drawable.light_bulb_buttons_and_switch_widget_image_off);
                    break;
                default:
                    // Element doesn’t have an onClick function assigned
                    break;
            }

            AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(context);

            // Update the widget
            appWidgetManager.UpdateAppWidget(me, widgetView);

            // Call the method from the parent class
            base.OnReceive(context, intent);
        }
    }
}
