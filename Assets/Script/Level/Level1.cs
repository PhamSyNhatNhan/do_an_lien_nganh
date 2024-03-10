using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level1 : MonoBehaviour
{
    private Map map;
    private UiController uc;
    private GameManager_ gm;
    
    [SerializeField] private List<GameObject> enemyManager = new List<GameObject>();
    private List<GameObject> curEnemyManager = new List<GameObject>();
    private int curWave = 1;
    
    private bool endLevel = false;
    private bool canWave = false;

    void Start()
    {
        map = GetComponent<Map>();
        uc = GameObject.Find("Canvas").GetComponent<UiController>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager_>();
        
        StartCoroutine(uc.countDownScene());
        StartCoroutine(starLevel());
    }

    IEnumerator starLevel()
    {
        yield return new WaitForSeconds(3.0f);
        
        canWave = true;
    }
    
    void Update()
    {
        checkEnemyManager();
        checkWave();
        gm.updateEnemy(curEnemyManager);
    }

    private void checkEnemyManager()
    {
        for (int i = curEnemyManager.Count - 1; i >= 0; i--)
        {
            if (curEnemyManager[i] == null)
            {
                curEnemyManager.RemoveAt(i);
            }
        }
    }

    private void checkWave()
    {
        if(!canWave) return;
        
        if (curEnemyManager.Count == 0 && curWave < 3)
        {
            curWave += 1;
            subSpawnEnemy();
        }
        else if(curEnemyManager.Count == 0 && curWave == 3 && !endLevel)
        {
            endLevel = true;
            map.endLevel();
        }
    }

    private void subSpawnEnemy()
    {
        int random = Random.Range(0, 3);
        spawnEnemy(map.MonsterSpawn1, map.MonsterSpawnRadius1, random);
        
        random = Random.Range(0, 3);
        spawnEnemy(map.MonsterSpawn2, map.MonsterSpawnRadius2, random);
        
        random = Random.Range(0, 3);
        spawnEnemy(map.MonsterSpawn3, map.MonsterSpawnRadius3, random);
        
        random = Random.Range(0, 3);
        spawnEnemy(map.MonsterSpawn4, map.MonsterSpawnRadius4, random);
    }
    

    private void spawnEnemy(Transform position, float radius, int numberEnemy)
    {
        for (int i = 0; i < numberEnemy; i++)
        {
            float randomX = Random.Range(-radius, radius);
            float randomY = Random.Range(-radius, radius);
            Vector2 randomPosition = new Vector2(position.position.x + randomX, position.position.y + randomY);
            
            int randomEnemy = Random.Range(0, enemyManager.Count);
            GameObject enemy = Instantiate(enemyManager[randomEnemy], randomPosition, Quaternion.identity);
            enemy.transform.parent = null;
            curEnemyManager.Add(enemy);
        }
    }
    
}
