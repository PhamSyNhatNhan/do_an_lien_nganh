using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private GameObject portalObject;
    [SerializeField] private Transform portalPosition1;
    [SerializeField] private Transform portalPosition2;
    [SerializeField] private Transform portalPosition3;
    [SerializeField] private Transform playerPosition;
    private string typeMap;
    
    [Header("Position 1")]
    [SerializeField] private Transform monsterSpawn1;
    [SerializeField] private float monsterSpawnRadius1;
    
    [Header("Position 2")]
    [SerializeField] private Transform monsterSpawn2;
    [SerializeField] private float monsterSpawnRadius2;
    
    [Header("Position 3")]
    [SerializeField] private Transform monsterSpawn3;
    [SerializeField] private float monsterSpawnRadius3;
    
    [Header("Position 4")]
    [SerializeField] private Transform monsterSpawn4;
    [SerializeField] private float monsterSpawnRadius4;
    
    [Header("Position Boss")]
    [SerializeField] private Transform bossSpawn;
    
    

    public void endLevel()
    {
        Instantiate(portalObject, portalPosition1);
        Instantiate(portalObject, portalPosition2);
        Instantiate(portalObject, portalPosition3);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(monsterSpawn1.position, monsterSpawnRadius1);
        Gizmos.DrawWireSphere(monsterSpawn2.position, monsterSpawnRadius2);
        Gizmos.DrawWireSphere(monsterSpawn3.position, monsterSpawnRadius3);
        Gizmos.DrawWireSphere(monsterSpawn4.position, monsterSpawnRadius4);
    }

    public GameObject PortalObject
    {
        get => portalObject;
        set => portalObject = value;
    }

    public Transform PortalPosition1
    {
        get => portalPosition1;
        set => portalPosition1 = value;
    }

    public Transform PortalPosition2
    {
        get => portalPosition2;
        set => portalPosition2 = value;
    }

    public Transform PortalPosition3
    {
        get => portalPosition3;
        set => portalPosition3 = value;
    }

    public Transform PlayerPosition
    {
        get => playerPosition;
        set => playerPosition = value;
    }

    public Transform MonsterSpawn1
    {
        get => monsterSpawn1;
        set => monsterSpawn1 = value;
    }

    public float MonsterSpawnRadius1
    {
        get => monsterSpawnRadius1;
        set => monsterSpawnRadius1 = value;
    }

    public Transform MonsterSpawn2
    {
        get => monsterSpawn2;
        set => monsterSpawn2 = value;
    }

    public float MonsterSpawnRadius2
    {
        get => monsterSpawnRadius2;
        set => monsterSpawnRadius2 = value;
    }

    public Transform MonsterSpawn3
    {
        get => monsterSpawn3;
        set => monsterSpawn3 = value;
    }

    public float MonsterSpawnRadius3
    {
        get => monsterSpawnRadius3;
        set => monsterSpawnRadius3 = value;
    }

    public Transform MonsterSpawn4
    {
        get => monsterSpawn4;
        set => monsterSpawn4 = value;
    }

    public float MonsterSpawnRadius4
    {
        get => monsterSpawnRadius4;
        set => monsterSpawnRadius4 = value;
    }

    public Transform BossSpawn
    {
        get => bossSpawn;
        set => bossSpawn = value;
    }

    public string TypeMap
    {
        get => typeMap;
        set => typeMap = value;
    }
}
