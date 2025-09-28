namespace MauiWidgets
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
#if ANDROID
            // Load username
            UsernameEntry.Text = UserPreferences.Username;

            // Load saved image if exists
            LoadSavedImage();
#endif
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
#if ANDROID
             UserPreferences.Username = UsernameEntry.Text;
#endif
        }

#if ANDROID
        private void LoadSavedImage()
        {
            var fileName = UserPreferences.ImagePath;
            if (!string.IsNullOrEmpty(fileName))
            {
                var localPath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                if (File.Exists(localPath))
                {
                    UserImage.Source = ImageSource.FromFile(localPath);
                }
            }
        }
#endif

        private async void OnPickImageClicked(object sender, EventArgs e)
        {
#if ANDROID
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    // Let UserPreferences handle copying and saving of the image
                    UserPreferences.ImagePath = result.FullPath;

                    // Update UI immediately
                    UserImage.Source = ImageSource.FromFile(result.FullPath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Image pick failed: {ex.Message}", "OK");
            }
#endif
        }
    }
}
