﻿using AV00_Control_Application.ViewModel;
using System.Diagnostics;

namespace AV00_Control_Application.View
{
    public partial class MainPage : ContentPage
    {

        private readonly ApplicationMainViewModel viewModel;
        public MainPage(ApplicationMainViewModel ViewModel)
        {
            viewModel = ViewModel;
            BindingContext = viewModel;
            InitializeComponent();
        }

        private void OnLogTypeViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Trace.WriteLine($"MainPage OnLogTypeViewSelectionChanged");
            viewModel.OnLogTypeViewSelectionChanged(sender, e);
        }
    }
}
