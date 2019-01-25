using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    private static GPS _Gps;
    public float Latitude;
    public float Longitude;

    // Use this for initialization
    void Start ()
    {
        _Gps = GPS.Instance();
        //DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
            yield break;
        }
        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait-- > 0) 
            yield return new WaitForSeconds(1);
        if (maxWait <= 0)
        {
            Debug.Log("Initializing Time out ");
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        //success 
        Latitude = Input.location.lastData.latitude;
        Longitude = Input.location.lastData.longitude;
        yield break;
    }

    // Update is called once per frame
	void Update () {
		
	}

    public static GPS Instance()
    {
        if (!_Gps)
        {
            _Gps = FindObjectOfType(typeof(GPS)) as GPS;
            if (!_Gps)
                Debug.LogError("There needs to be one active GPS script on a GameObject in your scene.");
        }
        return _Gps;
    }

}
