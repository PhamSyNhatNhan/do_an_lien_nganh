using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0 : MonoBehaviour
{
    private Map map;
    
    
    void Start()
    {
        map = GetComponent<Map>();
        map.endLevel();
    }

    
    void Update()
    {
        
    }
}
