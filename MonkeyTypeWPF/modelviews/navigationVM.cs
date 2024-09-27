using MonkeyTypeWPF.utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MonkeyTypeWPF.modelviews
{
    public class navigationVM : utilities.MainModelView
    {
        // текущий вид окна
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        // команды для перехода между юзер контролами
        public ICommand AccountCommand { get; set; }
        public ICommand TypingCommand { get; set; }
        // команды для управления окном
        public ICommand CloseWindowCommand { get; set; }
        public ICommand CollapseWindowCommand { get; set; }
        // методы команд

        private void Account(object obj) => CurrentView = new accountVM();
        private void Typing(object obj) => CurrentView = new typingVM(this);
        private void CloseWindow(object obj) => Process.GetCurrentProcess().Kill();
        private void CollapseWindow(object obj)
        {
            if (obj is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }
        public navigationVM()
        {
            // привязываем команды
            AccountCommand = new RelayCommand(Account);
            TypingCommand = new RelayCommand(Typing);
            CloseWindowCommand = new RelayCommand(CloseWindow);
            CollapseWindowCommand = new RelayCommand(CollapseWindow);
            // стартовое окно
            _currentView = new typingVM(this);
        }
    }
}
