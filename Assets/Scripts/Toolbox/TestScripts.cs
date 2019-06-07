using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TestScripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DeviceHandler.DisplayTypeAndAddress();
        print(DeviceHandler.FetchMacId());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
