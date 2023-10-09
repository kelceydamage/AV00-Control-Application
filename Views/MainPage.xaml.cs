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
        private void OnLogTypeViewSelectionChanged(object Sender, SelectionChangedEventArgs EventArgs)
        {
            var _ = viewModel.OnLogTypeViewSelectionChangedAsync(Sender, EventArgs);
        }

        //TODO: Implement autoscroll behind a toggle. Periodically scroll to the bottom if scroll-cursor not at bottom.
        // Skip, Take when scroll to top. Always render only 100 latest and removes oldest.
        // Is it possible to keep skip/take pagination values in sync with live collection upto
        // point where the old query is no longer valid? (Ex: select->100, then 100 new live events pop in,
        // and we need a new select query as we can no longer scroll-back)
    }
}
