using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Android : MonoBehaviour
{
    void Awake()
    {
        if (Application.isMobilePlatform)
        {
            Application.targetFrameRate = 120;
        }
    }
    
}
