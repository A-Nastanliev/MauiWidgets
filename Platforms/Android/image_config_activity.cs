using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Widget;

namespace MauiWidgets.Platforms.Android
{
    [Activity(Exported = true, TaskAffinity ="", ExcludeFromRecents = true)]
    public class image_config_activity : Activity
    {
        const int PickImage = 1001;
        int appWidgetId = AppWidgetManager.InvalidAppwidgetId;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            appWidgetId = Intent?.GetIntExtra(AppWidgetManager.ExtraAppwidgetId, AppWidgetManager.InvalidAppwidgetId)
                          ?? AppWidgetManager.InvalidAppwidgetId;

            // Launch picker
            Intent pickIntent = new(Intent.ActionOpenDocument);
            pickIntent.AddCategory(Intent.CategoryOpenable);
            pickIntent.SetType("image/*");
            StartActivityForResult(pickIntent, PickImage);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == PickImage && resultCode == Result.Ok && data?.Data != null)
            {
                var uri = data.Data;
                if (uri != null)
                {
                    try
                    {
                        // Copy picked image into app’s private storage
                        var fileName = $"widget_{appWidgetId}.jpg";
                        var destPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, fileName);

                        using (var input = ContentResolver.OpenInputStream(uri))
                        using (var output = File.Create(destPath))
                        {
                            input.CopyTo(output);
                        }

                        // Save per-widget filename
                        var prefs = GetSharedPreferences("WidgetPrefs", FileCreationMode.Private);
                        var editor = prefs.Edit();
                        editor.PutString($"img_{appWidgetId}", fileName);
                        editor.Apply();

                        // Update this widget
                        var appWidgetManager = AppWidgetManager.GetInstance(this);
                        image_widget_class.UpdateAppWidget(this, appWidgetManager, appWidgetId);

                        // Return success
                        Intent resultValue = new();
                        resultValue.PutExtra(AppWidgetManager.ExtraAppwidgetId, appWidgetId);
                        SetResult(Result.Ok, resultValue);
                    }
                    catch (System.Exception ex)
                    {
                        Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Long).Show();
                        SetResult(Result.Canceled);
                    }
                }
            }

            Finish();
        }
    }
}
