using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Pool_Pumps
{
    class Tap
    {
        public event Action<int> WaterFlowed; // обьявляем событие , уведомляем о палаче воды 

        private int tapSpeed;
        private bool isRunning;     // Флаг состояния
        private Thread tapThread;
        private Pool poolControl;   // Ссылка на объект Pool для добавления воды

        /// <summary>
        /// Свойство для управления скоростью
        /// </summary>
        public int TapSpeed
        {
            get => tapSpeed;
            set => tapSpeed = value;
        }

        public Tap(Pool pool)
        {
            isRunning = false;
            tapSpeed = 0;
            poolControl = pool; // Инициализация объекта Pool
        }


        public void Start() 
        {
            if (!isRunning)
            {
                isRunning = true;
                tapThread = new Thread(DoWork) { IsBackground = true };
                tapThread.Start();
            }
        }
        public void Stop()
        {
            if (tapThread != null && tapThread.IsAlive)
            {
                isRunning = false;

                tapThread.Join();
            }
        }
        private void DoWork()
        {
            while (isRunning)
            {
                poolControl.Dispatcher.Invoke(() =>
                {
                    poolControl.AddWater(tapSpeed * 0.1);
                });
                Thread.Sleep(100);
            }
        }

    }
}
