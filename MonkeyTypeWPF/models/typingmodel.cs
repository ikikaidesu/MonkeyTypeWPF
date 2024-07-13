using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Threading.Tasks;

namespace MonkeyTypeWPF.models
{
    // класс для записи данных для графа результатов и consistency
    public class PeriodData
    {
        public double Time;
        public double WPM;
        public double RaWWPM;
        public int Mistakes;
    }


    // класс для символов
    public class ColoredCharacter
    {
        public string Character { get; set; }
        public State State { get; set; }
        public int FontSizeText { get; set; }
    }
    internal class typingmodel
    {
        // путь к файлу с русскими словами
        private readonly string words_path = "textdata/words.txt";
        private readonly string marks_path = "textdata/marks.txt";
        private readonly string numbers_path = "textdata/numbers.txt";

        // Нужна ли пунктуация
        public bool AddPunctuation { get; set; } = false;
        // нужны ли цифры
        public bool AddNumbers { get; set; } = false;
        // добавляем текст для вывода
        public string text { get; set; }
        // списки для выбора параметра в режимах
        // для слов
        public int[] words_count_list { get; } = new int[] { 10, 25, 50, 100, 150 };
        // для времени 
        public int[] time_typing_list { get; } = new int[] { 15, 30, 60, 120 };
        // выбрать язык 
        // бро реально сделал фичу с намеком на апгрейд(его не будет). . .
        public string language = "russian";
        // Выбранный тип печатания
        public string typing_type { get; set; } = "time";
        // кол-во слов в тексте
        // по хорошему нужно бы реализовать алгоритм который бы просто держал строк 5 и обновлял их когда пользователь пишет строку на новую
        // но это в идеале, а желания делать это пока что нет, поэтому будем использовать фиксированное значение
        // изначально и в целом в виде печатания на время слов будет 150(надеюсь никто не может за 2 минуты написать 150 слов...)
        // если же человек будет нажимать на кол-во слов в виде на слова, то мы просто будем менять их  кол-во
        // а если перейдет обратно во время то установим обратно 150
        // этот счетчик будет использоваться для создания текста
        // помимо этого этот и счетчик ниже будут меняться по мере изменения параметров печатания
        public int words_count { get; set; } = 150;
        // время на печатание
        // равно изначально 15 так как изначально стоит 15 секунд
        public int time_typing { get; set; } = 15;
        // храним массив со словами
        private static string[] words;
        // храним массив с знаками препинания
        private static string[] mark;
        // храним массив с цифрами
        private static string[] numbers;
        // ввод пользователя
        public string userInput;
        // весь ввод пользователя(все строки)
        public ObservableCollection<ColoredCharacter> UserTotalInput = new ObservableCollection<ColoredCharacter>();
        // текущий ввод пользователя(текущая строка)
        public ObservableCollection<ColoredCharacter> coloredUserInput = new ObservableCollection<ColoredCharacter>();
        // размеры шрифта и размеры textblock
        // да, это тослтый костыль и в какой-то мере нарушение MVVM, но к сожалению я не нашел другого способа создания исккуственного раздления текста на строки
        // кроме как создать имитированный textblock через formattedtext и из него получить ширину символа для получения среднего кол-ва символов в строке
        public int charSize = 32;
        public int textbLockWidth = 850;
        // таймер
        public Stopwatch stopwatch = new Stopwatch();
        // проверка нужно ли обновлять coloredUserInput, нужна когда мы обнуляем ввод пользователя при новой строке
        public bool IsNewLine = false;
        // в MonkeyType есть параметр Accuracy который считает все правильные и неправильные символы(даже если вы напишите и удалите)
        // счетчик неправильно введенных символов
        public int MistakesCount = 0;
        // счетчик правильных символов
        public int CorrectCount = 0;
        // счетчик пропущенных символов
        public int MissedCount = 0;
        // список для хранения временных результатов за промежуток времени(для графа результатов)
        public List<PeriodData> PeriodData = new List<PeriodData>();
        // временный счетчик ошибок для списка данных(работать будет по принципу того что каждую секунду мы будем вычитать из общего кол-ва ошибок этот и добавлять в данные, после чего
        // присваивать общее кол-во ошибок этому счетчику передвигая его
        public int PeriodMissedCount = 0;
        // кол-во рестартов
        public int RestartCount = 0;
        // текст подсказка при вводе теста
        public string ToolTipText;

