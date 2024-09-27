using MonkeyTypeWPF.views;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MonkeyTypeWPF.utilities
{
    public static class CSVData
    {
        // путь к файлу
        private static readonly string file_path = "csv_data.csv";

        public static void create_csv()
        {
            // Проверяем существует ли файл
            if (!File.Exists(file_path))
            {
                // Создаем CSV файл
                using (StreamWriter writer = new StreamWriter(file_path))
                {
                    // Записываем колонны
                    writer.WriteLine("Number,Mode,WPM,RawWPM,Accuracy,Consistency,Date,IsNumbers,IsPunctuation,Language,Chars,RestartCount,Time,IsRecord, QuoteLength");
                }
            }
        }

        public static void export_csv(string UserPath)
        {
            // Путь к файлу внутри проекта
            string sourceFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, file_path);
            // Путь к файлу в выбранной папке
            string destinationFilePath = Path.Combine(UserPath, file_path);
            try
            {
                File.Copy(sourceFilePath, destinationFilePath, true);
            }
            catch 
            {

            }
        }

        // класс для создания теста
        public class TestResult : MainModelView
        {
            private string mode;
            private double wpm;
            private double rawWpm;
            private double accuracy;
            private double consistency;
            private DateTime date;
            private bool isNumbers;
            private bool isPunctuation;
            private string language;
            private string chars;
            private int restartCount;
            private TimeSpan time;
            private bool isrecord;

            public string Mode
            {
                get => mode;
                set
                {
                    if (mode != value)
                    {
                        mode = value;
                        OnPropertyChanged(nameof(Mode));
                    }
                }
            }

            public double WPM
            {
                get => wpm;
                set
                {
                    if (wpm != value)
                    {
                        wpm = value;
                        OnPropertyChanged(nameof(WPM));
                    }
                }
            }

            public double RawWPM
            {
                get => rawWpm;
                set
                {
                    if (rawWpm != value)
                    {
                        rawWpm = value;
                        OnPropertyChanged(nameof(RawWPM));
                    }
                }
            }

            public double Accuracy
            {
                get => accuracy;
                set
                {
                    if (accuracy != value)
                    {
                        accuracy = value;
                        OnPropertyChanged(nameof(Accuracy));
                    }
                }
            }

            public double Consistency
            {
                get => consistency;
                set
                {
                    if (consistency != value)
                    {
                        consistency = value;
                        OnPropertyChanged(nameof(Consistency));
                    }
                }
            }

            public DateTime Date
            {
                get => date;
                set
                {
                    if (date != value)
                    {
                        date = value;
                        OnPropertyChanged(nameof(Date));
                    }
                }
            }

            public bool IsNumbers
            {
                get => isNumbers;
                set
                {
                    if (isNumbers != value)
                    {
                        isNumbers = value;
                        OnPropertyChanged(nameof(IsNumbers));
                    }
                }
            }

            public bool IsPunctuation
            {
                get => isPunctuation;
                set
                {
                    if (isPunctuation != value)
                    {
                        isPunctuation = value;
                        OnPropertyChanged(nameof(IsPunctuation));
                    }
                }
            }

            public string Language
            {
                get => language;
                set
                {
                    if (language != value)
                    {
                        language = value;
                        OnPropertyChanged(nameof(Language));
                    }
                }
            }

            public string Chars
            {
                get => chars;
                set
                {
                    if (chars != value)
                    {
                        chars = value;
                        OnPropertyChanged(nameof(Chars));
                    }
                }
            }

            public int RestartCount
            {
                get => restartCount;
                set
                {
                    if (restartCount != value)
                    {
                        restartCount = value;
                        OnPropertyChanged(nameof(RestartCount));
                    }
                }
            }

            public TimeSpan Time
            {
                get => time;
                set
                {
                    if (time != value)
                    {
                        time = value;
                        OnPropertyChanged(nameof(Time));
                    }
                }
            }
            public bool IsRecord
            {
                get => isrecord;
                set
                {
                    if (isrecord != value)
                    {
                        isrecord = value;
                        OnPropertyChanged(nameof(IsRecord));
                    }
                }
            }
        }



        // метод добавления теста
        public static async Task add_test(string mode, double wpm, double rawwpm, double accuracy, double consistency, bool isnumbers, bool ispunctuation, string language, string chars, int restartcount, TimeSpan time)
        {
            // меняем все значения double округляя до 2 и меняя запятую на точку
            string WPM = Math.Round(wpm, 2).ToString().Replace(',', '.');
            string RawWPM = Math.Round(rawwpm, 2).ToString().Replace(',', '.');
            string Accuracy = Math.Round(accuracy, 2).ToString().Replace(',', '.');
            string Consistency = Math.Round(consistency, 2).ToString().Replace(',', '.');
            // (получаем вне врайтера чтобы не мешать процессу)
            // номер теста
            // проверяем рекорд ли
            bool IsRecord = CheckIsRecord(mode, wpm);
            int TestNum = read_tests().Count + 1;
            using (StreamWriter writer = new StreamWriter(file_path, true))
            {
                // время начала теста(дата)
                DateTime Date = DateTime.Now - time;
                // создаем строку для добавления
                string line = $"{TestNum},{mode},{WPM},{RawWPM},{Accuracy},{Consistency},{Date},{isnumbers},{ispunctuation},{language},{chars},{restartcount},{time},{IsRecord}";
                // добавляем
                writer.WriteLine(line);
            }
        }
        // метод получения всех тестов
        public static List<TestResult> read_tests()
        {
            // сюда будем записывать тесты
            List<TestResult> tests = new List<TestResult>();
            // читаем файл csv
            using (StreamReader reader = new StreamReader(file_path))
            {
                string line;
                // Пропускаем заголовки
                reader.ReadLine();
                // обходим все строки
                while ((line = reader.ReadLine()) != null)
                {
                    // Разделяем строку на колонки
                    string[] CurrentLine = line.Split(',');
                    // создаем экземпляр теста
                    TestResult test = new TestResult
                    {
                        Mode = CurrentLine[1],
                        WPM = Convert.ToDouble(CurrentLine[2], CultureInfo.InvariantCulture),
                        RawWPM = Convert.ToDouble(CurrentLine[3], CultureInfo.InvariantCulture),
                        Accuracy = Convert.ToDouble(CurrentLine[4], CultureInfo.InvariantCulture),
                        Consistency = Convert.ToDouble(CurrentLine[5], CultureInfo.InvariantCulture),
                        Date = Convert.ToDateTime(CurrentLine[6]),
                        IsNumbers = Convert.ToBoolean(CurrentLine[7]),
                        IsPunctuation = Convert.ToBoolean(CurrentLine[8]),
                        Language = Convert.ToString(CurrentLine[9]),
                        // так как чарс будет иметь вид 65/0/2/0, то мы так и делаем разделяя его по / и переводя все в int
                        Chars = Convert.ToString(CurrentLine[10]),
                        RestartCount = Convert.ToInt32(CurrentLine[11]),
                        Time = TimeSpan.Parse(CurrentLine[12]),
                        IsRecord = Convert.ToBoolean(CurrentLine[13])
                    };
                    // заполняем лист
                    tests.Add(test);
                }
            }
            // если тесты уже есть
            if (tests.Count > 0)
            {
                return tests;
            }
            // если тестов нет, то возвращаем лист с тестом где все по 0
            else
            {
                tests.Add(new TestResult());
                return tests;
            }
        }
        // метод получения лучших результатов во всех режимах
        // создадим для него класс 
        public class RecordTests : MainModelView
        {
            private TestResult _time15;
            private TestResult _time30;
            private TestResult _time60;
            private TestResult _time120;
            private TestResult _words10;
            private TestResult _words25;
            private TestResult _words50;
            private TestResult _words100;

            public TestResult Time15
            {
                get => _time15;
                set
                {
                    if (_time15 != value)
                    {
                        _time15 = value;
                        OnPropertyChanged(nameof(Time15));
                    }
                }
            }

            public TestResult Time30
            {
                get => _time30;
                set
                {
                    if (_time30 != value)
                    {
                        _time30 = value;
                        OnPropertyChanged(nameof(Time30));
                    }
                }
            }

            public TestResult Time60
            {
                get => _time60;
                set
                {
                    if (_time60 != value)
                    {
                        _time60 = value;
                        OnPropertyChanged(nameof(Time60));
                    }
                }
            }

            public TestResult Time120
            {
                get => _time120;
                set
                {
                    if (_time120 != value)
                    {
                        _time120 = value;
                        OnPropertyChanged(nameof(Time120));
                    }
                }
            }

            public TestResult Words10
            {
                get => _words10;
                set
                {
                    if (_words10 != value)
                    {
                        _words10 = value;
                        OnPropertyChanged(nameof(Words10));
                    }
                }
            }

            public TestResult Words25
            {
                get => _words25;
                set
                {
                    if (_words25 != value)
                    {
                        _words25 = value;
                        OnPropertyChanged(nameof(Words25));
                    }
                }
            }

            public TestResult Words50
            {
                get => _words50;
                set
                {
                    if (_words50 != value)
                    {
                        _words50 = value;
                        OnPropertyChanged(nameof(Words50));
                    }
                }
            }

            public TestResult Words100
            {
                get => _words100;
                set
                {
                    if (_words100 != value)
                    {
                        _words100 = value;
                        OnPropertyChanged(nameof(Words100));
                    }
                }
            }
        }
        public static RecordTests get_best_test_results()
        {
            // создаем экземпляр рекордов
            RecordTests Records = new RecordTests();
            // получаем тесты
            List<TestResult> tests = read_tests();
            // Перебор всех тестов в разделе tests
            foreach (var test in tests)
            {
                // проверка есть ли вообще там число или если есть то больше ли оно текущего
                switch (test.Mode)
                {
                    case "time 15":
                    {
                        if (Records.Time15 == null || Convert.ToDouble(test.WPM) > Records.Time15.WPM)
                        {
                            Records.Time15 = test;
                        }
                        break;
                    }
                    case "time 30":
                    {
                        if (Records.Time30 == null || Convert.ToDouble(test.WPM) > Records.Time30.WPM)
                        {
                            Records.Time30 = test;
                        }
                        break;
                    }
                    case "time 60":
                    {
                        if (Records.Time60 == null || Convert.ToDouble(test.WPM) > Records.Time60.WPM)
                        {
                            Records.Time60 = test;
                        }
                        break;
                    }
                    case "time 120":
                    {
                        if (Records.Time120 == null || Convert.ToDouble(test.WPM) > Records.Time120.WPM)
                        {
                            Records.Time120 = test;
                        }
                        break;
                    }
                    case "words 10":
                    {
                        if (Records.Words10 == null || Convert.ToDouble(test.WPM) > Records.Words10.WPM)
                        {
                            Records.Words10 = test;
                        }
                        break;
                    }
                    case "words 25":
                    {
                        if (Records.Words25 == null || Convert.ToDouble(test.WPM) > Records.Words25.WPM)
                        {
                            Records.Words25 = test;
                        }
                        break;
                    }
                    case "words 50":
                    {
                        if (Records.Words50 == null || Convert.ToDouble(test.WPM) > Records.Words50.WPM)
                        {
                            Records.Words50 = test;
                        }
                        break;
                    }
                    case "words 100":
                    {
                        if (Records.Words100 == null || Convert.ToDouble(test.WPM) > Records.Words100.WPM)
                        {
                            Records.Words100 = test;
                        }
                        break;
                    }
                }
            }
            return Records;
        }
        // метод проверки является ли тест рекордным
        public static bool CheckIsRecord(string Mode, double WPM)
        {
            // берем все тесты
            List<TestResult> tests = read_tests();
            // берем из тестов кол-во тех у кого режим равен введенному и WPM больше
            int HigherWPMTestsCount = tests.Where(ch => ch.Mode == Mode && ch.WPM > WPM).Count();
            // если таких 0, то тест рекордный, иначе нет
            return HigherWPMTestsCount == 0;
        }

        // метод получения максимального округленного WPM
        public static int get_max_round_wpm_tests()
        {
            // получаем тесты
            List<TestResult> tests = read_tests();
            // так как нам нужны для графов числа больше максимума и приближенные к кратным 10(103.54 = 110, 106.42 = 110)
            // то мы это и делаем путем деления числа на 10, а после округления до большего числа на 1 умножаем обратно на 10 и получаем то, что нам нужно
            return Convert.ToInt32(Math.Ceiling(tests.Max(ch => ch.WPM) / 10.0) * 10.0);
        }
        // блок данных статистики
        // метод получения максимального wpm в виде класса(для статистики)
        public static TestResult get_max_wpm_test()
        {
            // получаем тесты
            List<TestResult> tests = read_tests();
            // максимальный wpm
            double MaxWPM = tests.Max(ch => ch.WPM);
            // возвращяем тест где wpm равен максимальному
            return tests.Where(ch => ch.WPM == MaxWPM).Last();
        }
        // метод получения среднего WPM
        public static double get_avg_wpm()
        {
            // получаем тесты
            List<TestResult> tests = read_tests();
            // возвращаем среднее значение по WPM
            return tests.Average(ch => ch.WPM);
        }
        // метод получения среднего WPM за последние 10 тестов
        public static double get_avg_wpm_10_last_tests()
        {
            // получаем тесты
            List<TestResult> tests = read_tests();
            // пытаемся пропустить все тесты до последних 10(если меньше 10 в общем, то не пропускаем)
            tests = tests.Skip(Math.Max(0, tests.Count - 10)).ToList();
            // возвращаем среднее значение по WPM
            return tests.Average(ch => ch.WPM);
        }
        // метод получения высшего сырого wpm
        public static double get_max_raw_wpm()
        {
            // получаем тесты
            List<TestResult> tests = read_tests();
            // возвращяем максимальный RawWPM
            return tests.Max(ch => ch.RawWPM);
        }
        // метод получения среднего сырого wpm
        public static double get_avg_raw_wpm()
        {
            // получаем тесты
            List<TestResult> tests = read_tests();
            // возвращаем среднее значение по WPM
            return tests.Average(ch => ch.RawWPM);
        }
        // метод получения среднего сырого wpm за последние 10 тестов
        public static double get_avg_rawwpm_10_last_tests()
        {
            // получаем тесты
            List<TestResult> tests = read_tests();
            // пытаемся пропустить все тесты до последних 10(если меньше 10 в общем, то не пропускаем)
            tests = tests.Skip(Math.Max(0, tests.Count - 10)).ToList();
            // возвращаем среднее значение по WPM
            return tests.Average(ch => ch.RawWPM);
        }
        // метод получения максимальной точности
        public static double get_max_accuracy()
        {
            // получаем тесты
            List<TestResult> tests = read_tests();
            // возвращяем максимальный Accuracy
            return tests.Max(ch => ch.Accuracy);
        }
        // метод получения средней точности
        public static double get_avg_accuracy()
        {
            // получаем тесты
            List<TestResult> tests = read_tests();
            // возвращаем среднее значение по Accuracy
            return tests.Average(ch => ch.Accuracy);
        }
        // метод получения средней точности за последние 10 тестов
        public static double get_avg_acc_10_last_tests()
        {
            // получаем тесты
            List<TestResult> tests = read_tests();
            // пытаемся пропустить все тесты до последних 10(если меньше 10 в общем, то не пропускаем)
            tests = tests.Skip(Math.Max(0, tests.Count - 10)).ToList();
            // возвращаем среднее значение по Accuracy
            return tests.Average(ch => ch.Accuracy);
        }
        // метод получения лучшей последовательности
        public static double get_max_consistency()
        {
            // берем все тесты
            List<TestResult> tests = read_tests();
            return tests.Max(ch => ch.Consistency);
        }
        // метод получения средней последовательности
        public static double get_avg_consistency()
        {
            // берем все тесты
            List<TestResult> tests = read_tests();
            return tests.Average(ch => ch.Consistency);
        }
        // метод получения средней последовательности за последние 10 тестов
        public static double get_avg_con_10_last_tests()
        {
            // получаем тесты
            List<TestResult> tests = read_tests();
            // пытаемся пропустить все тесты до последних 10(если меньше 10 в общем, то не пропускаем)
            tests = tests.Skip(Math.Max(0, tests.Count - 10)).ToList();
            // возвращаем среднее значение по Consistency
            return tests.Average(ch => ch.Consistency);
        }

        // метод для заполнения тестами таблицу последних 10 тестов
        public static ObservableCollection<TestResult> get_10_last_tests()
        {
            // создаем хранилище для вывода
            ObservableCollection<TestResult> Last10Tests = new ObservableCollection<TestResult>();
            // получаем тесты
            List<TestResult> tests = read_tests();
            // так как у человека может быть 0 тестов, то мы проверяем первый тест из tests ялвяется ли он настоящим, или затычкой которая создается если тестов нет
            // если тест затычка, то возвращаем пустую колекцию
            if (String.IsNullOrEmpty(tests.First().Mode)) return new ObservableCollection<TestResult>();
            foreach (var i in tests.Skip(Math.Max(0, tests.Count - 10)))
            {
                Last10Tests.Add(i);
            }
            return Last10Tests;
        }
    }

}
