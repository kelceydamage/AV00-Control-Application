﻿using AV00_Control_Application.Models;
using AV00_Shared.Logging;
using AV00_Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Layouts;
using NetMQ;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Transport.Client;
using Transport.Event;

namespace AV00_Control_Application.ViewModels
{
    using LogEvent = Event<LogEventModel>;

    public partial class ApplicationMainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<LogEventModel> FilteredEventStream { get; set; }
        public LogEventModel LatestLogMessage { get; set; }
        public EnumLogMessageType[] EnumLogMessageTypePicker { get => Enum.GetValues<EnumLogMessageType>(); }
        public ITransportClient TransportClient => transportClient;
        private readonly ITransportClient transportClient;
        private readonly Task continualDatabaseUpdateTask;
        private IReadOnlyList<object> currentFilter = new List<object>();
        private ApplicationDbContext databaseContext;

        public ApplicationMainViewModel(ITransportClient DITransportClient, ApplicationDbContext DatabaseContext)
        {
            databaseContext = DatabaseContext;
            transportClient = DITransportClient;
            TransportClient.RegisterServiceEventCallback("LogService", OnMessageReceiveCallback);
            LogEventModel dummyData = new("TestService", EnumLogMessageType.Issuing, "This is a test message");
            FilteredEventStream = new() { dummyData };
            LatestLogMessage = FilteredEventStream.Last();
            continualDatabaseUpdateTask = StartDatabaseUpdateThread();
        }
        
        public async Task OnLogTypeViewSelectionChangedAsync(object Sender, SelectionChangedEventArgs EventArgs)
        {
            await Task.Run(() => UpdateFilteredEventStream(EventArgs));
        }

        public void OnLogTypeViewSelectionChanged(object Sender, SelectionChangedEventArgs EventArgs)
        {
            UpdateFilteredEventStream(EventArgs);
        }

        private Task StartDatabaseUpdateThread()
        {
            return Task.Run(() => ContinualDatabaseUpdateAsync());
        }

        private async Task ContinualDatabaseUpdateAsync()
        {
            int i = 0;
            while (true)
            {
                if (i < 200)
                {
                    LogEventModel dummyData = new("TestService", EnumLogMessageType.Issuing, "This is a test message");
                    databaseContext.Add(dummyData);
                    databaseContext.SaveChanges();
                    if (currentFilter.Contains(dummyData.LogType))
                    {
                        Trace.WriteLine($"Adding new message");
                        FilteredEventStream.Add(dummyData);
                        LatestLogMessage = dummyData;
                    }
                }
                await TransportClient.ProcessPendingEventsAsync();
                i++;
                Thread.Sleep(1000);
            }
        }

        private void UpdateFilteredEventStream(SelectionChangedEventArgs EventArgs)
        {
            currentFilter = EventArgs.CurrentSelection;
            Trace.WriteLine($"Selection changed {EventArgs.CurrentSelection}");
            IQueryable<LogEventModel> query = from message in databaseContext.LogMessages
                                  where currentFilter.Contains(message.LogType)
                                  select message;
            // Remove once satisfied with debugging.
            Trace.WriteLine($"Query Res Count: {query.Count()}");

            FilteredEventStream = new(query.Take(100));
            LatestLogMessage = FilteredEventStream.Last();
            OnPropertyChanged(nameof(FilteredEventStream));
        }

        private bool OnMessageReceiveCallback(NetMQMessage MQMessage)
        {
            Trace.WriteLine($"New message from queue, putting in DB");
            databaseContext.Add(new LogEvent(MQMessage).Model);
            databaseContext.SaveChanges();
            return true;
        }

        private static void OnFilterByLogTypeTextChanged(object sender, TextChangedEventArgs e)
        {
            Trace.WriteLine($"Text changed {e.NewTextValue}");
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
