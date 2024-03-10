using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5 : MonoBehaviour
{
    private Map map;
    [SerializeField] private GameObject harp;
    
    void Start()
    {
        map = GetComponent<Map>();
        Instantiate(harp, map.GetComponent<Map>().BossSpawn);
    }

    
    void Update()
    {
        
    }
}
