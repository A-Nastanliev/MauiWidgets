using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;

namespace MauiWidgets.Platforms.Android {

    [BroadcastReceiver(Label = "Color Press Game", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/color_press_game_widget_provider")]
    [Service(Exported = true)]

    internal class color_press_game_widget_class : AppWidgetProvider {

        /// <summary>
        /// Kind of like a constructor — usually the first 2 lines stay the same.
        /// </summary>
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds) {
            ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(color_press_game_widget_class)).Name);
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context));
        }

        /// <summary>
        /// The BuildRemoteViews method creates (or updates) the widget’s appearance and functionality.
        /// </summary>
        private RemoteViews BuildRemoteViews(Context context) {
            RemoteViews widgetView = new RemoteViews(context.PackageName, Resource.Layout.color_press_game_widget_layout);

            // Create intents for different colors
            Intent intent_green = new Intent(context, typeof(color_press_game_widget_class));
            Intent intent_blue = new Intent(context, typeof(color_press_game_widget_class));
            Intent intent_red = new Intent(context, typeof(color_press_game_widget_class));
            Intent intent_purple = new Intent(context, typeof(color_press_game_widget_class));
            Intent intent_white = new Intent(context, typeof(color_press_game_widget_class));

            // Set actions for each color
            intent_green.SetAction("chosen_green");
            intent_blue.SetAction("chosen_blue");
            intent_red.SetAction("chosen_red");
            intent_purple.SetAction("chosen_purple");
            intent_white.SetAction("chosen_white");

            // Create PendingIntents for each color
            PendingIntent pendingIntent_green = PendingIntent.GetBroadcast(context, 0, intent_green, PendingIntentFlags.Immutable);
            PendingIntent pendingIntent_blue = PendingIntent.GetBroadcast(context, 0, intent_blue, PendingIntentFlags.Immutable);
            PendingIntent pendingIntent_red = PendingIntent.GetBroadcast(context, 0, intent_red, PendingIntentFlags.Immutable);
            PendingIntent pendingIntent_purple = PendingIntent.GetBroadcast(context, 0, intent_purple, PendingIntentFlags.Immutable);
            PendingIntent pendingIntent_white = PendingIntent.GetBroadcast(context, 0, intent_white, PendingIntentFlags.Immutable);

            // Assign the onClick PendingIntent for each color
            widgetView.SetOnClickPendingIntent(Resource.Id.color_press_game_widget_color_green, pendingIntent_green);
            widgetView.SetOnClickPendingIntent(Resource.Id.color_press_game_widget_color_blue, pendingIntent_blue);
            widgetView.SetOnClickPendingIntent(Resource.Id.color_press_game_widget_color_red, pendingIntent_red);
            widgetView.SetOnClickPendingIntent(Resource.Id.color_press_game_widget_color_purple, pendingIntent_purple);
            widgetView.SetOnClickPendingIntent(Resource.Id.color_press_game_widget_color_white, pendingIntent_white);

            return widgetView;
        }

        /// <summary>
        /// The OnReceive method handles clicks on the widget.
        /// </summary>
        public override void OnReceive(Context context, Intent intent) {
            RemoteViews widgetView = BuildRemoteViews(context);
            ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(color_press_game_widget_class)).Name);

            // Set default text for each color after click
            widgetView.SetTextViewText(Resource.Id.color_press_game_widget_color_green, "green");
            widgetView.SetTextViewText(Resource.Id.color_press_game_widget_color_blue, "blue");
            widgetView.SetTextViewText(Resource.Id.color_press_game_widget_color_red, "red");
            widgetView.SetTextViewText(Resource.Id.color_press_game_widget_color_purple, "purple");
            widgetView.SetTextViewText(Resource.Id.color_press_game_widget_color_white, "white");


            switch (intent.Action) {
                case "chosen_green":
                    // Action when green is clicked
                    widgetView.SetTextViewText(Resource.Id.color_press_game_widget_color_green, "clicked");
                    break;
                case "chosen_blue":
                    // Action when blue is clicked
                    widgetView.SetTextViewText(Resource.Id.color_press_game_widget_color_blue, "clicked");
                    break;
                case "chosen_red":
                    // Action when red is clicked
                    widgetView.SetTextViewText(Resource.Id.color_press_game_widget_color_red, "clicked");
                    break;
                case "chosen_purple":
                    // Action when purple is clicked
                    widgetView.SetTextViewText(Resource.Id.color_press_game_widget_color_purple, "clicked");
                    break;
                case "chosen_white":
                    // Action when white is clicked
                    widgetView.SetTextViewText(Resource.Id.color_press_game_widget_color_white, "clicked");
                    break;
                default:
                    // The element doesn’t have an onClick function set
                    break;
            }

            AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(context);

            // Update the widget
            appWidgetManager.UpdateAppWidget(me, widgetView);

            // Call the parent class method
            base.OnReceive(context, intent);
        }
    }
}
