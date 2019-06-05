using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationHelper : MonoBehaviour
{
    private float _latitude  = 0.0f;
    private float _longitude = 0.0f;
    private static LocationHelper _locationHelper;

    IEnumerator Start()
    {
        _locationHelper = LocationHelper.Instance();

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            print("isEnabledByUser Not");
        }

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            print("Initializing");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            _latitude = Input.location.lastData.latitude;
            _longitude = Input.location.lastData.longitude;
        }
        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    public Vector2 GetLocation()
    {
        return new Vector2(_latitude,_longitude);
    }

    #region LocationHelper Instance
    public static LocationHelper Instance()
    {
        if (!_locationHelper)
        {
            _locationHelper = FindObjectOfType(typeof(LocationHelper)) as LocationHelper;
            if (!_locationHelper)
                Debug.LogError("There needs to be one active LocationHelper script on a GameObject in your scene.");
        }
        return _locationHelper;
    }
    #endregion

}