        // создаем конструкток класса
        public typingmodel()
        {
            // изначально заполняем его стартовым временем потому что стартовый режим это время и параметр 15с.
            ToolTipText = $"{time_typing}";
            // заполняем наши массивы данными из файлов
            words = get_words();
            mark = get_marks();
            numbers = get_numbers();
            // создаем текст и сразу же разделяем на строки
            text = AddLineBreaks(create_text(), GetMaxCharsPerLine());
        }

        // метод обновления текста
        // он будет вызываться при смене параметров печатания в view
        // тут мы просто все обнулим и обновим текст
        public string UpdateString()
        {
            userInput = "";
            UserTotalInput.Clear();
            coloredUserInput.Clear();
            if (stopwatch.IsRunning)
            {
                stopwatch.Reset();
            }
            text = AddLineBreaks(create_text(), GetMaxCharsPerLine());
            return text;
        }

        // метод обновления подсказки
        // возвращает соответствующий тип в зависимости от типа теста
        // если время - оставщиеся время, если слова - кол-во напечатанных слов/всего

        public string UpdateToolTipText()
        {
            if (typing_type == "time")
            {
                return $"{time_typing}";
            }
            else
            {
                return $"0/{words_count}";
            }
        }
        // метод получения текущей подсказки
        public string GetToolTipText()
        {
            // если время, то возвращаем выбранное время - прошедшее со старта теста время
            if (typing_type == "time")
            {
                return $"{Math.Round(time_typing - stopwatch.Elapsed.TotalSeconds)}";
            }
            // если слова, то считаем сколько слов юзер ввел
            // если строка первая(TotalUserCount.Count == 0), то просто берем кол-во пробелов + 1, если же не первая строка, то таким же способом берем из общего текста пользователя и из текущей строки
            else
            {
                int WordsCount = UserTotalInput.Count == 0 ? coloredUserInput.Count(ch => ch.Character == " ") : UserTotalInput.Count(ch => ch.Character == " ") + coloredUserInput.Count(ch => ch.Character == " ");
                return $"{WordsCount}/{words_count}";
            }
        }

        public string create_text()
        {
            // создаем список куда будем сохранять будущий текст
            List<string> textlist = new List<string>();
            // создаем хешсет где соберем рандомные слова
            // хешсет позволяет хранить только уникальные слова что позволит не допустить дубликаты
            HashSet<string> randomWords = new HashSet<string>();
            // создаем экземпляр рандома
            Random rnd = new Random();
            // добавляем в текст указаное кол-во слов
            while (randomWords.Count < words_count)
            {
                randomWords.Add(words[rnd.Next(0, words.Length)]);
            }
            // обходим хештаблицу и заполняем список словами
            foreach (var i in randomWords)
            {
                textlist.Add(i);
            }
            // теперь проверяем нужны ли знаки препинания
            // так как знаки препинания не считаются за слово, то и учитывать их в размере не нужно
            // а значит просто вставляем их
            if (AddPunctuation)
            {
                // будем вставлять знаки препинания каждые 1-5 слов рандомно
                int index = rnd.Next(1, 6);
                while (index < textlist.Count)
                {
                    // вставляем знак препинания
                    if (index < textlist.Count)
                    {
                        textlist.Insert(index,mark[rnd.Next(0, mark.Length)]);
                    }
                    // обновляем индекс
                    index += rnd.Next(2, 6);
                }
            }
            // теперь проверяем нужны ли цифры
            // тут все не много иначе ведь цифры являются словом в данном контексте
            // а значит вы их не вставляем, а заменяем слова ими
            if (AddNumbers)
            {
                // будем вставлять цифры каждые 1-5 слов рандомно
                int index = rnd.Next(1, 6);
                while (index < textlist.Count)
                {
                    // вставляем знак препинания
                    if (index < textlist.Count)
                    {
                        textlist[index] =  numbers[rnd.Next(0, numbers.Length)];
                    }
                    // обновляем индекс
                    index += rnd.Next(2, 6);
                }
            }
            // присваеиваем текст из списка нашей переменной
            return String.Join(" ", textlist);
        }

