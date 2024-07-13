using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MonkeyTypeWPF.utilities;
using Ookii.Dialogs.Wpf;

namespace MonkeyTypeWPF.models
{
    public class TestResultsModel
    {
        // метод добавления теста
        public async void AddTest(string Mode, double WPM, double RawWPM, double Accuracy, double Consistency, bool IsNumbers, bool IsPunctuation, string Language, string Chars, int RestartCount, TimeSpan Time, int WordsTyped)
        {
            // вызываем метод в CSVData который обновляет csv файл
            await CSVData.add_test(Mode, WPM, RawWPM, Accuracy, Consistency, IsNumbers, IsPunctuation, Language, Chars, RestartCount, Time);
            // Теперь обновляем данные в JsonData
            // обновляем кол-во начатых тестов(добавляем к ним кол-во рестартов)
            JsonData.update_tests_started(RestartCount + 1);
            // обновляем кол-во завершенных тестов(прибавляем 1)
            JsonData.update_tests_completed();
            // обновляем кол-во времени на тесты(прибавляем)
            JsonData.update_time_typing(Time);
            // обновляем кол-во напечатанных слов
            JsonData.update_words_typed(WordsTyped);
        }
    }
}
