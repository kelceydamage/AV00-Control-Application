using AV00_Shared.Logging;
using AV00_Shared.Messages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Transport.Client;

namespace AV00_Control_Application.ViewModel
{
    public partial class ApplicationMainViewModel
    {
        public List<LogMessage> EventStream { get => eventStream; }
        private readonly List<LogMessage> eventStream;
        public ObservableCollection<LogMessage> FilteredEventStream { get => filteredEventStream; }
        private readonly ObservableCollection<LogMessage> filteredEventStream;
        public EnumLogMessageType[] EnumLogMessageTypePicker { get => Enum.GetValues<EnumLogMessageType>(); }
        public ITransportClient TransportClient => transportClient;
        private readonly ITransportClient transportClient;

        public ApplicationMainViewModel(ITransportClient TransportClient)
        {
            transportClient = TransportClient;
            LogMessage dummyData = new(Guid.NewGuid(), "TestService", EnumLogMessageType.Issuing, "This is a test message");
            eventStream = new() { dummyData };
            for (var i = 0; i < 20; i++)
            {
                dummyData = new(Guid.NewGuid(), "TestService", EnumLogMessageType.Issuing, "This is a test message");
                eventStream.Add(dummyData);
            }
            filteredEventStream = new(EventStream);
        }

        public void OnLogTypeViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Trace.WriteLine($"Selection changed {e.CurrentSelection}");
            List<LogMessage> TempFiltered;
            TempFiltered = EventStream.Where(logMessage => Contains(logMessage, e.CurrentSelection)).ToList();
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
        }

        public bool Contains(LogMessage Message, IReadOnlyList<object> SelectedFilters)
        {
            return SelectedFilters.Contains(Message.LogType);
        }
    }
}
