using AV00_Control_Application.ViewModels;

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

        //TODO: Implement autoscroll behind a toggle. Periodically scroll to the bottom if scroll-cursor not at bottom.
    }
}
