﻿using System.Collections.ObjectModel;
using AV00_Shared.Logging;
using Transport.Messages;

namespace AV00_Control_Application.ViewModels
{
    public class EventDataViewModel
    {
        public ObservableCollection<LogMessage> EventStream { get => eventStream; }
        private readonly ObservableCollection<LogMessage> eventStream;

        public EnumLogMessageType[] EnumLogMessageTypePicker { get => Enum.GetValues<EnumLogMessageType>(); }

        public EventDataViewModel()
        {
            LogMessage dummyData = new(Guid.NewGuid(), "TestService", EnumLogMessageType.Issuing, "This is a test message");
            eventStream = new() { dummyData };
            for(var i = 0; i < 20; i++)
            {
                eventStream.Add(dummyData);
            }
        }

        public bool Contains(LogMessage Message)
        {
            //LogMessage? message = Message as LogMessage?;
            return(Message.LogType==EnumLogMessageType.Info);
        }
    } 
}
