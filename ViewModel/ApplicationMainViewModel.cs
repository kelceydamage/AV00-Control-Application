﻿using AV00_Shared.Logging;
using AV00_Shared.Messages;
using NetMQ;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Transport.Client;

namespace AV00_Control_Application.ViewModel
{
    public partial class ApplicationMainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<LogMessage> FilteredEventStream { get => filteredEventStream; }
        private readonly ObservableCollection<LogMessage> filteredEventStream;
        public EnumLogMessageType[] EnumLogMessageTypePicker { get => Enum.GetValues<EnumLogMessageType>(); }
        public ITransportClient TransportClient => transportClient;
        private readonly ITransportClient transportClient;
        private readonly Task continualDatabaseUpdateTask;

        public ApplicationMainViewModel(ITransportClient DITransportClient)
        {
            transportClient = DITransportClient;
            TransportClient.RegisterServiceEventCallback("LogService", OnMessageReceiveCallback);
            LogMessage dummyData = new(Guid.NewGuid(), "TestService", EnumLogMessageType.Issuing, "This is a test message");
            filteredEventStream = new() { dummyData };
            continualDatabaseUpdateTask = ContinualDatabaseUpdateAsync();
        }

        public async Task OnLogTypeViewSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            Trace.WriteLine($"Selection changed {e.CurrentSelection}");
            await Task.Run(() =>
            {
                List<LogMessage> TempFiltered;
                TempFiltered = FilteredEventStream.Where(logMessage => Contains(logMessage, e.CurrentSelection)).ToList();
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
            });
        }

        private async Task ContinualDatabaseUpdateAsync()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (FilteredEventStream.Count < 200)
                    {
                        LogMessage dummyData = new(Guid.NewGuid(), "TestService", EnumLogMessageType.Issuing, "This is a test message");
                        FilteredEventStream.Add(dummyData);
                    }
                    TransportClient.ProcessPendingEventsAsync();
                    Thread.Sleep(1000);
                }
            });
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
            return SelectedFilters.Contains(Message.LogType);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
