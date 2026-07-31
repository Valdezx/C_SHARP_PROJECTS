
using Microsoft.Maui.Controls;
using System;

namespace Buch_Project
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void ExitButton_Clicked(object sender, EventArgs e)
        {
            // Відображаємо діалогове вікно
            bool answer = await DisplayAlert("Вихід", "Ви дійсно хочете вийти з програми?", "Так", "Ні");

            if (answer)
            {
                Application.Current?.CloseWindow(Application.Current.MainPage.Window);
            }

        }


        private void LoginButton_Clicked(object sender, EventArgs e)
        {

        }

        private void RegisterButton_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}
