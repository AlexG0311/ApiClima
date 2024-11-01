using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ApiClima.Models;
using static ApiClima.Models.WeatherData;

using System.Security.Claims;
using ApiClima.Data;



namespace ApiClima
{
    public partial class MainPage : ContentPage
    {

        private readonly PersonaDataBase _database;
        private string selectedCity;
        private double currentTemperature;
        private string currentDescription;

        private const string ApiKey = "e0bea8c5b38e480f93f150836241504"; // Tu clave API

        public MainPage()
        {
            InitializeComponent();

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WeatherData.db3");
            _database = new PersonaDataBase(dbPath);

        }
        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            string cityName = cityEntry.Text;

            if (string.IsNullOrWhiteSpace(cityName))
            {
                await DisplayAlert("Error", "Por favor ingrese el nombre de la ciudad.", "OK");
                return;
            }

            await LoadWeatherDataAsync(cityName);
        }

        private async Task LoadWeatherDataAsync(string cityName)
        {
            // Construir la URL para el endpoint
            string apiUrl = $"https://api.weatherapi.com/v1/current.json?key={ApiKey}&q={cityName}&aqi=no";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(apiUrl);
                    var weatherData = JsonConvert.DeserializeObject<WeatherApiResponse>(response);

                    // Mostrar información del clima actual
                    cityLabel.Text = $"Ciudad: {weatherData.location.name}"; // Ciudad
                    temperatureLabel.Text = $"{weatherData.current.temp_c} °C"; // Temperatura en grados Celsius
                    selectedCity = cityName ;
                    weatherLabel.Text = weatherData.current.condition.text; // Resumen del clima
                    localtimeLabel.Text = $"Hora: {weatherData.location.localtime}";
                    // Cargar ícono del clima
                    string iconCode = weatherData.current.condition.icon; // Obtener el código del ícono
                    iconImage.Source = new UriImageSource
                    {
                        Uri = new Uri($"https:{iconCode}"), // Cargar el ícono
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(1)
                    };
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener la información de la ciudad: {cityName}.", "OK");
                Console.WriteLine(ex.Message);
            }
        }


       // private async void Insertar(object sender, EventArgs e)
       // {
         //   ClimaModel clima = LlenarPersona();

         //   int result = await App.PersonaDataBase.GuardarPersona(clima);
            //int result = await _apiService.PostAsync<Persona,int>("api/Personas", persona);

           // if (result > 0)
    //        {
        //        await DisplayAlert("Insert", "Exitoo", "Ok");
      //      }

         //  Limpiar();
      //  }




        private async void Insertar(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedCity))
            {
                var climaData = new ClimaModel
                {
                    name = selectedCity,
                 
                };

                await _database.GuardarClimaAsync(climaData);

                await DisplayAlert("Guardado", "El clima de la ciudad ha sido guardado en la base de datos.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Primero obtén el clima de una ciudad antes de guardarlo.", "OK");
            }
        }


    }


}
