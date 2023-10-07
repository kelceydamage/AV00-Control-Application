using AV00_Shared.Logging;
using AV00_Shared.Models;
using Microsoft.Maui.Layouts;
using NetMQ;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Transport.Client;

namespace AV00_Control_Application.ViewModels
{
    public partial class ApplicationMainViewModel : INotifyPropertyChanged
    {
        public List<LogMessage> FakeDB { get => fakeDB; }
        private readonly List<LogMessage> fakeDB;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<LogMessage> FilteredEventStream { get => filteredEventStream; }
        private readonly ObservableCollection<LogMessage> filteredEventStream;
        public EnumLogMessageType[] EnumLogMessageTypePicker { get => Enum.GetValues<EnumLogMessageType>(); }
        public ITransportClient TransportClient => transportClient;
        private readonly ITransportClient transportClient;
        private readonly Task continualDatabaseUpdateTask;
        private IReadOnlyList<object> currentFilter;

        public ApplicationMainViewModel(ITransportClient DITransportClient)
        {
            transportClient = DITransportClient;
            TransportClient.RegisterServiceEventCallback("LogService", OnMessageReceiveCallback);
            LogMessage dummyData = new(Guid.NewGuid(), "TestService", EnumLogMessageType.Issuing, "This is a test message");
            fakeDB = new List<LogMessage>() { dummyData };
            filteredEventStream = new(fakeDB);
            continualDatabaseUpdateTask = StartDatabaseUpdateThread();
        }

        public async Task OnLogTypeViewSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            await Task.Run(() => UpdateFilteredEventStream(e));
        }

        private Task StartDatabaseUpdateThread()
        {
            return Task.Run(() => ContinualDatabaseUpdateAsync());
        }

        private async Task ContinualDatabaseUpdateAsync()
        {
            while (true)
            {
                if (FakeDB.Count < 200)
                {
                    LogMessage dummyData = new(Guid.NewGuid(), "TestService", EnumLogMessageType.Issuing, "This is a test message");
                    fakeDB.Add(dummyData);
                    if (Contains(dummyData, currentFilter))
                    {
                        FilteredEventStream.Add(dummyData);
                    }
                }
                await TransportClient.ProcessPendingEventsAsync();
                Thread.Sleep(1000);
            }
        }

        private void UpdateFilteredEventStream(SelectionChangedEventArgs EventArgs)
        {
            currentFilter = EventArgs.CurrentSelection;
            Trace.WriteLine($"Selection changed {EventArgs.CurrentSelection}");
            List<LogMessage> TempFiltered;
            TempFiltered = fakeDB.AsParallel().Where(logMessage => Contains(logMessage, EventArgs.CurrentSelection)).ToList();
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

        private static bool OnMessageReceiveCallback(NetMQMessage MQMessage)
        {
            Trace.WriteLine($"New message from queue, putting in DB");
            return true;
        }

        private static void OnFilterByLogTypeTextChanged(object sender, TextChangedEventArgs e)
        {
            Trace.WriteLine($"Text changed {e.NewTextValue}");
        }

        private static bool Contains(LogMessage Message, IReadOnlyList<object> SelectedFilters)
        {
            if ( SelectedFilters == null )
            {
                return false;
            }
            return SelectedFilters.Contains(Message.LogType);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
