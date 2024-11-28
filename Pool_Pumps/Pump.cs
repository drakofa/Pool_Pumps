using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;


namespace Pool_Pumps
{
    /// <summary>
    /// класс для нассоса
    /// </summary>
    class Pump :Time
    {
        
        private Thread pumpThread;    // Поток, который выполняет работу насоса
        private bool isRunning;       // Флаг для остановки потока
        
        private int pumpTimeSpeed;    // Скорость откачки в литрах в минуту
        private Pool poolControl;   // Ссылка на объект Pool для добавления воды
        private Time time;

        public int PumpTimeSpeed
        {
            get => pumpTimeSpeed;
            set
            {
                if (pumpTimeSpeed != value)
                {
                    pumpTimeSpeed = value;
                    OnPropertyChanged(nameof(PumpTimeSpeed));
                }
            }
        }
        public Pump(Pool pool)
        {
            poolControl = pool;
            isRunning = false;
        }
        

        public Pump(Pool pool, Time globalTime)
        {
            poolControl = pool;
            isRunning = false;
            time = globalTime; // Ссылка на общий объект Time
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
            if (pumpThread != null && !pumpThread.IsAlive)
            {
                isRunning = false;
                
                pumpThread.Join();
            }
        }
        /// <summary>
        /// что делает поток
        /// </summary>
        private async void DoWork()
        {
            while (isRunning)
            {
                poolControl.Dispatcher.Invoke(() =>
                {
                    poolControl.RemoveWater(2 * time.TimeSpeed);
                });
                await Task.Delay(600);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
