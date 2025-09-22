namespace MauiWidgets
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
#if ANDROID
            UsernameEntry.Text = UserPreferences.Username;
#endif
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
#if ANDROID
             UserPreferences.Username = UsernameEntry.Text;
#endif
        }
    }
}