        // метод для получения средней ширины символа в строке
        private double GetAverageCharacterWidth()
        {
            // создаем "виртуальный" текстбокс где подставляем данные как у нашего оригинального, это нужно чтобы получить среднюю ширину символа
            // так как у нас шрифт Consolas, то и средняя ширина у всех одинаковая
            var formattedText = new FormattedText(
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Consolas"),
                charSize,
                Brushes.Black,
                new NumberSubstitution(),
                1);

            return formattedText.Width / formattedText.Text.Length;
        }

        // этот метод определяет макс. кол-во допустимых символов в строке
        public int GetMaxCharsPerLine()
        {
            // получаем ширину символа
            double averageCharWidth = GetAverageCharacterWidth();
            // - 10 так как при оригинальном размере текст выходит за пределы буквально на символ
            return (int)((textbLockWidth - 10) / averageCharWidth);
        }
        // метод добавляющий переносы в строках
        public string AddLineBreaks(string text, int maxCharsPerLine)
        {
            // делаем из нашего текста массив для корректного обхода текста по словам
            var words = text.Split(' ');
            // создаем стрингбилдер
            var result = new StringBuilder();
            // создаем отчет для кол-ва символов в строке текущей
            var currentLineLength = 0;

            // обходим слова
            foreach (var word in words)
            {
                // если длина строки + длина слова + 1 больше макс. допущенного значения
                if (currentLineLength + word.Length + 1 > maxCharsPerLine)
                {
                    // создаем перенос и обнуляем счетчик
                    result.Append('\n');
                    currentLineLength = 0;
                }
                // тут мы делаем проверку что слово в строке не первое и если это так добавляем пробел и увеличиваем счетчик
                else if (currentLineLength > 0)
                {
                    result.Append(' ');
                    currentLineLength++;
                }
                // добавляем слово 
                result.Append(word);
                // увеличиваем счетчик
                currentLineLength += word.Length;
            }
            // возвращаем отформатированную строку
            return result.ToString();
        }

        // метод обновления коллекции ввода пользователя
        public ObservableCollection<ColoredCharacter> UpdateColoredUserInput()
        {
            // очищаем ее
            coloredUserInput.Clear();
            // обходим ввод пользователя 
            for (int i = 0; i < userInput.Length; i++)
            {
                // берем знак из текста
                char character = text[i];
                // добовляем в коллекцию преобразованный символ
                coloredUserInput.Add(CheckCharacter(userInput[i], character));
                // проверяем на новую линию
                CheckForNewLine(i);
            }
            // обновляем кол-во пользовательского ввода
            UpdateCharacterCounts(userInput.Length - 1);
            // проверяем таймер
            CheckTimer();
            // возвращаем коллекцию
            return coloredUserInput;
        }
        // метод который будет сверять последний введенный символ пользователя с символом из текста совпадающим по индексу
        public void UpdateCharacterCounts(int lastInputIndex)
        {
            // если ввода нет
            if (userInput.Length <= 0) return;

            // проверка на пропущенный символ(вместо символа пробел)
            if (userInput[lastInputIndex] == ' ')
            {
                MissedCount++;
            }
            // проверка на правильный символ(символ равен символу из текста)
            else if (userInput[lastInputIndex] == text[lastInputIndex])
            {
                CorrectCount++;
            }
            // проверка на неправильный символ(символ не равен символу из текста)
            else
            {
                MistakesCount++;
            }
        }

        // увеличиваем кол-во рестартов при нажатии
        public void UpdateRestartCount()
        {
            RestartCount++;
        }

        // проверка символа и его создание
        public ColoredCharacter CheckCharacter(char UserCharacter, char TextCharacter)
        {
            // создаем символ для возврата
            ColoredCharacter coloredChar= new ColoredCharacter();
            // если ввод пользователя равен пробелу, а в тексте не пробел
            if (UserCharacter == ' ' && TextCharacter != ' ')
            {
                // ставим пропущенный
                coloredChar = new ColoredCharacter { Character = TextCharacter.ToString(), State = State.Missed, FontSizeText = charSize };
            }
            // если ввод пользователя это символ
            else
            {
                // получаем состояние
                State state = GetState(UserCharacter, TextCharacter);
                // ставим его
                coloredChar = new ColoredCharacter { Character = TextCharacter.ToString(), State = state, FontSizeText = charSize };
            }
            // возвращаем наш символ
            return coloredChar;
        }

