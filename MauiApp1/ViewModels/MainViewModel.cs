using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MauiApp1.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        public string message = "Hello, World!";


    }
}
