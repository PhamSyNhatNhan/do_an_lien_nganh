using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager_ : MonoBehaviour
{
    private Transform originWorld;
    private UiController uc;
    
    [SerializeField] private List<GameObject> playerManager;
    [SerializeField] private List<GameObject> enemyManager;
    
    [Header("Level Manager")]
    [SerializeField] private List<GameObject> levelManager;
    private GameObject curLevel;
    private int curLevelCount = -1;
    
    [Header("Player")]
    private GameObject curPlayer;
    
    void Start()
    {
        originWorld = transform;
        curPlayer = GameObject.Find("Player");
        uc = GameObject.Find("Canvas").GetComponent<UiController>();
        
        nextLevel();
    }
    
    void Update()
    {
        interactLevel();
    }

    private void interactLevel()
    {
        if (Input.GetKeyDown(KeyCode.F) && uc.StatusInteractUi)
        {
            nextLevel();
        }
    }

    
    public void nextLevel()
    {
        if(curLevelCount < levelManager.Count - 1)
        {
            curLevelCount += 1;
            if(curLevel != null) Destroy(curLevel);
            GameObject newlevel = Instantiate(levelManager[curLevelCount], originWorld);
            newlevel.transform.parent = null;
            
            curLevel = newlevel;  
        }
        
        if (curLevel == null || curLevel.GetComponent<Map>() == null || curLevel.GetComponent<Map>().PlayerPosition == null) {
            Debug.LogError("Some objects are not properly initialized.");
        }

        curPlayer.transform.position = curLevel.GetComponent<Map>().PlayerPosition.position;
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
