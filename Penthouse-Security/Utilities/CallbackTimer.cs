using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    public class CallbackTimer
    {
        private event Action callback;
        private int interval;

        public CallbackTimer(int interval, Action callback)
        {
            this.interval = interval;
            this.callback = callback;
            StartAsync();
        }

        async void StartAsync()
        {
            while (true)
            {
                await Task.Delay(interval);
                callback?.Invoke();
            }
        }
    }
}
