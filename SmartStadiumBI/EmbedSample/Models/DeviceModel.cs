using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GAA.IoT.Models
{
    public class DeviceModel
    {
        /// <summary>
        /// The device id used to identify a device
        /// </summary>
        public string DeviceId { get; set; }

        public string DeviceName { get; set; }
        public string ShortTitle { get; set; }

        public string Stand { get; set; }

        public string Description { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public int SortIndex { get; set; }
    }

    public class DeviceFactory
    {
        private List<DeviceModel> devices;

        /// <summary>
        /// This is a singleton so hide the constructor
        /// </summary>
        private DeviceFactory()
        {
            devices = new List<DeviceModel>();

            try
            {
                var deviceList = new List<DeviceModel>();

                // get the azure blob devices.json from smarthvacstorage/referencedata
                string cn = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", Strings.BLOBStorageAccountName, Strings.BLOBStorageAccountKey);

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(cn);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container.
                CloudBlobContainer container = blobClient.GetContainerReference("referencedata");

                // download json
                System.IO.MemoryStream s = new System.IO.MemoryStream();
                container.GetBlobReference("devices.json").DownloadToStream(s);
                string json = Encoding.UTF8.GetString(s.ToArray());


                // desrialise the json
                deviceList.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeviceModel>>(json));

                this.devices = deviceList.OrderBy(x => x.SortIndex).ToList();

            }
            catch( Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(  ex.GetText(true) );
            }
           
        }

        public List<DeviceModel> Devices
        {
            get
            {
                return devices.OrderBy(o=>o.SortIndex).ToList();
            }
        }

        private static DeviceFactory instance;

        public static DeviceFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeviceFactory();
                }
                return instance;
            }
        }

    }
}
