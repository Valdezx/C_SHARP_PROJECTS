using System.ComponentModel.Design;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
    

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}

