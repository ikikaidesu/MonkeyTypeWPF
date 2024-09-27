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

namespace MonkeyTypeWPF.usercontrols
{
    /// <summary>
    /// Логика взаимодействия для ExportCSV.xaml
    /// </summary>
    public partial class ExportCSV : UserControl
    {
        public ExportCSV()
        {
            InitializeComponent();
        }
        // создаем зависимость(передаваемая переменная) с командой для обработки при нажатии
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public static readonly DependencyProperty CommandProperty =
    DependencyProperty.Register("Command", typeof(ICommand), typeof(ExportCSV), new PropertyMetadata(default));
    }
}
