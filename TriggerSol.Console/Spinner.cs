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

namespace TriggerSol.Console
{
    public static class Spinner
    {
        static System.ComponentModel.BackgroundWorker _Spinner = InitializeBackgroundWorker();
        static int _Position = 25;
        static int _Velocity = 150;

        public static bool IsRunning { get; private set; }

        public static void Start(int velocity)
        {
            IsRunning = true;
            _Velocity = velocity;
            if (!_Spinner.IsBusy)
                _Spinner.RunWorkerAsync();
            else
                throw new InvalidOperationException("Cannot start spinner whilst spinner is already running!");
        }

        public static void Start() => Start(150);

        public static void Stop()
        {
            _Spinner.CancelAsync();

            while (_Spinner.IsBusy)
                System.Threading.Thread.Sleep(100);

           System.Console.CursorLeft = _Position;

            IsRunning = false;

            System.Console.Write(" ");
            System.Console.WriteLine();
        }

        static System.ComponentModel.BackgroundWorker InitializeBackgroundWorker()
        {
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker() { WorkerSupportsCancellation = true };

            worker.DoWork += delegate
            {
                _Position = System.Console.CursorLeft;

                while (!worker.CancellationPending)
                {
                    char[] spinChars = new char[] { '|', '/', '-', '\\' };

                    foreach (char spinChar in spinChars)
                    {
                        System.Console.CursorLeft = _Position;
                        System.Console.Write(spinChar);

                        System.Threading.Thread.Sleep(_Velocity);
                    }
                }
            };

            return worker;
        }
    }
}
