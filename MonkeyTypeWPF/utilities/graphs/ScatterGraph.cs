using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace MonkeyTypeWPF.utilities.graphs
{
    // класс для создания и работой с точечным графиком в профиле
    // Тут присутствует View часть потому что к сожалению вынести в xaml ее нельзя
    // В документации информации также нет, единственное что там есть это привязка Series и то к oxy:Plot (другому какому-то виду графика)
    public class ScatterGraph
    {
        // модель для вывода на граф
        public PlotModel MyModel { get; }
        // контроллер для обработки отображения подсказки
        public PlotController customController { get; private set; }
        // ось Х
        private LinearAxis xAxis;
        // ось Y(аккуратность)
        private LinearAxis yAxis1;
        // ось Y(WPM)
        private LinearAxis yAxis2;
        // точки тесты
        public ScatterSeries series1 { get; private set; }
        // точки аккуратность
        public ScatterSeries series2 { get; private set; }
        // кол-во тестов
        public int tests_count { get; private set; }
        // максимальный WPM
        public int Max_WPM { get; private set; }
        // конструктор где создаются и заполняются данные графа
        public ScatterGraph(int tests_count, int Max_WPM, List<CSVData.TestResult> All_Tests)
        {
            this.tests_count = tests_count;
            this.Max_WPM = Max_WPM;
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
                // ставим снизу
                Position = AxisPosition.Bottom,
                // убираем видимость
                IsAxisVisible = false,
                // ставим минимум
                Minimum = 5,
                // ставим максимум(берем кол-во тестов и умножаем на 10(каждый тест через 10), прибавляем 5(чтобы было место между последним тестом и правой осью))
                Maximum = this.tests_count * 10 + 5
            };
            // создание левой оси accuracy
            yAxis1 = new LinearAxis
            {
                // ставим слева
                Position = AxisPosition.Left,
                // пишем название 
                Title = "Accuracy",
                // устанавливаем размер шрифта
                TitleFontSize = 12,
                // устанавливаем значение шрифта
                TitleFontWeight = FontWeights.Bold,
                // устанавливаем стиль линий
                AxislineStyle = LineStyle.Solid,
                // устанавливаем цвета
                TextColor = OxyColor.Parse("#646669"),
                TitleColor = OxyColor.Parse("#646669"),
                // убираем палочки от графа до цифр
                TickStyle = TickStyle.None,
                // присваиваем ключ для дальнейшей адресации данных на нее
                Key = "YAxis1",
                // устанавливаем минимум и максимум
                Minimum = 0,
                Maximum = 100,
                // убираем взаимодействие
                IsPanEnabled = false,
                IsZoomEnabled = false,
                // ставим шаг между цифрами у оси
                MajorStep = 10,
                // убираем линии внутри графа
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                // переворачиваем ее
                StartPosition = 1,
                EndPosition = 0,
            };
            // создание правом оси WPM
            yAxis2 = new LinearAxis
            {
                // ставим справа
                Position = AxisPosition.Right,
                // пишем название 
                Title = "ǝʇnuıɯ ɹǝd spɹoʍ",
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
                // присваиваем ключ для дальнейшей адресации данных на нее
                Key = "YAxis2",
                // устанавливаем минимум и максимум
                Minimum = 0,
                Maximum = this.Max_WPM,
                // убираем взаимодействие
                IsPanEnabled = false,
                IsZoomEnabled = false,
                // ставим шаг между цифрами у оси
                MajorStep = 10,
                // ставим толщину линий внутри графа
                MajorGridlineThickness = 0.4,
            };
            // добавляем в нашу модель оси
            MyModel.Axes.Add(xAxis);
            MyModel.Axes.Add(yAxis1);
            MyModel.Axes.Add(yAxis2);

            // создаем точки для осей
            // это точки обозначающие WPM
            // создаем хранилище
            series1 = new ScatterSeries
            {
                // тип точек(круг)
                MarkerType = MarkerType.Circle,
                // размер
                MarkerSize = 3,
                // цвет
                MarkerFill = OxyColors.Gold,
                // ставим к какой оси крепиться
                YAxisKey = "YAxis2",
                // устанавливаем текст подсказки
                TrackerFormatString = "{Tag}"
            };

            // это точки обозначающие accuracy
            // создаем хранилище
            series2 = new ScatterSeries
            {
                // тип точек(треугольник)
                MarkerType = MarkerType.Triangle,
                // размер
                MarkerSize = 3,
                // цвет
                MarkerFill = OxyColor.Parse("#646669"),
                // ставим к какой оси крепиться
                YAxisKey = "YAxis1",
                // устанавливаем текст подсказки
                TrackerFormatString = "{Tag}"
            };
            // добавляем в модель наши точки
            MyModel.Series.Add(series1);
            MyModel.Series.Add(series2);

            // заполняем точками
            AddPoints(All_Tests);
            // создаем линии
            AddLines(series1);

            // настраиваем вывод подсказки

            customController = new PlotController();
            customController.UnbindMouseDown(OxyMouseButton.Left);
            customController.BindMouseEnter(PlotCommands.HoverSnapTrack);
        }
        // метод заполнения точек
        public void AddPoints(List<CSVData.TestResult> All_tests)
        {
            // так как у человека может быть 0 тестов, то мы проверяем первый тест из tests ялвяется ли он настоящим, или затычкой которая создается если тестов нет
            // если тест затычка, то выходим
            if (String.IsNullOrEmpty(All_tests.First().Mode)) return;
            // создаем счетчик для X у точек, он будет фиксированным так как не имеет особого значения, шаг будет равен 10
            int StepPoint = 10;
            // добавляем данные
            foreach (var i in All_tests)
            {
                // добавляем точку WPM в хранилище, присваивая
                // Х - наш шаг
                // Y - wpm теста
                // tag - текст подсказки
                series1.Points.Add(new ScatterPoint(StepPoint, Convert.ToInt32(i.WPM)) { Tag = $"wpm: {i.WPM}\nraw: {i.RawWPM}\nacc: {i.Accuracy}\n\nmode: {i.Mode}\nnumbers: {i.IsNumbers}\npunctuation: {i.IsPunctuation}\nlanguage: {i.Language}\n\ndate: {i.Date.ToString("d MMMM yyyy HH:mm", new CultureInfo("en-EN"))}" });
                // добавляем точку accuracy в хранилище, присваивая
                // X - наш шаг
                // Y - accuracy теста
                // tag - текст подсказки
                series2.Points.Add(new ScatterPoint(StepPoint, Convert.ToInt32(i.Accuracy)) { Tag = $"error rate: {100 - i.Accuracy}%\nacc: {i.Accuracy}%" });
                // увеличиваем шаг
                StepPoint += 10;
            }
        }
        // метод добавления линий которые идут в ступенчатом виде от первой точки до последующей больше ее точки
        public void AddLines(ScatterSeries series)
        {
            // если точек(а значит и тестов нет), то выходим из метода
            if (series.Points.Count == 0) return;
            // берем стартовую точку
            ScatterPoint currentPoint = series.Points[0];
            // обходим следующие
            foreach (var i in series.Points)
            {
                // если следующая точка больше по WPM чем наша
                if (i.Y > currentPoint.Y)
                {
                    // идем горизонтально до нее
                    var Xline = new LineAnnotation
                    {
                        // тип линии(идет по X или по Y)
                        Type = LineAnnotationType.Horizontal,
                        // стартовый Х(именно Minimun, потому что при обычном она начинается с левого края графа)
                        MinimumX = currentPoint.X,
                        // стартовый Y
                        Y = currentPoint.Y,
                        // конечный Х( + 0.1 так как без этого линия не совсем доходила до конца из-за чего справа снизу у ступеньки так сказать было видно пустое пространство)
                        MaximumX = i.X + 0.1,
                        // конечный Y
                        MaximumY = currentPoint.Y,
                        // цвет
                        Color = OxyColor.Parse("#646669"),
                        // тип линии
                        LineStyle = LineStyle.Solid,
                        // ставим к какой оси крепиться
                        YAxisKey = "YAxis2",
                        // делаем так чтобы точки были поверх линий
                        Layer = AnnotationLayer.BelowSeries,
                        // толщина линии
                        StrokeThickness = 3
                    };
                    // добавляем линию на модель
                    MyModel.Annotations.Add(Xline);
                    // идем вертикально до нее от конца горизонтальной линии
                    var Yline = new LineAnnotation
                    {
                        // тип линии(идет по X или по Y)
                        Type = LineAnnotationType.Vertical,
                        // стартовый Х
                        X = i.X,
                        // стартовый Y(именно Minimun, потому что при обычном она начинается с нижнего края графа)
                        MinimumY = currentPoint.Y,
                        // конечный X
                        MaximumX = i.X,
                        // конечный Y
                        MaximumY = i.Y,
                        // цвет
                        Color = OxyColor.Parse("#646669"),
                        // тип линии
                        LineStyle = LineStyle.Solid,
                        // ставим к какой оси крепиться
                        YAxisKey = "YAxis2",
                        // делаем так чтобы точки были поверх линий
                        Layer = AnnotationLayer.BelowSeries,
                        // толщина линии
                        StrokeThickness = 3
                    };
                    MyModel.Annotations.Add(Yline);
                    currentPoint = i;
                }
                // если меньше по WPM(значит просто идем до нее горизонтально не уменьшая высоту)
                else
                {
                    // идем горизонтально до нее
                    var Xline = new LineAnnotation
                    {
                        // тип линии(идет по X или по Y)
                        Type = LineAnnotationType.Horizontal,
                        // стартовый Х(именно Minimun, потому что при обычном она начинается с левого края графа)
                        MinimumX = currentPoint.X,
                        // стартовый Y
                        Y = currentPoint.Y,
                        // конечный Х
                        MaximumX = i.X,
                        // конечный Y
                        MaximumY = currentPoint.Y,
                        // цвет
                        Color = OxyColor.Parse("#646669"),
                        // тип линии
                        LineStyle = LineStyle.Solid,
                        // ставим к какой оси крепиться
                        YAxisKey = "YAxis2",
                        // делаем так чтобы точки были поверх линий
                        Layer = AnnotationLayer.BelowSeries,
                        // толщина линии
                        StrokeThickness = 3
                    };
                }
            }
            // тут создаем последнюю линию на графике у крайней точки до правой оси
            // берем последнюю точку
            var lastPoint = currentPoint;
            // идем горизонтально до нее
            var finalXline = new LineAnnotation
            {
                // тип линии(идет по X или по Y)
                Type = LineAnnotationType.Horizontal,
                // стартовый Х(именно Minimun, потому что при обычном она начинается с левого края графа)
                MinimumX = lastPoint.X,
                // стартовый Y
                Y = lastPoint.Y,
                // конечный Х ( берем максимум оси X по шагам и вычитаем - 2.5 чтобы она не касалась оси)
                MaximumX = MyModel.Axes[0].Maximum - 2.5,
                // конечный Y
                MaximumY = lastPoint.Y,
                // цвет
                Color = OxyColor.Parse("#646669"),
                // тип линии
                LineStyle = LineStyle.Solid,
                // ставим к какой оси крепиться
                YAxisKey = "YAxis2",
                // делаем так чтобы точки были поверх линий
                Layer = AnnotationLayer.BelowSeries,
                // толщина линии
                StrokeThickness = 3
            };
            // добавляем ее 
            MyModel.Annotations.Add(finalXline);
        }
    }
}

