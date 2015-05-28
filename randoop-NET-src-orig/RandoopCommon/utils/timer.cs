//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************


using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Common
{

    public interface ITimer
    {
        bool TimeToStop();
    }

    public class Timer : ITimer
    {
        /// <summary>
        /// Performance timer frequency.
        /// </summary>
        private static long fperfTimerFrequency;

        public static long PerfTimerFrequency
        {
            get
            {
                return fperfTimerFrequency;
            }
        }

        private long startTime;
        private double timeLimit;
        public bool startedCountDown = false;

        /// <summary>
        /// Creates a timer that will count down from the given number of seconds.
        /// </summary>
        /// <param name="TimeToCountDownFrom">Time to count down from, in seconds.</param>
        public Timer(double timeToCountDownFrom)
        {
            this.timeLimit = timeToCountDownFrom;
            Timer.QueryPerformanceCounter(ref this.startTime);
            this.startedCountDown = true;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Common.RandoopBug.#ctor(System.String)")]
        public bool TimeToStop()
        {
            if (!startedCountDown)
                throw new RandoopBug("countdown hasn't started.");

            long currentTime = 0;
            Timer.QueryPerformanceCounter(ref currentTime);
            double timeElapsed = ((double)(currentTime - startTime) / (double)(Timer.fperfTimerFrequency));

            if (timeElapsed >= timeLimit)
                return true;

            return false;
        }


        static Timer()
        {
            if (Timer.QueryPerformanceFrequency(ref fperfTimerFrequency) == 0)
                throw new RandoopBug("Error while querying external performance frequency.");
        }

        [DllImport("Kernel32.dll")]
        public extern static int QueryPerformanceCounter(ref long ticks);

        [DllImport("kernel32.dll")]
        public extern static int QueryPerformanceFrequency(ref long freq);
    }
}
