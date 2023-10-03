using System.Collections.ObjectModel;
using System.ComponentModel;
using Transport.Messages;

namespace AV00_Control_Application.ViewModels
{
    public class EventDataViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Event<LogMessage>> EventStream { get => eventStream; }
        private readonly ObservableCollection<Event<LogMessage>> eventStream;

        public EventDataViewModel()
        {
            LogMessage dummyData = new(Guid.NewGuid(), "TestService", "Issued", "This is a test message");
            Event<LogMessage> dummy = new("TestService", dummyData, EnumEventType.EventLog, dummyData.Id);
            eventStream = new()
            {
                dummy
            };
        }
    }
}
