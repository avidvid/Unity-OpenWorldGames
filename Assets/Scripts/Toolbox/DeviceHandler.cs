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
            string unicastAddresse = "";
            foreach (var addrInfo in properties.UnicastAddresses)
                unicastAddresse += "(" + addrInfo.Address.AddressFamily + ")" + addrInfo.Address.ScopeId + ",\t";
            print(cnt++ + "-"
                        + " \nDescription .......................... :" + adapter.Description
                        + " \nInterface type .......................... :" + adapter.NetworkInterfaceType
                        + " \nOperational Status .......................... :" + adapter.OperationalStatus
                        + " \nPhysical Address ........................ :" + adapter.GetPhysicalAddress()
                        + " \nUnicast Addresse ........................ :" + unicastAddresse
                        + " \nIs receive only.......................... :" + adapter.IsReceiveOnly
                        + " \nMulticast................................ :" + adapter.SupportsMulticast);
        }
    }
    public static void DisplayTypeAndAddress2()
    {
        try
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var netInterface in interfaces)
            {
                if ((netInterface.OperationalStatus == OperationalStatus.Up ||
                     netInterface.OperationalStatus == OperationalStatus.Unknown) &&
                    (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                     netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            var ipAddress = addrInfo.Address;
                            print(ipAddress);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

}
