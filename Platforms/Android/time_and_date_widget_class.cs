using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;
using Color = Android.Graphics.Color;

namespace MauiWidgets.Platforms.Android {

    [BroadcastReceiver(Label = "Time & Date", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/time_and_date_widget_provider")]
    [Service(Exported = true)]

    internal class time_and_date_widget_class : AppWidgetProvider {
        /// <summary>
        /// Called when the widget is updated (e.g., when placed on the home screen).
        /// Sets up the alarm manager to refresh the widget every minute.
        /// </summary>
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            base.OnUpdate(context, appWidgetManager, appWidgetIds);

            // Update immediately
            ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(time_and_date_widget_class)).Name);
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context));

            // Schedule updates every minute
            ScheduleMinuteUpdates(context);
        }

        /// <summary>
        /// Handles broadcasts, including our scheduled "update_time" action.
        /// </summary>
        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            if (intent.Action == "update_time")
            {
                AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(context);
                ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(time_and_date_widget_class)).Name);
                appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context));

                // Schedule the next update (so it always aligns exactly on the minute)
                ScheduleMinuteUpdates(context);
            }
        }

        /// <summary>
        /// Creates the RemoteViews for the widget.
        /// </summary>
        private RemoteViews BuildRemoteViews(Context context)
        {
            RemoteViews widgetView = new RemoteViews(context.PackageName, Resource.Layout.time_and_date_widget_layout);

            SetTextViewText(widgetView);

            return widgetView;
        }

        /// <summary>
        /// Sets the text of the widget to the current time and date.
        /// </summary>
        private void SetTextViewText(RemoteViews widgetView)
        {
            widgetView.SetTextViewText(Resource.Id.time_and_date_widget_time_and_date,
                DateTime.Now.ToString("H:mm\nd.M.yyyy"));
        }

        /// <summary>
        /// Schedules the widget to update every minute using AlarmManager.
        /// </summary>
        private void ScheduleMinuteUpdates(Context context)
        {
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

            Intent intent = new Intent(context, typeof(time_and_date_widget_class));
            intent.SetAction("update_time");

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(
                context,
                0,
                intent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            // Current system time in ms
            long currentTime = Java.Lang.JavaSystem.CurrentTimeMillis();

            // Calculate first trigger: start of the next minute
            long triggerAtMillis = currentTime - (currentTime % 60000) + 60000;

            // Use exact alarm so it fires exactly at the start of the minute
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.Rtc, triggerAtMillis, pendingIntent);
        }

    }
}
