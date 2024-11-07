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

namespace Pool_Pumps
{
    /// <summary>
    /// Логика взаимодействия для Pool.xaml
    /// </summary>
    public partial class Pool : UserControl
    {
        private double MaxWaterLevel = 200; // максимальная высота
        private double waterLevel = 0; //уровеь вады


        public Pool()
        {
            InitializeComponent();
        }

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
            set { SetValue(WaterLevelProperty, value); }
        }

        /// <summary>
        /// изменяем уровеь воды
        /// </summary>
        private void UpdateProgressBar()
        {
            Water.Value = WaterLevel; // Обновляем значение ProgressBar
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
