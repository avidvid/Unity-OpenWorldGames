using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class DeviceHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static string FetchMacId()
    {
        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.OperationalStatus == OperationalStatus.Up)
            {
                var macAddresses = nic.GetPhysicalAddress().ToString();
                if (macAddresses != "")
                    return macAddresses;
            }
        }
        throw new Exception("No Device Id!!!");
    }
    public static void DisplayTypeAndAddress()
    {
        int cnt = 1;
        foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
        {
            var properties = adapter.GetIPProperties();
            print(cnt++ + "-"
                        + " \nDescription .......................... :" + adapter.Description
                        + " \nInterface type .......................... :" + adapter.NetworkInterfaceType
                        + " \nOperational Status .......................... :" + adapter.OperationalStatus
                        + " \nPhysical Address ........................ :" + adapter.GetPhysicalAddress()
                        + " \nIs receive only.......................... :" + adapter.IsReceiveOnly
                        + " \nMulticast................................ :" + adapter.SupportsMulticast);
        }
    }
}
