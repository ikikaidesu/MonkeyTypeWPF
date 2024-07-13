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
    /// Логика взаимодействия для UpperToolTip.xaml
    /// </summary>
    public partial class UpperToolTip : UserControl
    {
        public UpperToolTip()
        {
            InitializeComponent();
        }
        // создаем зависимость(передаваемая переменная) с текстом подсказки
        public string ToolTipText
        {
            get => (string)GetValue(ToolTipTextProperty);
            set => SetValue(ToolTipTextProperty, value);
        }
        public static readonly DependencyProperty ToolTipTextProperty =
            DependencyProperty.Register("ToolTipText", typeof(string), typeof(UpperToolTip), new PropertyMetadata(default));
    }
}
