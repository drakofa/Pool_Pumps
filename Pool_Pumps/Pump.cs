using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Pool_Pumps
{
    /// <summary>
    /// класс для нассоса
    /// </summary>
    class Pump
    {
        private Thread pumpThread;    // Поток, который выполняет работу насоса
        private bool isRunning;       // Флаг для остановки потока
        private int pumpSpeed;        // Скорость откачки в литрах в минуту

        public Pump(int speed)
        {
            pumpSpeed = speed;
            isRunning = false;
        }

        /// <summary>
        /// начала потока
        /// </summary>
        public void Start() 
        {
            if (!isRunning)
            {
                isRunning = true;
                pumpThread = new Thread(DoWork); 
                pumpThread.Start();              
            }
        }
        /// <summary>
        /// конец потока
        /// </summary>
        public void Stop()
        {
            if (pumpThread != null && pumpThread.IsAlive)
            {
                isRunning = false;
                
                pumpThread.Join();
            }
        }
        /// <summary>
        /// что делает поток
        /// </summary>
        private void DoWork()
        {
            while (isRunning)
            {
                Thread.Sleep(1000);
            }
        }

    }

}
