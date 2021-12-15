using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphQlClient.Core;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using Cameras;
using TMS;
using Weather;

public class GraphQLTest : MonoBehaviour
{
    public GraphApi Test;
    public float timeInterval; // interval in seconds
    float actualTime;
    [SerializeField]
    AbstractMap _map;

    Vector2d[] _locationsTMS;
    Vector2d[] _locationsWeather;

    [SerializeField]
    float _spawnScale = 100f;

    [SerializeField]
    GameObject _markerPrefab;

    public List<GameObject> _spawnedTMSObjects;
    public List<GameObject> _spawnedWeatherObjects;

    public List<Cameras.Camera> cameras;
    public List<TmsStation> tmsStations;
    public List<WeatherStation> weatherStations;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
        timeInterval = 60.0f; // set interval for 5 minutes
        actualTime = timeInterval; // set actual time left until next update
    }

    // Update is called once per frame
    void Update()
    {
        actualTime -= Time.deltaTime; // subtract the time taken to render last frame

        if (actualTime <= 0) // if time runs out, do your update
        {
            // INSERT YOUR UPDATE CODE HERE
            GetAll();
            actualTime = timeInterval; // reset the timer
        }

        UpdateMarkerPositions();
    }

    public async void Setup()
    {
        /*GraphApi.Query query = Test.GetQueryByName("GetAllCameras", GraphApi.Query.Type.Query);
        UnityWebRequest request = await Test.Post(query);
        Debug.LogError(HttpHandler.FormatJson(request.downloadHandler.text));
        Root results = JsonConvert.DeserializeObject<Root>(HttpHandler.FormatJson(request.downloadHandler.text));
        cameras = results.data.cameras;*/
        GraphApi.Query query = Test.GetQueryByName("GetAllTmsStations", GraphApi.Query.Type.Query);
        UnityWebRequest request = await Test.Post(query);
        Debug.LogError(HttpHandler.FormatJson(request.downloadHandler.text));
        TMS.Root results = JsonConvert.DeserializeObject<TMS.Root>(HttpHandler.FormatJson(request.downloadHandler.text));
        tmsStations = results.data.tmsStations;

        GraphApi.Query query2 = Test.GetQueryByName("GetAllWeatherStations", GraphApi.Query.Type.Query);
        UnityWebRequest request2 = await Test.Post(query2);
        Debug.LogError(HttpHandler.FormatJson(request2.downloadHandler.text));
        Weather.Root results2 = JsonConvert.DeserializeObject<Weather.Root>(HttpHandler.FormatJson(request2.downloadHandler.text));
        weatherStations = results2.data.weatherStations;

        CreateAllMarkers();
    }

    public void GetAll()
    {
        GetCameras();
        GetTMS();
        getWeather();
    }

    public async void GetCameras()
    {
        GraphApi.Query query = Test.GetQueryByName("GetAllCameras", GraphApi.Query.Type.Query);
        UnityWebRequest request = await Test.Post(query);
        Debug.LogError(HttpHandler.FormatJson(request.downloadHandler.text));
        Cameras.Root results = JsonConvert.DeserializeObject<Cameras.Root>(HttpHandler.FormatJson(request.downloadHandler.text));
        cameras = results.data.cameras;
    }

    public async void GetTMS()
    {
        GraphApi.Query query = Test.GetQueryByName("GetAllTmsStations", GraphApi.Query.Type.Query);
        UnityWebRequest request = await Test.Post(query);
        Debug.LogError(HttpHandler.FormatJson(request.downloadHandler.text));
        TMS.Root results = JsonConvert.DeserializeObject<TMS.Root>(HttpHandler.FormatJson(request.downloadHandler.text));
        tmsStations = results.data.tmsStations;
    }

    public async void getWeather()
    {
        GraphApi.Query query = Test.GetQueryByName("GetAllWeatherStations", GraphApi.Query.Type.Query);
        UnityWebRequest request = await Test.Post(query);
        Debug.LogError(HttpHandler.FormatJson(request.downloadHandler.text));
        Weather.Root results = JsonConvert.DeserializeObject<Weather.Root>(HttpHandler.FormatJson(request.downloadHandler.text));
        weatherStations = results.data.weatherStations;
    }

    public void CreateAllMarkers()
    {
        CreateTMSMarkers();
        CreateWeatherMarkers();
    }

    public void CreateTMSMarkers()
    {
        _locationsTMS = new Vector2d[tmsStations.Count];
        _spawnedTMSObjects = new List<GameObject>();
        for (int i = 0; i < tmsStations.Count; i++)
        {
            var locationString = tmsStations[i].lat + ", " + tmsStations[i].lon;
            _locationsTMS[i] = Conversions.StringToLatLon(locationString);
            var instance = Instantiate(_markerPrefab);
            instance.transform.localPosition = _map.GeoToWorldPosition(_locationsTMS[i], true);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            _spawnedTMSObjects.Add(instance);

            BoxCollider boxCollider = instance.AddComponent<BoxCollider>();
            // Add and configure the touchable
            var touchable = instance.AddComponent<NearInteractionTouchableVolume>();
            touchable.EventsToReceive = TouchableEventType.Pointer;

            //TextMeshPro textmeshPro = new TextMeshPro();
            string text = "";
            for (int j = 0; j < tmsStations[i].sensorValues.Count; j++)
            {
                if (tmsStations[i].sensorValues[j].name == "KESKINOPEUS_60MIN_KIINTEA_SUUNTA2")
                {
                    text = text + " AVG Speed 60min Route1: " + tmsStations[i].sensorValues[j].sensorValue + " km/h";
                } else if (tmsStations[i].sensorValues[j].name == "KESKINOPEUS_60MIN_KIINTEA_SUUNTA1")
                {
                    text = text + " AVG Speed 60min Route2: " + tmsStations[i].sensorValues[j].sensorValue + " km/h";
                }
            }
            //textmeshPro.text = text;
            //textmeshPro.characterSize = 1;

            //Create new GameObject
            GameObject childObj = new GameObject();

            //Make block to be parent of this gameobject
            childObj.transform.parent = instance.transform;
            childObj.name = "Text Holder";

            //Create TextMesh and modify its properties
            TextMesh textMesh = childObj.AddComponent<TextMesh>();
            textMesh.text = text;
            textMesh.characterSize = 0.2f;

            //Set postion of the TextMesh with offset
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y + 0.5f, instance.transform.position.z);

            //childObj.active = false;
            childObj.transform.localScale = new Vector3(0, 0, 0);

            // Change color on pointer down and up
            var pointerHandler = instance.AddComponent<PointerHandler>();
            //pointerHandler.OnPointerDown.AddListener((e) => Debug.LogError("HELPHELP"));
            //pointerHandler.OnPointerUp.AddListener((e) => Debug.LogError("PLEHPLEH"));
            pointerHandler.OnPointerDown.AddListener((e) => e.Pointer.Result.CurrentPointerTarget.transform.GetChild(0).localScale = new Vector3(1, 1, 1));
            pointerHandler.OnPointerUp.AddListener((e) => e.Pointer.Result.CurrentPointerTarget.transform.GetChild(0).localScale = new Vector3(0, 0, 0));
        }
    }

    public void CreateWeatherMarkers()
    {
        _locationsWeather = new Vector2d[weatherStations.Count];
        _spawnedWeatherObjects = new List<GameObject>();
        for (int i = 0; i < weatherStations.Count; i++)
        {
            var locationString = weatherStations[i].lat + ", " + weatherStations[i].lon;
            _locationsWeather[i] = Conversions.StringToLatLon(locationString);
            var instance = Instantiate(_markerPrefab);
            instance.transform.localPosition = _map.GeoToWorldPosition(_locationsWeather[i], true);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            _spawnedWeatherObjects.Add(instance);

            BoxCollider boxCollider = instance.AddComponent<BoxCollider>();
            // Add and configure the touchable
            var touchable = instance.AddComponent<NearInteractionTouchableVolume>();
            touchable.EventsToReceive = TouchableEventType.Pointer;

            //TextMeshPro textmeshPro = new TextMeshPro();
            string text = "";
            for (int j = 0; j < weatherStations[i].sensorValues.Count; j++)
            {
                if (weatherStations[i].sensorValues[j].name == "ILMA")
                {
                    text = text + " Temperature: " + weatherStations[i].sensorValues[j].sensorValue + " C";
                }
                else if (weatherStations[i].sensorValues[j].name == "KESKITUULI")
                {
                    text = text + " Wind speed: " + weatherStations[i].sensorValues[j].sensorValue + " m/s";
                }
            }
            //textmeshPro.text = text;
            //textmeshPro.characterSize = 1;

            //Create new GameObject
            GameObject childObj = new GameObject();

            //Make block to be parent of this gameobject
            childObj.transform.parent = instance.transform;
            childObj.name = "Text Holder";

            //Create TextMesh and modify its properties
            TextMesh textMesh = childObj.AddComponent<TextMesh>();
            textMesh.text = text;
            textMesh.characterSize = 0.2f;

            //Set postion of the TextMesh with offset
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y + 0.5f, instance.transform.position.z);

            //childObj.active = false;
            childObj.transform.localScale = new Vector3(0, 0, 0);

            // Change color on pointer down and up
            var pointerHandler = instance.AddComponent<PointerHandler>();
            //pointerHandler.OnPointerDown.AddListener((e) => Debug.LogError("HELPHELP"));
            //pointerHandler.OnPointerUp.AddListener((e) => Debug.LogError("PLEHPLEH"));
            pointerHandler.OnPointerDown.AddListener((e) => e.Pointer.Result.CurrentPointerTarget.transform.GetChild(0).localScale = new Vector3(1, 1, 1));
            pointerHandler.OnPointerUp.AddListener((e) => e.Pointer.Result.CurrentPointerTarget.transform.GetChild(0).localScale = new Vector3(0, 0, 0));
        }
    }

    public void UpdateMarkerPositions()
    {
        int count = _spawnedTMSObjects.Count;
        for (int i = 0; i < count; i++)
        {
            var spawnedObject = _spawnedTMSObjects[i];
            var location = _locationsTMS[i];
            spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
            spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        }

        int count2 = _spawnedWeatherObjects.Count;
        for (int i = 0; i < count2; i++)
        {
            var spawnedObject = _spawnedWeatherObjects[i];
            var location = _locationsWeather[i];
            spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
            spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        }
    }
}
