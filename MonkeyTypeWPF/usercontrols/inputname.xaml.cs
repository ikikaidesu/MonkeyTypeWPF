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
    /// Логика взаимодействия для inputname.xaml
    /// </summary>
    public partial class inputname : UserControl
    {
        public inputname()
        {
            InitializeComponent();
        }

        // создаем зависимость(передаваемая переменная) с командой для обработки при нажатии
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        // создаем зависимость(передаваемая переменная) с текстом на фоне
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        // создаем зависимость(передаваемая переменная) с ником юзера
        public string NickName
        {
            get => (string)GetValue(NickNameProperty);
            set => SetValue(NickNameProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(inputname), new PropertyMetadata(default));
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(inputname), new PropertyMetadata(default));
        public static readonly DependencyProperty NickNameProperty =
            DependencyProperty.Register("NickName", typeof(string), typeof(inputname), new PropertyMetadata(default));
    }
}
