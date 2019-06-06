using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TestScripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(FetchMacId());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public string FetchMacId()
    {
        string macAddresses = "";

        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.OperationalStatus == OperationalStatus.Up)
            {
                macAddresses += nic.GetPhysicalAddress().ToString();
                break;
            }
        }
        return macAddresses;
    }

}
