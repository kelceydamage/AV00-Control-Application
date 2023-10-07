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

        //TODO: This feels bad, firing many of these could lead to an unpredictable view of the data.
        private void OnLogTypeViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Trace.WriteLine($"MainPage OnLogTypeViewSelectionChanged");
            var _ = viewModel.OnLogTypeViewSelectionChangedAsync(sender, e);
        }
    }
}
