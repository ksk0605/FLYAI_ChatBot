using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTHubClient
{
    internal class DummySensor
    {
        WeatherModel weatherModel = new WeatherModel();
        private Random random = new Random();

        public WeatherModel GetWeatherModel(string deviceID)
        {
            // 온도, 습도, 먼지농도 설정
            weatherModel.DeviceID = deviceID;
            weatherModel.Temperature = random.Next(25, 32);
            weatherModel.Humidity = random.Next(60, 80);
            weatherModel.Dust = 50 + weatherModel.Temperature + random.Next(1, 5);          
            return weatherModel;
        }
    }
}
