using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyTypeWPF.models;
using System.Windows.Input;
using MonkeyTypeWPF.utilities;
using MonkeyTypeWPF.usercontrols;
using System.Runtime.CompilerServices;
using System.IO;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using System.ComponentModel;
using OxyPlot.Annotations;

namespace MonkeyTypeWPF.modelviews
{
    // создаем прослойку VM между accountModel и accountView
    public class accountVM : utilities.MainModelView
    {
        // создаем экземпляр нашей модели
        private readonly AccountModel _accountModel;
        // создаем показывающийся никнейм человека
        public string DisplayNickName
        {
            get { return _accountModel.nickname; }
            set { _accountModel.nickname = value; OnPropertyChanged(); }
        }
        // создаем дату присоединения
        public string DisplayCreateAccount
        {
            get { return _accountModel.CreateAccount.ToString("d MMMM yyyy", new CultureInfo("en-EN")); }
            set { _accountModel.CreateAccount = DateTime.ParseExact(value, "d MMMM yyyy", new CultureInfo("en-EN")); OnPropertyChanged(); }
        }

        // создаем кол-во начатых тестов
        public int DisplayTestsStarted
        {
            get { return _accountModel.tests_started; }
        }
        // создаем кол-во завершенных тестов
        public int DisplayTestsCompleted
        {
            get { return _accountModel.tests_completed; }
        }
        // создаем процент завершенности тестов относительно начатых
        public double DisplayTestCompletedPerStarted
        {
            get { return  Math.Round((_accountModel.tests_completed / (double)_accountModel.tests_started) * 100,0); }
        }
        // создаем процент рестартов относительно завершенных
        public double DisplayRestartsPerCompletedTests
        {
            get { return Math.Round((DisplayTestsStarted - DisplayTestsCompleted) / (double)DisplayTestsCompleted, 1); }
        }
        // создаем максимальный wpm и тест в котором он установлен
        public CSVData.TestResult DisplayMaxTestWPM
        {
            get { return _accountModel.max_test_wpm; }
            set { _accountModel.max_test_wpm = value; OnPropertyChanged(); }
        }
        // создаем средний WPM
        public double DisplayAverageWPM
        {
            get { return _accountModel.avg_wpm; }
            set { _accountModel.avg_wpm = value; OnPropertyChanged(); }
        }
        // создаем средний WPM за последние 10 тестов
        public double DisplayAvgWPMLast10Tests
        {
            get { return _accountModel.avg_wpm_10_last_tests; }
            set { _accountModel.avg_wpm_10_last_tests = value; OnPropertyChanged(); }
        }
        // создаем максимальный сырой wpm
        public double DisplayHighestRawWPM
        {
            get { return _accountModel.max_raw_wpm; }
            set { _accountModel.max_raw_wpm = value; OnPropertyChanged(); }
        }
        // создаем средний сырой wpm
        public double DisplayAverageRawWPM
        {
            get { return _accountModel.avg_raw_wpm; }
            set { _accountModel.avg_raw_wpm = value; OnPropertyChanged(); }
        }
        // создаем средний сырой wpm за последние 10 тестов
        public double DisplayAvgRawWPM10Tests
        {
            get { return _accountModel.avg_raw_wpm_10_lasts_tests; }
            set { _accountModel.avg_raw_wpm_10_lasts_tests = value; OnPropertyChanged(); }
        }
        // создаем максимальную точность
        public double DisplayHighestAccuracy
        {
            get { return _accountModel.max_accuracy; }
            set { _accountModel.max_accuracy = value; OnPropertyChanged(); }
        }
        // создаем среднюю точность
        public double DisplayAverageAccuracy
        {
            get { return _accountModel.avg_accuracy; }
            set { _accountModel.avg_accuracy = value; OnPropertyChanged(); }
        }
        // создаем среднюю точность за последние 10 тестов
        public double DisplayAvgAcc10lastTests
        {
            get { return _accountModel.avg_accuracy_10_last_tests; }
            set { _accountModel.avg_accuracy_10_last_tests = value; OnPropertyChanged(); }
        }
        // создаем лучшую последовательность
        public double DisplayHighestConsistency
        {
            get { return _accountModel.max_consistency; }
            set { _accountModel.max_consistency = value; OnPropertyChanged(); }
        }
        // создаем среднюю последовательность
        public double DisplayAverageConsistency
        {
            get { return _accountModel.avg_consistency; }
            set { _accountModel.avg_consistency = value; OnPropertyChanged(); }
        }
        // создаем среднюю последовательность за последние 10 тестов
        public double DisplayAvgCon10lastTests
        {
            get { return _accountModel.avg_con_10_last_tests; }
            set { _accountModel.avg_con_10_last_tests = value; OnPropertyChanged(); }
        }
        public string DisplayTestsCompletedToolTip
        {
            get
            {            
                // устанавливаем подсказку
                if (DisplayTestsCompleted > 0)
                {
                    return $"{(DisplayTestsCompleted / (double)DisplayTestsStarted) * 100:0}% ({Math.Round((DisplayTestsStarted - DisplayTestsCompleted) / (double)DisplayTestsCompleted, 1)} restarts per completed test)";
                }
                else
                {
                    return "Вы еще не завершили ни одного теста :(";
                }
            }
        }
        // создаем время печатания
        public TimeSpan DisplayTimeTyping
        {
            get { return _accountModel.time_typing; }
        }
        // создаем кол-во напечатанных слов
        public int DisplayWordsTyped
        {
            get { return _accountModel.words_typed; }
        }
        // БЛОК ТЕСТОВ
        // получаем лучшие результаты тестов
        public CSVData.RecordTests RecordTests
        {
            get { return _accountModel.tests_records; }
        }
        // получаем все тесты 
        public List<CSVData.TestResult> All_Tests
        {
            get { return _accountModel.all_tests; }
            set {  _accountModel.all_tests = value; OnPropertyChanged(); }

        }
        // получаем макисмальный WPM
        public int Max_Round_WPM
        {
            get { return _accountModel.max_round_wpm; }
            set { _accountModel.max_round_wpm = value; OnPropertyChanged(); }
        }
        // создаем точечный график
        public utilities.graphs.ScatterGraph Scattergraph
        {
            get { return _accountModel.scattergraph; }
        }
        // модель точечного графа
        public PlotModel ScatterModel
        {
            get { return _accountModel.scattermodel; }
        }
        // создаем колонный график
        public utilities.graphs.BarGraph Bargraph
        {
            get { return _accountModel.bargraph; }
        }
        // создаем модель колонного нрафика
        public PlotModel BarModel
        {
            get { return _accountModel.barmodel; }
        }

        // создаем экземпляр для контроллера подсказки графа
        public PlotController customController
        {
            // берем контроллер из графа
            get { return _accountModel.custom_controller; }
        }
        // создаем экземпляр 10 последних тестов для DataGrid
        public ObservableCollection<CSVData.TestResult> Last10Tests
        {
            get { return _accountModel.last_10_tests; }
            set { _accountModel.last_10_tests = value; OnPropertyChanged(); }
        }
        // команды
        public ICommand ChangeNickNameCommand { get; set; }
        public ICommand ExportCSVCOmmand {  get; set; }
        // методы команд
        private void ChangeNickName(object obj)
        {
            _accountModel.update_nick(obj.ToString());
        }
        private void ExportCSV(object obj)
        {
            _accountModel.ExportCSV();
        }
        // создаем конструктор где заполняем данные 
        public accountVM()
        {
            // инициализируем нашу модель
            _accountModel = new AccountModel();
            // привязываем команду
            ChangeNickNameCommand = new RelayCommand(ChangeNickName);
            ExportCSVCOmmand = new RelayCommand(ExportCSV);
        }
    }
}
