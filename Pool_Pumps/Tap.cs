using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
namespace Pool_Pumps

{
    class Tap : Time, INotifyPropertyChanged
    {
        public event Action<int> WaterFlowed;
        private Time time;
        private int tapSpeed;
        
        private bool isRunning;     // Флаг состояния
        private Thread tapThread;
        private Pool poolControl;   // Ссылка на объект Pool для добавления воды
        private MainWindow mainwindow;
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
        public Tap(Pool pool, MainWindow mainWindow)
        {
            isRunning = false;
            tapSpeed = 0;
            poolControl = pool; // Инициализация объекта Pool
            mainwindow = mainWindow;
        }

        public Tap(Pool pool, MainWindow mainWindow, Time globalTime)
        {
            isRunning = false;
            tapSpeed = 0;
            poolControl = pool; // Инициализация объекта Pool
            mainwindow = mainWindow;
            time = globalTime; // Ссылка на общий объект Time
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
                    poolControl.AddWater(tapSpeed * time.TimeSpeed);
                    
                });
                Thread.Sleep(600);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
