using System.ComponentModel.Design;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private int PasswordLength = 4;

        public MainPage()
        {
            InitializeComponent();
            Grid = new Grid();
            Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        private void PasswordStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            PasswordLength = (int)e.NewValue;
            PasswordLengthLabel.Text = "Password Length: " + PasswordLength;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string password = GeneratedPass(PasswordLength);
            GeneratedPasswordLabel.Text = password;

        }

        private string GeneratedPass(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^_+-=|;:,.?";
            Random random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}

