using AV00_Control_Application.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;

namespace AV00_Control_Application.Views
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

        //TODO: This feels bad... Investigating.
        private void OnLogTypeViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _ = viewModel.OnLogTypeViewSelectionChangedAsync(sender, e);
        }
    }
}
