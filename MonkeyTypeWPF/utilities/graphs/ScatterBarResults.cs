using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyTypeWPF.models;
using System.Diagnostics.Tracing;

namespace MonkeyTypeWPF.utilities.graphs
{
    // класс для создания и работой с точечным графиком в профиле
    // Тут присутствует View часть потому что к сожалению вынести в xaml ее нельзя
    // В документации информации также нет, единственное что там есть это привязка Series и то к oxy:Plot (другому какому-то виду графика)
    public class ScatterGraphResults
    {
        // модель для вывода на граф
        public PlotModel MyModel { get; }
        // контроллер для обработки отображения подсказки
        public PlotController customController { get; private set; }
        // ось Х
        private LinearAxis xAxis;
        // ось Y(WPM)
        private LinearAxis yAxis1;
        // ось Y(Errors)
        private LinearAxis yAxis2;
        // точки WPM
        public LineSeries WPMSeries { get; private set; }
        // точки RawWPM
        public LineSeries RawWPMSeries { get; private set; }
        // точки ошибки
        public ScatterSeries MistakesSeries { get; private set; }
        // конструктор где создаются и заполняются данные графа
        public ScatterGraphResults(List<PeriodData> period_tests)
        {
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

            // Настройка осей
            // создание оси Х
            xAxis = new LinearAxis
            {
                // ставим справа
                Position = AxisPosition.Bottom,
                // устанавливаем размер шрифта
                TitleFontSize = 12,
                // устанавливаем значение шрифта
                TitleFontWeight = FontWeights.Bold,
                // устанавливаем стиль линий внутри графа
                MajorGridlineStyle = LineStyle.Solid,
                // устанавливаем цвета линий внутри графа
                MajorGridlineColor = OxyColor.Parse("#2c2e31"),
                // устанавливаем стиль линий оси
                AxislineStyle = LineStyle.Solid,
                // устанавливаем цвета
                TextColor = OxyColor.Parse("#646669"),
                TitleColor = OxyColor.Parse("#646669"),
                // убираем палочки от графа до цифр
                TickStyle = TickStyle.None,
                // шаг
                MajorStep = 1,
                // убираем взаимодействие
                IsPanEnabled = false,
                IsZoomEnabled = false,
                // ставим толщину линий внутри графа
                MajorGridlineThickness = 0.4
            };
            // создание левой оси WPM
            yAxis1 = new LinearAxis
            {
                // ставим слева
                Position = AxisPosition.Left,
                // пишем название 
                Title = "Words per minute",
                // устанавливаем размер шрифта
                TitleFontSize = 12,
                // устанавливаем значение шрифта
                TitleFontWeight = FontWeights.Bold,
                // устанавливаем стиль линий
                AxislineStyle = LineStyle.Solid,
                // устанавливаем стиль линий внутри графа
                //MajorGridlineStyle = LineStyle.Solid,
                // устанавливаем цвета
                TextColor = OxyColor.Parse("#646669"),
                TitleColor = OxyColor.Parse("#646669"),
                AxislineColor = OxyColor.Parse("#2c2e31"),
                // убираем палочки от графа до цифр
                TickStyle = TickStyle.None,
                // присваиваем ключ для дальнейшей адресации данных на нее
                Key = "YAxis1",
                // минимум
                Minimum = 0,
                // убираем взаимодействие
                IsPanEnabled = false,
                IsZoomEnabled = false,
                // ставим шаг между цифрами у оси
                MajorStep = 40,
            };
            // создание правом оси Errors
            yAxis2 = new LinearAxis
            {
                // ставим справа
                Position = AxisPosition.Right,
                // пишем название 
                Title = "sɹoɹɹǝ",
                // устанавливаем размер шрифта
                TitleFontSize = 12,
                // устанавливаем значение шрифта
                TitleFontWeight = FontWeights.Bold,
                // устанавливаем цвета линий внутри графа
                MajorGridlineColor = OxyColor.Parse("#2c2e31"),
                // устанавливаем стиль линий оси
                AxislineStyle = LineStyle.Solid,
                // устанавливаем цвета
                TextColor = OxyColor.Parse("#646669"),
                TitleColor = OxyColor.Parse("#646669"),
                AxislineColor = OxyColor.Parse("#2c2e31"),
                // убираем палочки от графа до цифр
                TickStyle = TickStyle.None,
                // присваиваем ключ для дальнейшей адресации данных на нее
                Key = "YAxis2",
                // минимум
                Minimum = 0,
                // убираем взаимодействие
                IsPanEnabled = false,
                IsZoomEnabled = false,
                // убираем линии внутри графа
                MajorGridlineStyle = LineStyle.None
            };
            // добавляем в нашу модель оси
            MyModel.Axes.Add(xAxis);
            MyModel.Axes.Add(yAxis1);
            MyModel.Axes.Add(yAxis2);

            // создаем точки для осей
            // это точки обозначающие WPM
            // создаем хранилище
            // Создание серии данных с линией между точками
            WPMSeries = new LineSeries
            {
                // Цвет линии
                Color = OxyColors.Gold,
                // Толщина линии
                StrokeThickness = 2,
                // тип точек(круг)
                MarkerType = MarkerType.Circle,
                // размер точек
                MarkerSize = 3, 
                // цвет точек
                MarkerFill = OxyColors.Gold,
                // ставим к какой оси крепиться
                YAxisKey = "YAxis1",
                // устанавливаем текст подсказки
                TrackerFormatString = "wpm: {4}"
            };


            // это точки обозначающие RawWPM
            // создаем хранилище
            RawWPMSeries = new LineSeries
            {
                // Цвет линии
                Color = OxyColor.Parse("#646669"),
                // Толщина линии
                StrokeThickness = 2,
                // тип точек(круг)
                MarkerType = MarkerType.Circle,
                // размер точек
                MarkerSize = 3,
                // цвет точек
                MarkerFill = OxyColor.Parse("#646669"),
                // ставим к какой оси крепиться
                YAxisKey = "YAxis1",
                // устанавливаем текст подсказки
                TrackerFormatString = "raw: {4}"
            };

            // это точки обозначающие Mistakes
            // создаем хранилище
            MistakesSeries = new ScatterSeries
            {
                // тип точек(треугольник)
                MarkerType = MarkerType.Cross,
                // размер
                MarkerSize = 3,
                MarkerStroke = OxyColors.Red,
                // цвет
                MarkerFill = OxyColors.Red,
                // ставим к какой оси крепиться
                YAxisKey = "YAxis2",
                // устанавливаем текст подсказки
                TrackerFormatString = "Errors: {4}"
            };

            // добавляем в модель наши точки
            MyModel.Series.Add(WPMSeries);
            MyModel.Series.Add(RawWPMSeries);
            MyModel.Series.Add(MistakesSeries);

            // заполняем точками
            AddPoints(period_tests);

            // настраиваем вывод подсказки

            customController = new PlotController();
            customController.UnbindMouseDown(OxyMouseButton.Left);
            customController.BindMouseEnter(PlotCommands.HoverSnapTrack);
        }
        // метод заполнения точек wpm и RawWPM
        public void AddPoints(List<PeriodData> Period_tests)
        {
            // добавляем данные
            foreach (var i in Period_tests)
            {
                // добавляем точку WPM в хранилище, присваивая
                // Х - время
                // Y - wpm теста
                WPMSeries.Points.Add(new DataPoint(Convert.ToInt16(i.Time), Convert.ToInt32(i.WPM)));
                // добавляем точку RawWPM в хранилище, присваивая
                // X - время
                // Y - raw теста
                RawWPMSeries.Points.Add(new DataPoint(Convert.ToInt16(i.Time), Convert.ToInt32(i.RaWWPM)));
                if (i.Mistakes > 0)
                {
                    MistakesSeries.Points.Add(new ScatterPoint(Convert.ToInt16(i.Time), i.Mistakes));
                }
            }
        }
    }
}
