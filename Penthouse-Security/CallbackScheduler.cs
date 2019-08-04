﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    public class CallbackScheduler
    {
        event Action callback;
        int interval;

        public CallbackScheduler(int interval, Action callback)
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