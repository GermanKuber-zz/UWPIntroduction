using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;

namespace ReadApp
{
    public static class ReadResitory
    {
        private static List<ReadModel> _readCache;

        public static async Task<List<ReadModel>> GetReadsAsync()
        {

            //Verfico el cache
            if (_readCache != null)
                return _readCache;
            //Busco los datos en la nube
            var client = new HttpClient();
            var stream = await client.GetStreamAsync(
                  "http://beta.json-generator.com/api/json/get/VkuVFeVY-");

            var serializer = new DataContractJsonSerializer(typeof(List<ReadModel>));
            _readCache = (List<ReadModel>)serializer.ReadObject(stream);

           return _readCache;
            
           // return await ReadFromFile();
        }

        public static async Task<List<ReadModel>> ReadFromFile()
        {
            //El archivo data.json debe esta en : ReadApp\bin\x86\Debug\AppX\data.json
            StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///data.json"));
            var file = File.ReadAllText(storageFile.Path);
            var listReturn = JsonConvert.DeserializeObject<List<ReadModel>>(file);
            return listReturn;
        }
    }
}