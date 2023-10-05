using AV00_Shared.Logging;
using System.Collections.ObjectModel;
using Transport.Messages;

namespace AV00_Control_Application
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        public ObservableCollection<LogMessage> EventStream { get => eventStream; }
        private readonly ObservableCollection<LogMessage> eventStream;
        public EnumLogMessageType[] EnumLogMessageTypePicker { get => Enum.GetValues<EnumLogMessageType>(); }

        public MainPage()
        {
            InitializeComponent();

            LogMessage dummyData = new(Guid.NewGuid(), "TestService", EnumLogMessageType.Issuing, "This is a test message");
            eventStream = new() { dummyData };
            for (var i = 0; i < 20; i++)
            {
                eventStream.Add(dummyData);
            }
            ObservableCollection<LogMessage> FilteredEventStream = new ObservableCollection<LogMessage>(eventStream);
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
            Console.WriteLine($"Selection changed {e.CurrentSelection}");
        }

        public bool Contains(LogMessage Message)
        {
            //LogMessage? message = Message as LogMessage?;
            return (Message.LogType == EnumLogMessageType.Info);
        }
    }
}