        // метод получения состояния
        public State GetState(char UserCharacter, char TextCharacter)
        {
            // если введенный символ равен оригинальному
            if (UserCharacter == TextCharacter)
            {
                // возвращаем состояние - правильный
                return State.Correct;
            }
            // иначе
            else
            {
                // возвращаем состояние - неправильный
                return State.Incorrect;
            }
        }

        // метод проверки на новую строку
        private void CheckForNewLine(int LastCharIndex)
        {
            // если символ есть и он равен \n
            if (text.Length > LastCharIndex + 1 && text[LastCharIndex + 1] == '\n')
            {
                // пропускаем текущую строку чтобы удалить ее и создать перенос на новую
                text = new string(text.Skip(LastCharIndex + 2).ToArray());
                // заполняем введеную строку в наше хранилище
                FillTotalInput();
                // очищаем пользовательский ввод
                userInput = string.Empty;
                coloredUserInput.Clear();
                IsNewLine = true;
            }
        }

        // метод получения слов из файла
        private string[] get_words()
        {
            List<string> words = new List<string>();
            StreamReader f = new StreamReader(words_path, Encoding.GetEncoding("windows-1251"));
            while (!f.EndOfStream)
            {
                string s = f.ReadLine();
                words.Add(s);
            }
            f.Close();
            return words.ToArray();
        }
        // метод получения знаков препинания из файла
        private string[] get_marks()
        {
            List<string> marks = new List<string>();
            StreamReader f = new StreamReader(marks_path);
            while (!f.EndOfStream)
            {
                string s = f.ReadLine();
                marks.Add(s);
            }
            f.Close();
            return marks.ToArray();
        }
        // метод получения цифр препинания из файла
        private string[] get_numbers()
        {
            List<string> numbers = new List<string>();
            StreamReader f = new StreamReader(numbers_path);
            while (!f.EndOfStream)
            {
                string s = f.ReadLine();
                numbers.Add(s);
            }
            f.Close();
            return numbers.ToArray();
        }
        // метод проверки первая ли строка вводится(нужно для корректной работы выключения отображения настроек)
        // без этой проверки у человека будет видно настройки если он уже ввел строку и его перекинуло на новую и его ввод очистился
        public bool IsFirstLine()
        {
            return UserTotalInput.Count > 0;
        }

        // метод смены значения пунктуации
        public void ChangePunctuationValue()
        {
            AddPunctuation = !AddPunctuation;
        }
        // метод смены значения числа
        public void ChangeNumbersValue()
        {
            AddNumbers = !AddNumbers;
        }
        // метод смены типа печатания
        public void ChangeTypeTyping()
        {
            typing_type = typing_type == "time" ? "words" : "time";
            if (typing_type == "words")
            {
                // если тип -> слова, то ставим кол-во слов = 10
                words_count = words_count_list[0];
            }
            else
            {
                // иначе ставим 150
                words_count = words_count_list[4];
            }
        }
        // метод смены значения параметра печатания
        public void ChangeParameter(int number)
        {
            if (typing_type == "time")
            {
                time_typing = number;
            }
            else
            {
                words_count = number;
            }
        }
        // метод получения типа теста
        public string GetTypingType()
        {
            return typing_type;
        }

        // метод получения есть ли цифры
        public bool GetIsNumbers()
        {
            return AddNumbers;
        }

        // метод получения есть ли пунктуация
        public bool GetIsPunctuation()
        {
            return AddPunctuation;
        }

        // метод получения рестартов
        public int GetRestartCount()
        {
            return RestartCount;
        }

