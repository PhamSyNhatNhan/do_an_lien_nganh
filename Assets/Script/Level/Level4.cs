using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4 : MonoBehaviour
{
    private Map map;
    private UiController uc;
    private GameManager_ gm;

    [SerializeField] private GameObject boss;
    private GameObject curBoss;
    
    private bool endLevel = false;
    private bool canUpdate = false;
    
    void Start()
    {
        map = GetComponent<Map>();
        uc = GameObject.Find("Canvas").GetComponent<UiController>();
        
        StartCoroutine(uc.countDownScene());
        StartCoroutine(starLevel());
        gm.updateEnemy(new List<GameObject>() {boss});
    }
    
    IEnumerator starLevel()
    {
        yield return new WaitForSeconds(3.0f);

        curBoss = Instantiate(boss, map.BossSpawn);
        curBoss.transform.parent = null;
        canUpdate = true;
    }
    
    void Update()
    {
        if (curBoss == null && !endLevel && canUpdate)
        {
            endLevel = true;
            map.endLevel();
        }
    }


}
