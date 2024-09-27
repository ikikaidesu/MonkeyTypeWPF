using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MonkeyTypeWPF.utilities.graphs
{
    // класс для создания и настройки графа с колоннами
    // Тут присутствует View часть потому что к сожалению вынести в xaml ее нельзя
    // В документации информации также нет, единственное что там есть это привязка Series и то к oxy:Plot (другому какому-то виду графика)
    public class BarGraph
    {
        // модель для вывода на граф
        public PlotModel MyModel { get; }
        // тип диаграммы в который будем передавать значения
        private LinearBarSeries barSeries;
        // ось Х
        private CategoryAxis categoryAxis;
        // ось Y
        private LinearAxis LinearAxis;
        // максимальный WPM, он нужен для понимания кол-ва labels 
        private int Max_WPM;
        public BarGraph(int Max_WPM, List<CSVData.TestResult> All_Tests)
        {
            // мы можем по Max_WPM узнать кол-во labels
            // так как Max_WPM будет самым крайним, то разделив на 10 мы можем узнать кол-во labels до него
            // так как каждый label это диапазон 10
            this.Max_WPM = Max_WPM / 10;
            // инициализируем нашу модель
            MyModel = new PlotModel
            {
                // задаем задний фон
                Background = OxyColors.Transparent,
                // толщина стенок графа
                PlotAreaBorderThickness = new OxyThickness(0.4),
                // цвет стенок графа
                PlotAreaBorderColor = OxyColor.Parse("#2c2e31")
            };
            // создание колонн для оси
            // эти колонны означают кол-во тестов в текущем диапазоне
            // создаем хранилище
            barSeries = new LinearBarSeries
            {
                // цвет
                FillColor = OxyColor.Parse("#e2b714"),
                // вместо number кол-во колонн
                BarWidth = 80 / ((this.Max_WPM / 10) % 10 + 1),
                TrackerFormatString = "\n■Tests: {4}"
            };
            // добавляем в модель наши колонны
            MyModel.Series.Add(barSeries);

            // Настройка осей
            // создание оси Х
            categoryAxis = new CategoryAxis
            {
                // ставим снизу
                Position = AxisPosition.Bottom,
                // устанавливаем стиль линий внутри графа
                MajorGridlineStyle = LineStyle.Solid,
                // ставим толщину линий внутри графа
                MajorGridlineThickness = 0.4,
                // устанавливаем цвета линий внутри графа
                TicklineColor = OxyColor.Parse("#2c2e31"),
                MajorGridlineColor = OxyColor.Parse("#2c2e31"),
                // убираем взаимодействие
                IsPanEnabled = false,
                IsZoomEnabled = false,
                // устанавливаем цвета
                TextColor = OxyColor.Parse("#646669"),
                TitleColor = OxyColor.Parse("#646669")

            };
            // создание оси Y
            LinearAxis = new LinearAxis
            {
                // ставим слева
                Position = AxisPosition.Left,
                // устанавливаем минимум
                Minimum = 0,
                // убираем шаг в числах
                MajorStep = double.NaN,
                // пишем название 
                Title = "tests",
                // убираем палочки от графа до цифр
                TickStyle = TickStyle.None,
                // устанавливаем цвета
                TextColor = OxyColor.Parse("#646669"),
                TitleColor = OxyColor.Parse("#646669"),
                // устанавливаем размер шрифта
                TitleFontSize = 14,
                // убираем взаимодействие
                IsPanEnabled = false,
                IsZoomEnabled = false, 
            };

            // заполняем колонны
            AddValue(All_Tests);
            AddLabels(Max_WPM);

            // добавляем в нашу модель оси
            MyModel.Axes.Add(categoryAxis);
            MyModel.Axes.Add(LinearAxis);
        }
        // метод добавляющий значения для колонн
        public void AddValue(List<CSVData.TestResult> All_Tests)
        {
            // так как у человека может быть 0 тестов, то мы проверяем первый тест из tests ялвяется ли он настоящим, или затычкой которая создается если тестов нет
            // если тест затычка, то выходим
            if (String.IsNullOrEmpty(All_Tests.First().Mode)) return;
            // создаем шаг/диапазон в котором будем проверять находится ли текущий тест или нет
            // так как на сайте диапазон это x - x+9(0-9), то и мы начнем с 0 и будем проверять находится ли тест в диапазоне, а потом прибавлять к шагу 10
            int step = 0;
            // также добавим индекс для обхода тестов
            int TestInd = 0;
            //  добавим индекс текущей колонны чтобы к ней добавлялось значение
            // ну и счетчик для добавления в нее итогового значения
            int ColumnInd = 0;
            int ColumnY = 0;
            // сортируем список по WPM
            All_Tests = All_Tests.OrderBy(ch => ch.WPM).ToList();
            // обходим все тесты
            // обходим через while так как нам нужно контролировать с каким тестом мы сейчас работаем
            // чтобы не переходить к следующему если текущий не подходит в диапазон и для него диапазон нужно менять
            while (TestInd < All_Tests.Count) 
            {
                // если WPM текущего теста в диапазоне x - x+9
                if (All_Tests[TestInd].WPM >= step && All_Tests[TestInd].WPM <= step+9)
                {
                    // прибавляем к Y колонны один
                    ColumnY++;
                    // переходим к следующему тесту
                    TestInd++;
                }
                // если тест не подошел под диапазон(больше)
                else
                {
                    // добавляем колонну с получившимися данными
                    barSeries.Points.Add(new DataPoint(ColumnInd, ColumnY));
                    // обновляем айди колонны
                    ColumnInd++;
                    // обнуляем его Y
                    ColumnY = 0;
                    // переходим к следующему диапазону
                    step += 10;
                }
            }
            // для обработки последнего теста
            if (ColumnY > 0)
            {
                barSeries.Points.Add(new DataPoint(ColumnInd, ColumnY));
            }
        }
        // метод добавления Labels для оси X
        public void AddLabels(int Max_WPM)
        {
            // создаем шаг/диапазон 
            // так как на сайте диапазон это x - x+9(0-9), то и мы начнем с 0 и будем проверять находится ли тест в диапазоне, а потом прибавлять к шагу 10
            int step = 0;
            // обходим через цикл while так как нам нужно создавать лейблы пока макс значение не перестанет входить в диапазон
            while (step  < Max_WPM)
            {
                // создаем диапазон
                categoryAxis.Labels.Add($"{step}-{step+9}");
                // обновляем его
                step += 10;
            }
        }
    }
}
