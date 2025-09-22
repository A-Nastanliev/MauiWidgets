using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Bumptech.Glide.Request.Target;
using Bumptech.Glide.Util;
using static Android.Widget.RemoteViews;

namespace MauiWidgets.Platforms.Android
{

    [BroadcastReceiver(Label = "Light Bulb Switch", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/light_bulb_switch_widget_provider")]
    [Service(Exported = true)]
    internal class light_bulb_switch_widget_class : AppWidgetProvider
    {
        /// <summary>
        /// Something like a constructor — usually the first 2 lines remain the same.
        /// </summary>
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(light_bulb_switch_widget_class)).Name);
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context));
        }

        /// <summary>
        /// The BuildRemoteViews method creates (or updates) the widget’s appearance and behavior.
        /// </summary>
        private RemoteViews BuildRemoteViews(Context context)
        {
            RemoteViews widgetView = new RemoteViews(context.PackageName, Resource.Layout.light_bulb_switch_widget_layout);

            // Create an intent for the switch
            Intent intent_switch = new Intent(context, typeof(light_bulb_switch_widget_class));

            // Set an action for the switch
            intent_switch.SetAction("switch");

            // Create a PendingIntent for the switch
            PendingIntent pendingIntent_switch = PendingIntent.GetBroadcast(context, 0, intent_switch, PendingIntentFlags.Immutable);

            // Assign the PendingIntent to the switch’s onClick
            widgetView.SetOnClickPendingIntent(Resource.Id.light_bulb_switch_widget_switch, pendingIntent_switch);

            return widgetView;
        }

        /// <summary>
        /// The OnReceive method handles clicks on the widget.
        /// </summary>
        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            if (intent.Action == "switch")
            {
                RemoteViews widgetView = new RemoteViews(context.PackageName, Resource.Layout.light_bulb_switch_widget_layout);

                // Read the current state (e.g., from SharedPreferences)
                var prefs = context.GetSharedPreferences("widget_prefs", FileCreationMode.Private);
                bool isOn = prefs.GetBoolean("switch_state", false);

                // Toggle it
                isOn = !isOn;
                prefs.Edit().PutBoolean("switch_state", isOn).Apply();

                // Update UI based on new state
                if (isOn)
                {
                    widgetView.SetTextViewText(Resource.Id.light_bulb_switch_widget_switch, "turn off");
                    widgetView.SetImageViewResource(Resource.Id.light_bulb_switch_widget_light_bulb,
                        Resource.Drawable.light_bulb_buttons_and_switch_widget_image_on);
                }
                else
                {
                    widgetView.SetTextViewText(Resource.Id.light_bulb_switch_widget_switch, "turn on");
                    widgetView.SetImageViewResource(Resource.Id.light_bulb_switch_widget_light_bulb,
                        Resource.Drawable.light_bulb_buttons_and_switch_widget_image_off);
                }

                // Re-register PendingIntent (so button keeps working after update)
                Intent switchIntent = new Intent(context, typeof(light_bulb_switch_widget_class));
                switchIntent.SetAction("switch");
                PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, 0, switchIntent, PendingIntentFlags.Immutable);
                widgetView.SetOnClickPendingIntent(Resource.Id.light_bulb_switch_widget_switch, pendingIntent);

                // Apply update
                ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(light_bulb_switch_widget_class)).Name);
                AppWidgetManager.GetInstance(context).UpdateAppWidget(me, widgetView);
            }
        }
    }
}
