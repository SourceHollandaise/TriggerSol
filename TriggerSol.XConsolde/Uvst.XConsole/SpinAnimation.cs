//
// SpinAnimation.cs
//
// Author:
//       Jörg Egger <joerg.egger@outlook.de>
//
// Copyright (c) 2015 Jörg Egger
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Para.Data.Client;
using TriggerSol.Boost;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using Uvst.Domain;
using Uvst.Model;

namespace Uvst.XConsole
{

    public static class SpinAnimation
    {

        //spinner background thread
        private static System.ComponentModel.BackgroundWorker spinner = initialiseBackgroundWorker();
        //starting position of spinner changes to current position on start
        private static int spinnerPosition = 25;
        //pause time in milliseconds between each character in the spin animation
        private static int spinWait = 150;
        //field and property to inform client if spinner is currently running
        private static bool isRunning;

        public static bool IsRunning { get { return isRunning; } }

        /// <summary>
        /// Worker thread factory
        /// </summary>
        /// <returns>background worker thread</returns>

        private static System.ComponentModel.BackgroundWorker initialiseBackgroundWorker()
        {

            System.ComponentModel.BackgroundWorker obj = new System.ComponentModel.BackgroundWorker();
            //allow cancellation to be able to stop the spinner
            obj.WorkerSupportsCancellation = true;
            //anonymous method for background thread's DoWork event
            obj.DoWork += delegate
            {
                //set the spinner position to the current console position
                spinnerPosition = Console.CursorLeft;
                //run animation unless a cancellation is pending
                while (!obj.CancellationPending)
                {
                    //characters to iterate through during animation
                    char[] spinChars = new char[] { '|', '/', '-', '\\' };
                    //iterate through animation character array
                    foreach (char spinChar in spinChars)
                    {
                        //reset the cursor position to the spinner position
                        Console.CursorLeft = spinnerPosition;
                        //write the current character to the console
                        Console.Write(spinChar);
                        //pause for smooth animation - set by the start method
                        System.Threading.Thread.Sleep(spinWait);
                    }
                }
            };
            return obj;
        }

        /// <summary>
        /// Start the animation
        /// </summary>
        /// <param name="spinWait">wait time between spin steps in milliseconds</param>
        public static void Start(int spinWait)
        {
            //Set the running flag
            isRunning = true;
            //process spinwait value
            SpinAnimation.spinWait = spinWait;
            //start the animation unless already started
            if (!spinner.IsBusy)
                spinner.RunWorkerAsync();
            else
                throw new InvalidOperationException("Cannot start spinner whilst spinner is already running");
        }

        /// <summary>
        /// Overloaded Start method with default wait value
        /// </summary>
        public static void Start()
        {
            Start(150);
        }

        /// <summary>
        /// Stop the spin animation
        /// </summary>

        public static void Stop()
        {
            //Stop the animation
            spinner.CancelAsync();
            //wait for cancellation to complete
            while (spinner.IsBusy)
                System.Threading.Thread.Sleep(100);
            //reset the cursor position
            Console.CursorLeft = spinnerPosition;
            //set the running flag
            isRunning = false;
            Console.Write("");
        }
    }

}
