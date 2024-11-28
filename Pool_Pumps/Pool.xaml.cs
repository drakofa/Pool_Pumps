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
using System.ComponentModel;

namespace Pool_Pumps
{
    /// <summary>
    /// Логика взаимодействия для Pool.xaml
    /// </summary>
    public partial class Pool : UserControl ,INotifyPropertyChanged
    {
        private double MaxWaterLevel = 2000; // максимальная высота
        private double waterLevel = 0; //уровеь вады
        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindow mainwindow;

        /// <summary>
        /// пустой констурктор
        /// </summary>
        public Pool()
        {
            InitializeComponent();
        }
        //public Pool(MainWindow mainWindow)
        //{
        //    InitializeComponent();
        //    mainwindow = mainWindow;
        //}
        public MainWindow MainWindow { get; set; }
        public static readonly DependencyProperty WaterLevelProperty =
        DependencyProperty.Register(
         "WaterLevel",
         typeof(double),
         typeof(Pool),
         new PropertyMetadata(0.0, OnWaterLevelChanged));
        private static void OnWaterLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pool = d as Pool;
            pool?.UpdateProgressBar();
        }

        public double WaterLevel
        {
            get { return (double)GetValue(WaterLevelProperty); }
            set { SetValue(WaterLevelProperty, value);
                OnPropertyChanged(nameof(WaterLevel));
                CheckPumpsActivation(); // Вызываем проверку после изменения уровня воды
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Включает насосы, если уровень воды достигает 75%
        /// </summary>
        private void CheckPumpsActivation()
        {
            if (WaterLevel >= 75)
            {
                // Активируем насосы, если уровень воды достигает 75%
                mainwindow.RadioButton_activetion();
            }
            else if (WaterLevel == 0)
            {
                // Деактивируем насосы, если уровень воды равен 0
                mainwindow.RadioButton_disactivetion();
            }
        }

        /// <summary>
        /// изменяем уровеь воды
        /// </summary>
        private void UpdateProgressBar()
        {
            Water.Value = WaterLevel; // Обновляем значение ProgressBar
            mainwindow.updateLableWather();
        }


        public void AddWater(double amount)
        {
            WaterLevel = Math.Min(WaterLevel + amount, 100);
        }

        public void RemoveWater(double amount)
        {
            WaterLevel = Math.Max(WaterLevel - amount, 0);
        }
    }
}
