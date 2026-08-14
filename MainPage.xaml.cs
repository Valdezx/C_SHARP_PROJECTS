using System;

namespace taekwondo_app
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ViewFormsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Forms());
        }

        private async void ViewSparringButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Sparring());
        }

        
    }
}

