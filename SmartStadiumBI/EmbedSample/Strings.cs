    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAA.IoT
{
    public class Strings
    {
        public static string IotHubHostname = "SoundDataHub.azure-devices.net";
        public static string IotHubSharedKeyName = "iothubowner";
        public static string IotHubSharedKey = "6WwHJVx1Cb6CBQ+NPbrWIZysXOW0848xLasK1n6BgZE=";
        public static string IoTHubConnectionString = string.Format("HostName={0};SharedAccessKeyName={1};SharedAccessKey={2}", IotHubHostname, IotHubSharedKeyName, IotHubSharedKey);
        public static string IotHubD2cEndpoint = "messages/events";

        public static string BLOBStorageAccountName = "gaasounddata";
        public static string BLOBStorageAccountKey = "fOI6pI1UPH8x1ApDlsBIAMmDk1oZqLSc1aiA0vPu2s2f9YtRKgSTlXUV9ecLGJ5HvU+AVH+IZO0d1xvaz1raVw==";
        public static string StorageConnectionstring = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", BLOBStorageAccountName, BLOBStorageAccountKey);

        public static string EventHubSBConnectionString = "sb://iothub-ns-sounddatah-19781-2d43cc2281.servicebus.windows.net/";
        public static string EventHubName = "SmartStadiumHub";
        public static string SBconnectionString = string.Format("Endpoint={0};SharedAccessKeyName={1};SharedAccessKey={2}", EventHubSBConnectionString, IotHubSharedKeyName, IotHubSharedKey);
 
        public static string DocumentDBEndPointUrl = "https://smartstadiumpoc.documents.azure.com:443/";
        public static string DocumentDBPrimaryKey = "uFLuswidTgcodgXzcm0f3kVYwXfhH3xVoYAxHtlsXfcNRE7CJ6uzQZtB4s9LjxZzM8gaD32DyDvFto3PJ8lVEw==";
        public static string DocumentDBDBName = "StadiumData";
        public static string DocumentDBCollectionName = "Data";

        public static string RedisConnectionString = "SmartHVACDemo.redis.cache.windows.net:6380,password=bJSNJOhTAiONUVWMtMhXoFV96Hge+07wazrfQoeNPXQ=,ssl=True,abortConnect=False";
    }
}
