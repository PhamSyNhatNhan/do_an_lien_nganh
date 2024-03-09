using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager_ : MonoBehaviour
{
    private Transform originWorld;
    
    
    [SerializeField] private List<GameObject> playerManager;
    [SerializeField] private List<GameObject> enemyManager;
    
    [Header("Level Manager")]
    [SerializeField] private List<GameObject> levelManager;
    private GameObject curLevel;
    private int curLevelCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        originWorld = transform;

        /*
        GameObject newlevel = Instantiate(levelManager[curLevelCount], originWorld);
        newlevel.transform.parent = null;
        curLevel = newlevel;
        */
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    private void checkInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            nextLevel();
        }
    }

    private void nextLevel()
    {
        if(curLevelCount < levelManager.Count - 1)
        {
            curLevelCount += 1;
            Destroy(curLevel);
            GameObject newlevel = Instantiate(levelManager[curLevelCount], originWorld);
            newlevel.transform.parent = null;
            
            curLevel = newlevel;  
        }
        
    }

    public void DisablePlayer(GameObject Player, float time)
    {
        Player.layer = 8;
        Player.GetComponent<SpriteRenderer>().sortingLayerName = "Default";

        StartCoroutine(EnablePlayer(Player, time));
    }

    IEnumerator EnablePlayer(GameObject Player, float time)
    {
        yield return new WaitForSeconds(time);
        
        Player.layer = 6;
        Player.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
    }
}
