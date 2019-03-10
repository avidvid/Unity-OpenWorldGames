using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapHelper 
{
    public static void Swap(ref Vector3 item1, ref Vector3 item2)
    {
        Vector3 temp = item1;
        item1 = item2;
        item2 = temp;
    }
}
