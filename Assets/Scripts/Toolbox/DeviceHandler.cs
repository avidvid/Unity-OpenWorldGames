using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
            var macAddresses = nic.GetPhysicalAddress().ToString();
            if (macAddresses != "")
                return macAddresses;
        }
        throw new Exception("No Device Id!!!");
    }
    public static void DisplayTypeAndAddress()
    {
        int cnt = 1;
        foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
        {
            var properties = adapter.GetIPProperties();
            string unicastAddresse = "";
            foreach (var addrInfo in properties.UnicastAddresses)
                unicastAddresse += "(" + addrInfo.Address.AddressFamily + ")";
            print(cnt++ + "-"
                        + " \nDescription .......................... :" + adapter.Description
                        + " \nInterface type .......................... :" + adapter.NetworkInterfaceType
                        + " \nSpeed .......................... :" + adapter.Speed
                        + " \nOperational Status .......................... :" + adapter.OperationalStatus
                        + " \nPhysical Address ........................ :" + adapter.GetPhysicalAddress()
                        + " \nUnicast Addresse ........................ :" + unicastAddresse
                        + " \nIs receive only.......................... :" + adapter.IsReceiveOnly
                        + " \nMulticast................................ :" + adapter.SupportsMulticast
                        + " \nIs receive only.......................... :" + adapter.Name
                        + " \nGetIPv4Properties.......................... :" + adapter.GetIPv4Statistics());
        }
    }
}