using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class RandomHelper
{
    public static bool TrueFalse(Vector2 location, int key)
    {
        return Range(location, key, int.MaxValue)>int.MaxValue/2;
    }

    public static int Range(Vector3 location, int key, float range)
    {
        return Range(location.x, location.y, key, (int)range);
    }

    public static int Range(Vector2 location, int key, float range)
    {
        return Range(location.x, location.y, key, (int) range);
    }

    public static int Range(float x, float y, int key, int range)
    {
        return Range((int)x, (int)y, key, range);
    }
    public static int Range(int key, int range)
    {
        return Range(0,0, key, range);
    }
    public static int Range(int x, int y, int key, int range)
    {
        uint hash = (uint)key;
        hash ^= (uint)x;
        hash *= 0x51d7348d;
        hash ^= 0x85dbdda2;
        hash = (hash << 16) ^ (hash >> 16);
        hash *= 0x7588f287;
        hash ^= (uint)y;
        hash *= 0x487a5559;
        hash ^= 0x64887219;
        hash = (hash << 16) ^ (hash >> 16);
        hash *= 0x63288691;
        return (int)(hash % range);
    }
    public static float Percent(Vector3 location, int key)
    {
        return Percent((int)location.x, (int)location.y, key);
    }
    public static float Percent(int x, int y, int key)
    {
        return (float)Range(x, y, key,int.MaxValue) / (float)int.MaxValue;
    }
    public static float Percent(float x, float y, int key)
    {
        return Percent((int) x, (int) y, key);
    }
    public static float CriticalRange(float max)
    {
        if (max<=1)
            return 0;
        //x7 times 20% of the times
        var criticalValue = UnityEngine.Random.Range(0, 10) > 8 ? 7 : 1; 
        var result = UnityEngine.Random.Range(1, max) * criticalValue;
        return result;
    }
    public static bool GetLucky(Vector3 pos, float chance)
    {
        //Debug.Log("chance = " + chance + " YourChance= " + RandomHelper.Percent(pos, 1) + pos);
        if (chance >= 1f || chance > RandomHelper.Percent(pos, 1))
            return true;
        return false;
    }
    internal static int RangeMinMax(int v1, int v2)
    {
        return UnityEngine.Random.Range(v1, v2);
    }
    internal static int StringToRandomNumber(string myString, int range=20)
    {
        MD5 md5Hasher = MD5.Create();
        var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(myString));
        var iValue = Math.Abs(BitConverter.ToInt32(hashed, 0))% range;
        return iValue;
    }
    internal static float AbsZero(float input)
    {
        return input > 0 ? input : 0;
    }
}