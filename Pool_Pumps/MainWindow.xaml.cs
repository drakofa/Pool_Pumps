using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.ComponentModel;
using System.Threading;


namespace Pool_Pumps
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly Dictionary<string, bool> pumpFlags = new Dictionary<string, bool>();

        private Time globalTime;

        private Tap tap;

        private bool pump_socket_flag = false;

        // Словарь для хранения насосов
        private readonly Dictionary<string, Pump> pumps = new Dictionary<string, Pump>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            poolControl.mainwindow = this;

            Binding waterlvl = new Binding("WaterLevel")
            {
                Source = poolControl,
                Mode = BindingMode.OneWay
            };

            poolControl.Water.SetBinding(ProgressBar.ValueProperty, waterlvl);
            


            globalTime = new Time(); // Создаем общий объект Time
            //кран для добавления воды в бассейн.
            tap = new Tap(poolControl, this, globalTime);
            tap.Start();

            //Экземпляры pump1–pump5 создаются для управления пятью насосами.
            // Инициализация насосов и добавление их в словарь
            // иниц. флагов
            for (int i = 1; i <= 5; i++)
            {
                string pumpName = $"Pump{i}";
                pumps[pumpName] = new Pump(poolControl, globalTime);
                pumpFlags[pumpName] = true; 
            }


            BindTimeSpeedToSlider(globalTime, timeSlider);
            //worker1 = new BackgroundWorker();
            //worker2 = new BackgroundWorker();
            //worker3 = new BackgroundWorker();
            //worker4 = new BackgroundWorker();
            //worker5 = new BackgroundWorker();
            //не используется, T.К. использую Task.Run ))))))


        }
        private void poolControl_Loaded(object sender, RoutedEventArgs e)
        {
            poolControl.mainwindow = this;
            
        }

        public void updateLableWather()
        {
            watherlable.Content = poolControl.WaterLevel;
        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tap.TapSpeed = (int)waterSlider.Value;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && pumps.TryGetValue(checkBox.Name, out var pump))
            {
                RunInNewThread(pump.Start);
            }
        }
        private void CheckBox_UnChecked(object sender, RoutedEventArgs e) 
        {
            if (pump_socket_flag && sender is CheckBox checkBox && pumps.TryGetValue(checkBox.Name, out var pump))
            {
                pump.Stop();
            }
        }

        /// <summary>
        /// запускает метод Start для каждого насоса в отдельном потоке.
        /// </summary>
        /// <param name="action"></param>
        private void RunInNewThread(Action action)
        {
            Task.Run(() => action());
        }

        public void RadioButton_activetion()
        {
            foreach (var pair in pumpFlags)
            {
                // Пытаемся найти CheckBox с именем ключа насоса
                CheckBox checkBox = (CheckBox)FindName(pair.Key);
                if (checkBox != null)
                {
                    // Устанавливаем состояние CheckBox в соответствии с флагом
                    checkBox.IsChecked = pair.Value;
                }
            }
        }

        public void RadioButton_disactivetion()
        {
            foreach (var pair in pumpFlags)
            {
           
                CheckBox checkBox = (CheckBox)FindName(pair.Key);
                if (checkBox != null)
                {
                    
                    checkBox.IsChecked = false;
                    if (pumps.TryGetValue(pair.Key, out var pump))
                    {
                        pump.Stop(); 
                    }
                }
            }
        }

        private void pump_socket(object sender, RoutedEventArgs e)
        {
            
            if (sender is Button button)
            {
                string pumpName = button.Tag?.ToString();
                if (!string.IsNullOrEmpty(pumpName) && pumpFlags.ContainsKey(pumpName))
                {
                    
                    pumpFlags[pumpName] = !pumpFlags[pumpName];

                    
                    

                    
                    CheckBox checkBox = (CheckBox)FindName(pumpName);
                    if (checkBox != null)
                    {
                        checkBox.IsEnabled = pumpFlags[pumpName]; 
                        if (!pumpFlags[pumpName])
                        {
                            checkBox.IsChecked = false; 
                            pumps[pumpName].Stop();     
                        }
                    }
                }
            }
        }


        private void BindTimeSpeedToSlider(Time timeObject, Slider slider)
        {
            var binding = new Binding("TimeSpeed")
            {
                Source = timeObject,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            BindingOperations.SetBinding(slider, Slider.ValueProperty, binding);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string pumpName && pumpFlags.ContainsKey(pumpName))
            {
                // Переключаем флаг насоса
                pumpFlags[pumpName] = !pumpFlags[pumpName];

                // Обновляем статус чекбокса
                CheckBox checkBox = (CheckBox)FindName(pumpName);
                if (checkBox != null)
                {
                    checkBox.IsEnabled = pumpFlags[pumpName]; // Включаем/выключаем управление
                    if (!pumpFlags[pumpName])
                    {
                        checkBox.IsChecked = false; // Сбрасываем флажок, если насос отключён
                        pumps[pumpName].Stop();     // Останавливаем насос
                    }
                }

                //// Обновляем статус насоса
                //StatusLable.Content = pumpFlags[pumpName]
                //    ? $"СТАТУС : {pumpName} работает"
                //    : $"СТАТУС : {pumpName} не работает";
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}