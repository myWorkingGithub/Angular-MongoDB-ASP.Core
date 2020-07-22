using System;
using System.Threading;

namespace AngularMongoASP.SignalR.TimerFeatures
{
    public class TimerManager
    {
        private Timer _timer;
        private AutoResetEvent _autoResetEvent;
        private Action _action;
 
        public DateTime TimerStarted { get; }
 
        public TimerManager(System.Action action)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, 2000, 3000);
            TimerStarted = System.DateTime.Now;
        }
    
        public void Execute(object stateInfo)
        {
            _action();
    
            if((System.DateTime.Now - TimerStarted).Seconds > 60)
            {
                _timer.Dispose();
            }
        }
    }
}