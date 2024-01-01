// Импорт пространств имен
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Weather // Определение пространства имен 
{
    public partial class MainWindow : Window // Объявление класса MainWindow наследующего от класса Window
    {
        private readonly string apiKey = "4cd3fafa54ff0cd9c3260b9f65f6b140";// Ключ API 

        public MainWindow()// Конструктор класса
        {
            InitializeComponent(); // Инициализация компонентов интерфейса
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)// Асинхронный обработчик события клика по кнопке
        {
            string city = CityNameTextBox.Text;// Получение текста из текстового поля
            if (!string.IsNullOrWhiteSpace(city)) // Проверка, что поле не пустое и не содержит только пробелы
            {
                await GetWeatherDataAsync(city);// Вызов асинхронного метода для получения данных о погоде
            }
        }

        private async Task GetWeatherDataAsync(string city)// Асинхронный метод для получения данных о погоде
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";// Формирование URL запроса к API

            using (var client = new HttpClient())// Создание экземпляра HttpClient для отправки запросов
            {
                try
                {
                    var response = await client.GetStringAsync(url);// Отправка асинхронного запроса и получение ответа
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(response);//Десериализация JSON ответа в объект

                    TemperatureTextBlock.Text = $"Температура: {weatherData.Main.Temp} °C";//Обновление интерфейса данными о погоде
                    DescriptionTextBlock.Text = $"Описание: {weatherData.Weather[0].Description}";
                    WindSpeedTextBlock.Text = $"Скорость ветра: {weatherData.Wind.Speed} м/с";
                }
                catch (Exception ex)//  исключения
                {
                    MessageBox.Show($"Ошибка при получении данных о погоде: {ex.Message}");
                }
            }
        }
    }

    // Классы для десериализации данных из JSON-ответа API
    public class WeatherData
    {
        public WeatherMain Main { get; set; }
        public Weather[] Weather { get; set; }
        public Wind Wind { get; set; }
    }

    public class WeatherMain
    {
        public double Temp { get; set; } // Температура
    }

    public class Weather
    {
        public string Description { get; set; }//Описание погоды
    }

    public class Wind
    {
        public double Speed { get; set; }// Скорость ветра
    }
}
