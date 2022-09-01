/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Timers;
using Microsoft.Azure.Devices.Client;


namespace IoTHubClient
{
    internal class Program
    {
        private static System.Timers.Timer SensorTimer;


        private static DeviceClient SensorDevice = null;
        private const string DeviceID = "Device1";
        private const string DeviceConnectionString = "HostName=labuser9iothub.azure-devices.net;DeviceId=Device1;SharedAccessKey=DcpIctl/cpAbmG0w5V9W3uPGNzxNIb74T35KzSvRENw=";
        private static DummySensor Sensor = new DummySensor();
        
        
        static void Main(string[] args)
        {
            Console.WriteLine("\nPress the Enter Key to exit the application...");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);

            SensorDevice = DeviceClient.CreateFromConnectionString(DeviceConnectionString); //, DeviceID);
            
            
            if (SensorDevice == null)
            {
                Console.WriteLine("Failed to Create DeviceClient!");
            }
            else
            {
                // 연결이 되면 Timer 호출
                SetTimer();
            }
              
            
            
            Console.ReadLine();
            SensorTimer.Stop(); // Timer 멈추고
            SensorTimer.Dispose(); // Sensor 메모리해제
        }

        private static void SetTimer()
        {
            SensorTimer = new System.Timers.Timer(2000); // 2초마다 이벤트를 만드는 센서
            SensorTimer.Elapsed += SensorTimer_Elapsed;
            SensorTimer.Enabled = true;
            SensorTimer.AutoReset = true;
        }

        private static async void SensorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //2초에 한번씩 작동
            Console.WriteLine("Event!!! ----> {0:HH:mm:ss.fff}", e.SignalTime);
            await SendEvent();
        }

        private static async Task SendEvent()
        {
            String json = Newtonsoft.Json.JsonConvert.SerializeObject(Sensor.GetWeatherModel(DeviceID).ToString());
            Console.WriteLine(json); // 정적 메소드 안에는 정적 객체가 있어야 함
            Message eventMessage = new Message(Encoding.UTF8.GetBytes(json));
            await SensorDevice.SendEventAsync(eventMessage);
        }
    }
}*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Timers;
using Microsoft.Azure.Devices.Client;

namespace IoTHubClient
{
    internal class Program
    {
        private static System.Timers.Timer SensorTimer;

        private static DeviceClient SensorDevice = null;
        private const string DeviceID = "Device1";
        private const string DeviceConnectionString = "HostName=labuser9iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=z/Jo31G1HEGGAgGAG7RtvTqndRMEndMgG9jWLdqvj7A=";
        private static DummySensor Sensor = new DummySensor();

        static void Main(string[] args)
        {
            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);

            SensorDevice = DeviceClient.CreateFromConnectionString(DeviceConnectionString, DeviceID);

            if (SensorDevice == null)
            {
                Console.WriteLine("Failed to create DeviceClient!");
            }
            else
            {
                SetTimer();
            }

            Console.ReadLine();
            SensorTimer.Stop();
            SensorTimer.Dispose();
        }

        private static void SetTimer()
        {
            SensorTimer = new System.Timers.Timer(2000);
            SensorTimer.Elapsed += SensorTimer_Elapsed;
            SensorTimer.Enabled = true;
            SensorTimer.AutoReset = true;
        }

        private static async void SensorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Event!!! ——> {0:HH:mm:ss.fff}", e.SignalTime);
            await SendEvent();
        }

        private static async Task SendEvent()
        {
            String json = Newtonsoft.Json.JsonConvert.SerializeObject(Sensor.GetWeatherModel(DeviceID));
             // 정적 메소드 안에는 정적 객체가 있어야 함
            Message eventMessage = new Message(Encoding.UTF8.GetBytes(json));
   
            await SensorDevice.SendEventAsync(eventMessage);
            Console.WriteLine(json);
        }
    }
}

