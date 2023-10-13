using AV00_Control_Application.ViewModels;
using System.Collections.Specialized;
using System.ComponentModel;
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

        private void OnLogTypeViewSelectionChanged(object Sender, SelectionChangedEventArgs EventArgs)
        {
            viewModel.OnLogTypeViewSelectionChanged(Sender, EventArgs);
        }

        private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
            {
                return;
            }

            //NOTE: workaround on windows to fire collectionview itemthresholdreached command, because it does not work on windows
            if (sender is CollectionView cv && cv is IElementController element)
            {
                var count = element.LogicalChildren.Count;
                if (e.LastVisibleItemIndex + 1 - count + cv.RemainingItemsThreshold >= 0)
                {
                    // Implement new dataloading for incremental loading
                }
            }
        }
    }
}