        // метод получения текста
        public string GetText()
        {
            return text;
        }
        // получение булево переключение на новую строку
        // суть в том что в VM мы проверяем равна ли она true 
        //  если равна, значит нужно переходить на новую строку
        // а если нужно перейти на новую строку, то и данные обновить, что мы и делаем в if блоке
        public bool GetIsNewLine()
        {
            return IsNewLine;
        }
        // обновляем переключатель после успешной проверки
        public void UpdateIsNewLine()
        {
            IsNewLine = false;
        }
        public string GetInputText()
        {
            return userInput;
        }
        // получение коллекции ввода пользователя
        public ObservableCollection<ColoredCharacter> GetColoredUserInput()
        {
            return coloredUserInput;
        }
        // метод который проверяет пора ли завершать текст
        // а точнее дошло ли время таймера до выбранного
        // или напечатал ли человек выбранное кол-во слов
        public bool IsTestComplete()
        {
            if (typing_type == "time")
            {
                return stopwatch.ElapsedMilliseconds / 1000 >= time_typing;
            }
            else
            {
                return text.Length == userInput.Length;
            }
        }
        // метод проверки таймера
        // он нужен для корректного времени теста
        // если человек начал тест(ввел символ), то мы запускаем таймер
        // если же человек напишет что-то и потом удалит, то время когда он ничего не писал нам не нужно
        private void CheckTimer()
        {
            // если ввод не пустой и таймер не запущен
            if (!string.IsNullOrEmpty(userInput) && !stopwatch.IsRunning)
            {
                stopwatch.Start();
                // запускаем асинхронный метод который не будет мешать основному коду программы и тесту 
                // и будет записывать каждую секунду временные данные такие как time, wpm, rawwpm и ошибки
                // это нужно для consistency и для графа в результатах
                RecordPeriodData();


            }
            // если ввод пустой, таймер запущен и общий ввод равен 0,
            // последнее условие нужно для корректной проверки того что человек правда удалил все что ввел
            else if (string.IsNullOrEmpty(userInput) && stopwatch.IsRunning && UserTotalInput.Count == 0)
            {
                stopwatch.Stop();
                stopwatch.Reset();
            }
        }

        // метод для записи данных в временные отрезки
        private async Task RecordPeriodData()
        {
            // очищаем его(если тест запускается не с первого раза, то в нем могут остаться данные, а так как метод вызывается один раз за тест, то очищать можно и тут)
            PeriodData.Clear();
            // очищаем кол-во пропущенных
            MissedCount = 0;
            // пока таймер работает
            while (stopwatch.IsRunning)
            {
                // ждем секунду
                await Task.Delay(1000);
                // получаем wpm на текущий момент
                double WPM = ((double)(GetCharCount()-(GetMistakesCount() + GetMissedStateCount()))/5) / stopwatch.Elapsed.TotalMinutes;
                // получаем сырой wpm на текущий момент
                double RawWPM = ((double)GetCharCount() / 5) / stopwatch.Elapsed.TotalMinutes;
                // добавляем данные (
                // в wpm проверка на 0, ведь человек может писать только ошибками, а ими wpm не считают и из-за этого он будет отрицательным
                // ошибки получаются путем того что мы берем общее кол-во минус за последнюю секунду, после чего обновляем данные последних ошибок
                PeriodData.Add(new PeriodData { Time = stopwatch.Elapsed.TotalSeconds, WPM = WPM > 0 ? WPM : 0, RaWWPM = RawWPM, Mistakes = MissedCount - PeriodMissedCount });
                PeriodMissedCount = MissedCount;
            }
        }

        // метод получения типа теста
        public string GetTestType()
        {
            // тип теста состоит из типа и параметра
            // получаем параметр
            string parameter = typing_type == "time" ? time_typing.ToString() : words_count.ToString();
            return typing_type + " " + parameter;
        }
        // метод получения языка
        public string GetLanguage()
        {
            return language;
        }

        // метод получения WPM для результатов
        public double GetWPM()
        {
            // выключаем таймер
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
            }
            // получаем время в минутах
            var elapsedTime = stopwatch.Elapsed.TotalMinutes;
            // это нужно ведь мы не обрабатываем в методе новой строки последнюю, а там также есть заполнение Общего ввода
            // из-за чего последняя строка не идет в счет
            FillTotalInput();
            // получаем кол-во введенных слов пользователем.
            var wordCount = GetCharCount();
            var MistakesCount = GetMistakesStateCount() + GetMissedStateCount();
            var wpm = ((wordCount-MistakesCount)/5) / elapsedTime;
             return wpm;
        }

