﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpStudy
{
    public class SampleEventArgs
    {
        public SampleEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; } // readonly
    }

    partial class Publisher
    {
        // Declare the delegate (if using non-generic pattern).
        public delegate void SampleEventHandler(object sender, SampleEventArgs e);

        // Declare the event.
        public event SampleEventHandler SampleEvent;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void RaiseSampleEvent()
        {
            // Raise the event in a thread-safe manner using the?. operator.
            SampleEvent?.Invoke(this, new SampleEventArgs("Hello"));
        }
    }
}
