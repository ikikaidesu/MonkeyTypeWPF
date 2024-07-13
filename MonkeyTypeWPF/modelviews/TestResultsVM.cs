using MonkeyTypeWPF.models;
using MonkeyTypeWPF.utilities;
using MonkeyTypeWPF.utilities.graphs;
using Newtonsoft.Json.Bson;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyTypeWPF.modelviews
{
    public class TestResultsVM : MainModelView
    {
        // создаем экземпляр модели
        private readonly TestResultsModel _model = new TestResultsModel();
        // создаем экземпляр для MainVM для переходов
        private readonly navigationVM _navigationVM;
        // создаем поля и свойства для передачи и вывода данных
        private string _testtype;
        private string _language;
        private double _wpm;
        private double _rawwpm;
        private double _accuracy;
        private double _consistency;
        private bool _isnumbers;
        private bool _ispunctuation;
        private int _restartcount;
        private string _chars;
        private TimeSpan _time;
        private int _correctcount;
        private int _mistakescount;
        private List<PeriodData> _perioddata;
        private int _wordstyped;
        public string TestType
        {
            get { return _testtype; }
            set { _testtype = value; OnPropertyChanged(); }
        }

        public string Language
        {
            get { return _language; }
            set { _language = value; OnPropertyChanged(); }
        }

        public double WPM
        {
            get { return _wpm; }
            set { _wpm = value; OnPropertyChanged(); }
        }

        public double RawWPM
        {
            get { return _rawwpm; }
            set { _rawwpm = value; OnPropertyChanged(); }
        }

        public double Accuracy
        {
            get { return _accuracy; }
            set { _accuracy = value; OnPropertyChanged(); }
        }

        public double Consistency
        {
            get { return _consistency; }
            set { _consistency = value; OnPropertyChanged(); }
        }

        public bool IsNumbers
        {
            get { return _isnumbers; }
            set { _isnumbers = value; OnPropertyChanged(); }
        }
        public bool IsPunctuation
        {
            get { return _ispunctuation; }
            set { _ispunctuation = value; OnPropertyChanged(); }
        }
        public int RestartCount
        {
            get { return _restartcount; }
            set { _restartcount = value; OnPropertyChanged(); }
        }
        public string Chars
        {
            get { return _chars; }
            set { _chars = value; OnPropertyChanged(); }
        }

        public TimeSpan Time
        {
            get { return _time; }
            set { _time = value; OnPropertyChanged(); }
        }
        // свойство для вывода в результате 
        public double NumTime
        {
            get { return _time.TotalSeconds; }
        }
        public int CorrectCount
        {
            get { return _correctcount; }
            set { _correctcount = value; OnPropertyChanged(); }
        }
        public int MistakesCount
        {
            get { return _mistakescount; }
            set { _mistakescount = value; OnPropertyChanged(); }
        }
        public List<PeriodData> periodData
        {
            get { return _perioddata; }
            set { _perioddata = value; OnPropertyChanged(); }
        }
        public int WordsTyped
        {
            get { return _wordstyped; }
            set { _wordstyped = value; OnPropertyChanged(); }
        }
        // свойства графа
        // создаем его
        public ScatterGraphResults scatterGraph { get; set; } 
        // получаем модель
        public PlotModel scatterModel { get; set;}
        // получаем контроллер подсказок
        public PlotController customController { get; set; }

        // свойства подсказок
        public string ToolTipWPM { get; set; }
        public string ToolTipAcc { get; set; }
        public string ToolTipRawWPM { get; set; }
        public string ToolTipCharacters { get; set; }
        public string ToolTipTime { get; set; }
        
        // команды
        public ICommand NextTestCommand { get; set; }
        
        // методы команд
        public void NextTest(object obj)
        {
            _navigationVM.CurrentView = new typingVM(_navigationVM);
        }

        public TestResultsVM(navigationVM navigationvm, string testtype, string language,double wpm, double rawwpm, double accuracy, double consistency,bool isnumbers, bool ispunctuation, int restartcount, string chars, TimeSpan time, int correctcount, int mistakescount, List<PeriodData> PeriodData, int wordstyped)
        {
            // заполняем
            _navigationVM = navigationvm;
            TestType = testtype;
            Language = language;
            WPM = wpm;
            RawWPM = rawwpm;
            Accuracy = accuracy;
            Consistency = consistency;
            IsNumbers = isnumbers;
            IsPunctuation = ispunctuation;
            RestartCount = restartcount;
            Chars = chars;
            Time = time;
            CorrectCount = correctcount;
            MistakesCount = mistakescount;
            scatterGraph = new ScatterGraphResults(PeriodData);
            WordsTyped = wordstyped;
            scatterModel = scatterGraph.MyModel;
            customController = scatterGraph.customController;
            // привязываем команды
            NextTestCommand = new RelayCommand(NextTest);
            // заполняем подсказки
            ToolTipWPM = $"{Math.Round(WPM,2)} wpm";
            ToolTipAcc = $"{Math.Round(Accuracy, 2)}% ({CorrectCount} correct / {MistakesCount} incorrect)";
            ToolTipRawWPM = $"{Math.Round(RawWPM, 2)} wpms";
            ToolTipCharacters = "correct, incorrect, extra, and missed";
            ToolTipTime = $"{Math.Round(Time.TotalSeconds, 2)}s";
            AddTest();
        }
        // метод который вызывает в модели добавление теста
        private void AddTest()
        {
            _model.AddTest(TestType, WPM, RawWPM, Accuracy, Consistency, IsNumbers, IsPunctuation, Language, Chars, RestartCount, Time, WordsTyped);
        }
    }
}
