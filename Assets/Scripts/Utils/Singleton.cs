﻿
using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this as T;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
