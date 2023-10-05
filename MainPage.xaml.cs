using AV00_Shared.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using AV00_Shared.Messages;

namespace AV00_Control_Application
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        public List<LogMessage> EventStream { get => eventStream; }
        private readonly List<LogMessage> eventStream;
        public ObservableCollection<LogMessage> FilteredEventStream { get => filteredEventStream; }
        private readonly ObservableCollection<LogMessage> filteredEventStream;
        public EnumLogMessageType[] EnumLogMessageTypePicker { get => Enum.GetValues<EnumLogMessageType>(); }

        public MainPage()
        {
            LogMessage dummyData = new(Guid.NewGuid(), "TestService", EnumLogMessageType.Issuing, "This is a test message");
            eventStream = new() { dummyData };
            for (var i = 0; i < 20; i++)
            {
                dummyData = new(Guid.NewGuid(), "TestService", EnumLogMessageType.Issuing, "This is a test message");
                eventStream.Add(dummyData);
            }
            filteredEventStream = new(EventStream);
            InitializeComponent();
            FilteredEventsView.ItemsSource = FilteredEventStream;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void OnLogTypeViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Trace.WriteLine($"Selection changed {e.CurrentSelection}");
            List<LogMessage> TempFiltered;
            TempFiltered = EventStream.Where(logMessage => Contains(logMessage, LogTypeView.SelectedItems)).ToList();
            for (int i = FilteredEventStream.Count - 1; i >= 0; i--)
            {
                var item = FilteredEventStream[i];
                if (!TempFiltered.Contains(item))
                {
                    FilteredEventStream.Remove(item);
                }
            }
            foreach (var item in TempFiltered)
            {
                if (!FilteredEventStream.Contains(item))
                {
                    Trace.WriteLine($"Adding {item}, count={FilteredEventStream.Count}");
                    FilteredEventStream.Add(item);
                }
            }
        }

        private void OnFilterByLogTypeTextChanged(object sender, TextChangedEventArgs e)
        {
            Trace.WriteLine($"Text changed {e.NewTextValue}");
            List<LogMessage> TempFiltered;
        }

        public bool Contains(LogMessage Message, IList<object> SelectedFilters)
        {
            return SelectedFilters.Contains(Message.LogType);
        }
    }
}