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
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Security;

namespace Common
{

    /// <summary>
    /// Executes some code, catching and logging any exceptions
    /// or assertion violations.
    /// The executor turns off debug assertion pop-up windows, but
    /// does detect assertion violations and reports them.
    /// </summary>
    public class CodeExecutor
    {
        private CodeExecutor()
        {
            // Empty body.
        }

        private static bool usingRandoopDebugListener;

        private static DebugListener fdebugListener = null;

        private static Collection<TraceListener> originalListeners;

        private class DebugListener : TraceListener
        {
            StringBuilder messageSB = new StringBuilder();
            public bool failWasCalled;

            public string FailureMessage
            {
                get
                {
                    return messageSB.ToString();
                }
            }

            public bool FailWasCalled
            {
                get
                {
                    return failWasCalled;
                }
            }

            public void Reset()
            {
                failWasCalled = false;
                messageSB = new StringBuilder();
            }

            public DebugListener()
            {
                failWasCalled = false;
            }

            public override void Write(string message)
            {
                failWasCalled = true;
                messageSB.Append(message);
            }

            public override void WriteLine(string message)
            {
                failWasCalled = true;
                messageSB.AppendLine(message);
            }
        }

        /// <summary>
        /// Creates a new plan executor. Turns off System.Diagnostics.Debug.Listeners
        /// so assertion failures in the tested code don't interrrupt randoop. Enlists a
        /// Randoop.DebugListener listener so that such assertion failures can
        /// still be detected.
        /// </summary>
        static CodeExecutor()
        {
            // Remove all debug listeners--otherwise randoop may be interrupted.
            // Store them, in case the user wants to add them back.
            originalListeners = new Collection<TraceListener>();
            System.Diagnostics.Trace.Listeners.Clear();
            for (int i = 0; i < Debug.Listeners.Count; i++)
            {
                originalListeners.Add(Debug.Listeners[i]);
                Debug.Listeners.RemoveAt(i);
            }

            // Add our own debug listener instead.
            fdebugListener = new DebugListener();
            Debug.Listeners.Add(fdebugListener);

            usingRandoopDebugListener = true;
        }


        /// <summary>
        /// Wraps some code to be executed.
        /// </summary>
        public delegate void CodeToExecute();

        private static double SecondsSpentInCalls;

        /// <summary>
        /// Executes the code in the CodeToExecute delegate.
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Common.AssertionViolation.#ctor(System.String)")]
        public static bool ExecuteReflectionCall(CodeToExecute call, TextWriter writer, out Exception exceptionThrown)
        {
            long startTime = 0;
            Timer.QueryPerformanceCounter(ref startTime);

            if (usingRandoopDebugListener) fdebugListener.Reset();
            bool invocationOk = true;
            exceptionThrown = null;

            try
            {
                //new PermissionSet(null).PermitOnly();
                call();
                //CodeAccessPermission.RevertAll();
                //CodeAccessPermission.RevertPermitOnly();

            }
            catch (TargetInvocationException e)
            {
                exceptionThrown = e.InnerException;
                Util.Assert(exceptionThrown != null);
                writer.WriteLine("e : " + e);
                invocationOk = false;
                writer.WriteLine("Execution failed.");


            }
            catch (SecurityException e)
            {
                exceptionThrown = e;
                writer.WriteLine("e : " + e);
                invocationOk = false;
                writer.WriteLine("Execution failed.");

            }
            catch (Exception e)
            {
                exceptionThrown = e;
                writer.WriteLine("e : " + e);
                invocationOk = false;
                writer.WriteLine("execution failed.");
            }

            if (usingRandoopDebugListener && fdebugListener.FailWasCalled)
            {
                invocationOk = false;
                exceptionThrown = new AssertionViolation("An assertion was violated: "
                    + fdebugListener.FailureMessage);
            }

            long endTime = 0;
            Timer.QueryPerformanceCounter(ref endTime);

            try
            {
                ;
            }
            catch (SecurityException e)
            {
                exceptionThrown = e;
                writer.WriteLine("e : " + e);
                invocationOk = false;
                writer.WriteLine("Execution failed.");
            }
            SecondsSpentInCalls += ((double)(endTime - startTime)) / ((double)(Timer.PerfTimerFrequency));

            if (!invocationOk)
                return false;

            return true;
        }

    }
}
