using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace MonkeyTypeWPF.usercontrols
{
    /// <summary>
    /// Логика взаимодействия для RecordsInfo.xaml
    /// </summary>
    public partial class RecordsInfo : UserControl
    {
        public RecordsInfo()
        {
            InitializeComponent();
        }
        // создаем зависимости(передаваемая переменная) с данными теста
        public string Mode
        {
            get => (string)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        public double WPM
        {
            get => (double)GetValue(WPMProperty);
            set => SetValue(WPMProperty, value);
        }
        public double RawWPM
        {
            get => (double)GetValue(RawWPMProperty);
            set => SetValue(RawWPMProperty, value);
        }
        public double Accurancy
        {
            get => (double)GetValue(AccurancyProperty);
            set => SetValue(AccurancyProperty, value);
        }
        public double Сonsistency
        {
            get => (double)GetValue(СonsistencyProperty);
            set => SetValue(СonsistencyProperty, value);
        }
        public DateTime Date
        {
            get => ((DateTime)GetValue(DateProperty));
            set => SetValue(DateProperty, value);
        }
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(string), typeof(RecordsInfo), new PropertyMetadata(default));
        public static readonly DependencyProperty WPMProperty =
            DependencyProperty.Register("WPM", typeof(double), typeof(RecordsInfo), new PropertyMetadata(default));
        public static readonly DependencyProperty RawWPMProperty =
    DependencyProperty.Register("RawWPM", typeof(double), typeof(RecordsInfo), new PropertyMetadata(default));
        public static readonly DependencyProperty AccurancyProperty =
            DependencyProperty.Register("Accurancy", typeof(double), typeof(RecordsInfo), new PropertyMetadata(default));
        public static readonly DependencyProperty СonsistencyProperty =
            DependencyProperty.Register("Сonsistency", typeof(double), typeof(RecordsInfo), new PropertyMetadata(default));
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(RecordsInfo), new PropertyMetadata(default));
    }
}
