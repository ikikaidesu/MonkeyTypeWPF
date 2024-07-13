using MonkeyTypeWPF.models;
using MonkeyTypeWPF.utilities;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace MonkeyTypeWPF.modelviews
{
    internal class typingVM : utilities.MainModelView
    {
        // создаем экземпляр нашей модели
        private readonly typingmodel _typingmodel;
        // создаем экземпляр для MainVM для переходов
        private readonly navigationVM _navigationVM;
        // создаем переключатель обозначающий то что тест начат
        private bool _isteststarted;
        // создаем показывающийся текст
        public string DisplayText
        {
            get { return _typingmodel.text; }
            set { _typingmodel.text = value; OnPropertyChanged(); }
        }
        // создаем параметры типа печатания
        // не знаю чем думал когда делал так, хотя с другой стороны я не знал что можно создать класс и привязать как данные
        // в идеале это нужно было бы переделать в одно свойство в виде класса которое в зависимости от typing_type брало бы какой-то другой экземпляр класса
        // после чего просто биндить, но что имеем то имеем
        public int DisplayTypingParameter_1
        {
            get { return GetDisplayTypingParameter(0); }
        }
        public int DisplayTypingParameter_2
        {
            get { return GetDisplayTypingParameter(1); }
        }
        public int DisplayTypingParameter_3
        {
            get { return GetDisplayTypingParameter(2); }
        }
        public int DisplayTypingParameter_4
        {
            get { return GetDisplayTypingParameter(3); }
        }
        // видимость режимов 
        // мы будем ее скрывать если человек начнет тест, чтобы он не мог менять текст 
        private bool _typingsettingsvisibility;
        public bool TypingSettingsVisibility
        {
            get { return _typingsettingsvisibility; }
            set { _typingsettingsvisibility = value; OnPropertyChanged(); }
        }
        // так как у нас костыль в плане переноса строк из-за которого нам нужно учитывать размер шрифта и textblock, то и привязать эти данные придется
        public int FontSizeText
        {
            get { return _typingmodel.charSize; }
        }
        public int WidthText
        {
            get { return _typingmodel.textbLockWidth; }
        }
        // добавляем свойство с текстом подсказки над текстом которое оповещает сколько времени/слов осталось
        public string ToolTipText
        {
            get { return _typingmodel.ToolTipText; }
            set { _typingmodel.ToolTipText = value; OnPropertyChanged(); }
        }
        // пользовательский ввод где мы при взаимодействии вызываем метод обновляющий данные
        public string UserInput
        {
            get => _typingmodel.userInput;
            set
            {
                _typingmodel.userInput = value;
                OnPropertyChanged();
                OnUserInputChanged();
            }
        }
        // коллекция цветных символов
        public ObservableCollection<ColoredCharacter> ColoredUserInput
        {
            get => _typingmodel.coloredUserInput;
            set
            {
                _typingmodel.coloredUserInput = value;
                OnPropertyChanged();
            }
        }


        // команды 
        public ICommand AddPunctiationCommand { get; set; }
        public ICommand AddNumbersCommand { get; set; }
        public ICommand ChangeTypingFormCommand { get; set; }
        public ICommand ChangeTypingParameterCommand { get; set; }
        public ICommand ResetTextCommand { get; set; }
        // методы комманд
        public void AddPunctuation(object obj)
        {
            _typingmodel.ChangePunctuationValue();
            UpdateTextSettings();
        }
        public void AddNumbers(object obj)
        {
            _typingmodel.ChangeNumbersValue();
            UpdateTextSettings();
        }
        public void ChangeTypingForm(object obj)
        {
            if ((string)obj != _typingmodel.typing_type )
            {
                _typingmodel.ChangeTypeTyping();
                OnPropertyChanged(nameof(DisplayTypingParameter_1));
                OnPropertyChanged(nameof(DisplayTypingParameter_2));
                OnPropertyChanged(nameof(DisplayTypingParameter_3));
                OnPropertyChanged(nameof(DisplayTypingParameter_4));
                UpdateTextSettings();
                ToolTipText = _typingmodel.ToolTipText;
            }
        }
        public void ChangeTypingParameter(object obj)
        {
            _typingmodel.ChangeParameter((int)obj);
            UpdateTextSettings();
        }
        public void ResetText(object obj)
        {
            UpdateTextSettings();
            _typingmodel.UpdateRestartCount();
        }

        public typingVM(navigationVM navigationVM)

        {
            // создаем экземпляр нашей модели
            _typingmodel = new typingmodel();
            // заполняем
            _navigationVM = navigationVM;
            // устанавливаем видимость настроек печатания
            TypingSettingsVisibility = true;
            // устанавливаем то что тест еще не начат
            _isteststarted = false;
            // привязываем команды
            AddPunctiationCommand = new RelayCommand(AddPunctuation);
            AddNumbersCommand = new RelayCommand(AddNumbers);
            ChangeTypingFormCommand = new RelayCommand(ChangeTypingForm);
            ChangeTypingParameterCommand = new RelayCommand(ChangeTypingParameter);
            ResetTextCommand = new RelayCommand(ResetText);
        }

        // метод получения данных для параметров теста
        public int GetDisplayTypingParameter(int index)
        {
            return _typingmodel.typing_type == "time" ? _typingmodel.time_typing_list[index] : _typingmodel.words_count_list[index];
        }

        // метод обновляющий данные при смене параметра печатания
        public void UpdateTextSettings()
        {
            DisplayText = _typingmodel.UpdateString();
            UserInput = _typingmodel.GetInputText();
            ColoredUserInput = _typingmodel.GetColoredUserInput();
        }

        // метод обновления данных при взаимодействии с InputText
        private void OnUserInputChanged()
        {
            // обновляем коллекцию
            ColoredUserInput = _typingmodel.UpdateColoredUserInput();
            // проверяем видимость объектов в зависимости от состояния теста
            TypingSettingsVisibility = UserInput.Length > 0 || _typingmodel.IsFirstLine() ? false : true;
            // обновляем подсказку и завершаем тест в зависимости от типа теста
            switch (_typingmodel.GetTypingType())
            {
                case "words":
                    ToolTipText = _typingmodel.GetToolTipText();
                    if (UserInput.Length == DisplayText.Length)
                    {
                        ShowResults();
                    }
                    break;

                case "time":
                    if (!_isteststarted)
                    {
                        ChangeTimeInToolTip();
                        _isteststarted = true;
                    }
                    break;
            }
            // проверяем есть ли новая строка
            if (_typingmodel.GetIsNewLine())
            {
                DisplayText = _typingmodel.GetText();
                ColoredUserInput = _typingmodel.GetColoredUserInput();
                _typingmodel.UpdateIsNewLine();
            }
        }

        // метод который меняет подсказку при тесте на время
        private async void ChangeTimeInToolTip()
        {
            // пока тест не завершен мы меняем данные в подсказке
            while (!_typingmodel.IsTestComplete())
            {
                ToolTipText = _typingmodel.GetToolTipText();
                await Task.Delay(1000);
            }
            ShowResults();
            return;
        }
        // метод который показывает результаты теста
        public void ShowResults()
        {
            string testtype = _typingmodel.GetTestType();
            string language = _typingmodel.GetLanguage();
            double wpm = _typingmodel.GetWPM();
            double rawwpm = _typingmodel.GetRawWPM();
            double accuracy = _typingmodel.GetAccuracy();
            double consistency = _typingmodel.GetConsistency();
            bool IsNumbers = _typingmodel.GetIsNumbers();
            bool IsPunctuation = _typingmodel.GetIsPunctuation();
            string chars = _typingmodel.GetChars();
            TimeSpan time = _typingmodel.GetTime();
            int correctcount = _typingmodel.GetCorrectCount();
            int mistakescount = _typingmodel.GetMistakesCount();
            int restartcount = _typingmodel.GetRestartCount();
            List<PeriodData> PeriodData = _typingmodel.GetPeriodData();
            int wordstyped = _typingmodel.GetWordsTyped();
            _navigationVM.CurrentView = new TestResultsVM(_navigationVM,testtype, language, wpm, rawwpm, accuracy, consistency,IsNumbers, IsPunctuation, restartcount, chars, time, correctcount, mistakescount, PeriodData, wordstyped);
        }
    }
}
