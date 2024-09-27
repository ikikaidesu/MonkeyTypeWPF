using MonkeyTypeWPF.utilities;
using MonkeyTypeWPF.utilities.graphs;
using MonkeyTypeWPF.views;
using Ookii.Dialogs.Wpf;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace MonkeyTypeWPF.models
{
    // модель это объект содержащий данные для страниц
    // в данном классе будут данные юзера
    public class AccountModel
    {
        // никнейм юзера
        public string nickname { get; set; } = JsonData.get_name();
        // дата присоединения к программе(скачивания и открытия)
        public DateTime CreateAccount { get; set; } = JsonData.get_join_date();
        // кол-во начатых тестов
        public int tests_started { get; set; } = JsonData.get_tests_started();
        // кол-во завершенных тестов
        public int tests_completed { get; set; } = JsonData.get_tests_completed();
        // общее время печатания(танцы с бубном тут потому что нужно округлить для красивого вида секунды)
        public TimeSpan time_typing { get; set; } = TimeSpan.FromSeconds(Math.Round(JsonData.get_time_typing().TotalSeconds));
        // кол-во написанных слов
        public int words_typed { get; set; } = JsonData.get_words_typed();
        // лучшие результаты тестов
        public CSVData.RecordTests tests_records = CSVData.get_best_test_results();
        // все тесты юзера
        public List<CSVData.TestResult> all_tests = CSVData.read_tests();
        // максимальный округленный WPM(для графов) 
        public int max_round_wpm = CSVData.get_max_round_wpm_tests();
        // тест с максимальным WPM
        public CSVData.TestResult max_test_wpm = CSVData.get_max_wpm_test();
        // средний WPM
        public double avg_wpm = CSVData.get_avg_wpm();
        // средний WPM за последние 10 тестов
        public double avg_wpm_10_last_tests = CSVData.get_avg_wpm_10_last_tests();
        // максимальный сырой wpm
        public double max_raw_wpm = CSVData.get_max_raw_wpm();
        // средний сырой wpm
        public double avg_raw_wpm = CSVData.get_avg_raw_wpm();
        // средний сырой wpm за последние 10 тестов
        public double avg_raw_wpm_10_lasts_tests = CSVData.get_avg_rawwpm_10_last_tests();
        // максимальная точность
        public double max_accuracy = CSVData.get_max_accuracy();
        // средняя точность 
        public double avg_accuracy = CSVData.get_avg_accuracy();
        // средняя точность за последние 10 тестов
        public double avg_accuracy_10_last_tests = CSVData.get_avg_acc_10_last_tests();
        // лучшая последовательность
        public double max_consistency = CSVData.get_max_consistency();
        // средняя последовательность
        public double avg_consistency = CSVData.get_avg_consistency();
        // средняя последовательность за последние 10 тестов
        public double avg_con_10_last_tests = CSVData.get_avg_con_10_last_tests();
        // последние 10 тестов
        public ObservableCollection<CSVData.TestResult> last_10_tests = CSVData.get_10_last_tests();

        // графики
        // точечный граф
        public utilities.graphs.ScatterGraph scattergraph
        {
            get { return new utilities.graphs.ScatterGraph(all_tests.Count, max_round_wpm, all_tests); }
        }
        public PlotModel scattermodel
        {
            get { return scattergraph.MyModel; }
        }
        // колонный график
        public utilities.graphs.BarGraph bargraph
        {
            get { return new utilities.graphs.BarGraph(max_round_wpm, all_tests); }
        }
        // создаем модель колонного нрафика
        public PlotModel barmodel
        {
            get { return bargraph.MyModel; }
        }
        // создаем экземпляр для контроллера подсказки графа
        public PlotController custom_controller
        {
            // берем контроллер из графа
            get { return scattergraph.customController; }
        }


        // метод обновления ника юзера
        public void update_nick(string nickname)
        {
            // обновляем в файле
            JsonData.set_name(nickname);
            // устанавливаем ник в model
            this.nickname = nickname;
        }
        // метод скачивания CSV файла с тестами
        public void ExportCSV()
        {
            // создаем диалоговое окно
            var folderBrowserDialog = new VistaFolderBrowserDialog();
            // пишем описание(сверху будет)
            folderBrowserDialog.Description = "Выберите папку";
            // прикрепляем описание
            folderBrowserDialog.UseDescriptionForTitle = true;
            // если  папка выбрана
            if (folderBrowserDialog.ShowDialog() == true)
            {
                // берем выбранный путь
                string selectedFolderPath = folderBrowserDialog.SelectedPath;
                // экспортируем csv файл
                CSVData.export_csv(selectedFolderPath);
                // оповещаем об этом
                MessageBox.Show("Файл успешно скачан!");
            }
        }
    }
}
