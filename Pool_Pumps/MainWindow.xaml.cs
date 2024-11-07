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


namespace Pool_Pumps
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Tap tap;

        
        public MainWindow()
        {
            InitializeComponent();

            Binding waterlvl = new Binding("WaterLevel")
            {
                Source = poolControl,
                Mode = BindingMode.OneWay
            };

            poolControl.Water.SetBinding(ProgressBar.ValueProperty, waterlvl);


            tap = new Tap(poolControl);
        }

        private void poolControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddWater_Click(object sender, RoutedEventArgs e)
        {
            //poolControl.addWater(tap.TapSpeed); // Добавить 10% воды
            tap.Start();
        }

        private void RemoveWater_Click(object sender, RoutedEventArgs e)
        {
            //poolControl.RemoveWater(tap.TapSpeed); // Убрать 10% воды
            tap.Stop();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            
            tap.TapSpeed = (int)waterSlider.Value;
        }
    }
}
