using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasJSON : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

namespace Cameras
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Preset
    {
        public string imageUrl { get; set; }
    }

    public class Camera
    {
        public string name { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public List<Preset> presets { get; set; }
    }

    public class Data
    {
        public List<Camera> cameras { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }

}

namespace TMS
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class SensorValue
    {
        public string name { get; set; }
        public int sensorValue { get; set; }
    }

    public class TmsStation
    {
        public string tmsStationId { get; set; }
        public string name { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public List<SensorValue> sensorValues { get; set; }
    }

    public class Data
    {
        public List<TmsStation> tmsStations { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }


}

namespace Weather
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class SensorValue
    {
        public string name { get; set; }
        public double sensorValue { get; set; }
    }

    public class WeatherStation
    {
        public string name { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public List<SensorValue> sensorValues { get; set; }
    }

    public class Data
    {
        public List<WeatherStation> weatherStations { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }


}
