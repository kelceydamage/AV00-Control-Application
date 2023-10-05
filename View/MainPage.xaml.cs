using AV00_Control_Application.ViewModel;

namespace AV00_Control_Application.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(ApplicationMainViewModel ViewModel)
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }

        private void LogTypeView_SelectionChanged()
        {

        }
    }
}