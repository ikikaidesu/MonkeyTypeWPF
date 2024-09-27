using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using MonkeyTypeWPF.Properties;

namespace MonkeyTypeWPF.utilities
{
    // класс отвечающий за управление файлом и данными json, а точнее за данные аккаунта
    public static class JsonData
    {
        // путь к файлу
        private static readonly string file_path = "json_data.json";

        // проверка и создание в случае провала проверки файла json
        public static void create_json()
        {
            // Проверяем существует ли файл
            if (!File.Exists(file_path))
            {
                // Создание основного json с данными аккаунта
                JObject json = new JObject
                    (
                    new JProperty("name", string.Empty),
                    new JProperty("join_date", DateTime.Now),
                    new JProperty("tests_started", 0),
                    new JProperty("tests_completed", 0),
                    new JProperty("time_typing", TimeSpan.Zero),
                    new JProperty("words_typed", 0)
                    );
                // Записываем JSON в файл
                File.WriteAllText(file_path, json.ToString());
            }
        }
        // метод для читки json файла
        private static JObject read_json()
        {
            // Читаем файл
            string jsonContent = File.ReadAllText(file_path);
            // Парсим его
            JObject json = JObject.Parse(jsonContent);
            return json;
        }



        // методы с именем юзера 
        // получение имени
        public static string get_name()
        {
            // Парсим JSON
            JObject json = read_json();
            // Получаем значение ключа "name"
            string name = json["name"].ToString();
            return name;
        }
        // изменение имени
        public static void set_name(string name)
        {
            // Парсим JSON
            JObject json = read_json();
            // Изменяем значение ключа "name"
            json["name"] = name;
            File.WriteAllText(file_path, json.ToString());
        }
        // методы с датой юзера
        public static DateTime get_join_date()
        {
            // Парсим JSON
            JObject json = read_json();
            // Получаем значение ключа "join_date"
            DateTime date = Convert.ToDateTime(json["join_date"]);
            return date;
        }
        // методы с кол-вом начатых тестов
        // получение тестов
        public static int get_tests_started()
        {
            // Парсим JSON
            JObject json = read_json();
            // Получаем значение ключа "tests_started"
            int tests_started = (int)json["tests_started"];
            return tests_started;
        }
        // изменение тестов
        public static void update_tests_started(int tests_started)
        {
            // Парсим JSON
            JObject json = read_json();
            // Изменяем значение ключа "tests_started"
            json["tests_started"] = (int)json["tests_started"] + tests_started;
            File.WriteAllText(file_path, json.ToString());
        }
        // методы с кол-вом завершенных тестов
        // получение тестов
        public static int get_tests_completed()
        {
            // Парсим JSON
            JObject json = read_json();
            // Получаем значение ключа "tests_completed"
            int tests_completed = (int)json["tests_completed"];
            return tests_completed;
        }
        // изменение тестов
        public static void update_tests_completed()
        {
            // Парсим JSON
            JObject json = read_json();
            // Изменяем значение ключа "tests_completed"
            json["tests_completed"] = (int)json["tests_completed"] + 1;
            File.WriteAllText(file_path, json.ToString());
        }
        // методы с временем печатания
        // получение
        public static TimeSpan get_time_typing()
        {
            // Парсим JSON
            JObject json = read_json();
            // Получаем значение ключа "tests_completed"
            TimeSpan time_typing = (TimeSpan)json["time_typing"];
            return time_typing;
        }
        // изменение тестов
        public static void update_time_typing(TimeSpan time_typing)
        {
            // Парсим JSON
            JObject json = read_json();
            // Изменяем значение ключа "time_typing"
            json["time_typing"] = (TimeSpan)json["time_typing"] + time_typing;
            File.WriteAllText(file_path, json.ToString());
        }
        // получение кол-ва слов
        public static int get_words_typed()
        {
            // Парсим JSON
            JObject json = read_json();
            // Получаем значение ключа "tests_completed"
            int words_typed = (int)json["words_typed"];
            return words_typed;
        }
        // изменение кол-ва слов
        public static void update_words_typed(int word_typed)
        {
            // Парсим JSON
            JObject json = read_json();
            // Изменяем значение ключа "time_typing"
            json["words_typed"] = (int)json["words_typed"] + word_typed;
            File.WriteAllText(file_path, json.ToString());
        }
    }
}
