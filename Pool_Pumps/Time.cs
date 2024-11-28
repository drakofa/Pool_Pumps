    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.ComponentModel;

    namespace Pool_Pumps
    {
        public class Time : INotifyPropertyChanged
        {
            private int timeSpeed = 1;

            /// <summary>
            /// Скорость времени (привязывается к слайдеру)
            /// </summary>
            public int TimeSpeed
            {
                get => timeSpeed;
                set
                {
                    if (timeSpeed != value)
                    {
                    timeSpeed = value < 1 ? 1 : value;
                    OnPropertyChanged(nameof(TimeSpeed));
                }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