        // метод получения RawWPM
        public double GetRawWPM()
        {
            // выключаем таймер
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
            }
            // получаем время в минутах
            var elapsedTime = stopwatch.Elapsed.TotalMinutes;
            // это нужно ведь мы не обрабатываем в методе новой строки последнюю, а там также есть заполнение Общего ввода
            // из-за чего последняя строка не идет в счет
            //FillTotalInput();
            // получаем кол-во введенных слов пользователем.
            var wordCount = GetCharCount();
            var rawwpm = (wordCount/ 5) / elapsedTime;
            return rawwpm;
        }

        // метод получения Accuracy
        public double GetAccuracy()
        {
            return ((double)CorrectCount / (CorrectCount + MissedCount + MistakesCount)) * 100;
        }

        // метод получения Consistency
        public double GetConsistency()
        {
            // получаем средний wpm
            double avgWPM = PeriodData.Average(ch => ch.WPM);
            // получаем дисперсию(берем и вычитаем из всех wpm из периодов среднее и возводим в 2 степень, а после берем среднее значение всего этого
            double dispersion = PeriodData.Average(ch => Math.Pow(ch.WPM - avgWPM, 2));
            // получаем отклонение
            double deviation = Math.Sqrt(dispersion);
            double consistency = (1 - (deviation/avgWPM)) * 100;
            return consistency;
        }
        public List<PeriodData> GetPeriodData()
        {
            return PeriodData;
        }
        // метод получения Chars
        // честно говоря я не понял что такое extra на сайте в символах, поэтому оно будет нулем. . .
        public string GetChars()
        {
            return $"{GetCorrectStateCount()}/{GetMistakesStateCount()}/0/{GetMissedStateCount()}";
        }
        // получаем время в TimeSpan
        public TimeSpan GetTime()
        {
            return stopwatch.Elapsed;
        }
        // получаем кол-во правильных символов
        public int GetCorrectCount()
        {
            return CorrectCount;
        }
        // получаем кол-во ошибок
        public int GetMistakesCount()
        {
            return MistakesCount;
        }
        
        // метод получения кол-ва напечатанных слов
        public int GetWordsTyped()
        {
            return UserTotalInput.Count(ch => ch.Character == " ") + coloredUserInput.Count(ch => ch.Character == " ");
        }


        // этот метод высчитывает общее кол-во напечатанных символов не включая пробелы
        // это нужно будет для высчета wpm и rawwpm
        // почему символы, а не слова?
        // так как у нас два параметра wpm и rawwpm, то высчитать их через слова не получится
        // так как слово не правильное если в нем все неправильно и так получается что либо wpm будет высчитываться либо rawwpm
        // поэтому используется альтернатива в которой мы делим кол-во символов на 5 для получения rawwpm и кол-во символов - ошибки / 5 для wpm
        private int GetCharCount()
        {
            if (UserTotalInput.Count > 0)
            {
                return UserTotalInput.Where(ch => ch.Character != " ").Count();
            }
            else
            {
                return coloredUserInput.Where(ch => ch.Character != " ").Count();
            }
        }
        
        // получаем кол-во символов с состоянием правильный
        private int GetCorrectStateCount()
        {
            // если первая строка(тоесть полный ввод пока пустой)
            if (UserTotalInput.Count > 0)
            {
                return UserTotalInput.Where(ch => ch.State == State.Correct && ch.Character != " ").Count();
            }
            else
            {
                return coloredUserInput.Where(ch => ch.State == State.Correct && ch.Character != " ").Count();
            }
        }
        // получаем кол-во символов с состоянием неправильный
        private int GetMistakesStateCount()
        {
            // если первая строка(тоесть полный ввод пока пустой)
            if (UserTotalInput.Count > 0)
            {
                return UserTotalInput.Where(ch => ch.State == State.Incorrect).Count();
            }
            else
            {
                return coloredUserInput.Where(ch => ch.State == State.Incorrect).Count();
            }
        }
        // получаем кол-во символов с состоянием пропущенный
        private int GetMissedStateCount()
        {
            // если первая строка(тоесть полный ввод пока пустой)
            if (UserTotalInput.Count > 0)
            {
                return UserTotalInput.Where(ch => ch.State == State.Missed).Count();
            }
            else
            {
                return coloredUserInput.Where(ch => ch.State == State.Missed).Count();
            }
        }
        // метод заполнения Общего ввода на основе текущего
        // мы просто берем строку введенную пользователем и записываем ее сда
        private void FillTotalInput()
        {
            foreach (var i in coloredUserInput)
            {
                UserTotalInput.Add(i);
            }
            UserTotalInput.Add(new ColoredCharacter { Character = " ", State = State.None });
        }
    }
}
