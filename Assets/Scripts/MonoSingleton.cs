using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T GetT;
    public static T Instance
    {
        get
        {
            if(GetT == null)
            {
                GetT = (T)GameObject.FindObjectOfType(typeof(T));
            }

            return GetT;
        }
    }
}
