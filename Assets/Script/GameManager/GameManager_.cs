using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager_ : MonoBehaviour
{
    private Transform originWorld;
    private UiController uc;
    private CinemachineVirtualCamera virtualCamera;
    
    [SerializeField] private List<GameObject> playerManager;
    
    [SerializeField] private List<GameObject> enemyManager;
    [SerializeField] private List<GameObject> enemyCurentManager = new List<GameObject>();
    
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
        Time.timeScale = 0.0f;
        uc.MainMenu.SetActive(true);
        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
    }
    
    void Update()
    {
        CheckInput();
    }

    public void godmodeEnable()
    {
        curPlayer.GetComponent<PlayerHealth>().enterGodmode();
    }

    public void updateEnemy(List<GameObject> enemy)
    {
        enemyCurentManager = enemy;
    }
    
    private void DestroyAllEnemies()
    {
        foreach (GameObject enemy in enemyCurentManager)
        {
            Destroy(enemy);
        }
        
        enemyCurentManager.Clear();
    }

    public void winGame()
    {
        Destroy(curPlayer);
        uc.WinMenu.SetActive(false);
        uc.MainMenu.SetActive(true);
    }
    
    public void mainGame()
    {
        uc.PauseMenu.SetActive(false);
        uc.MainMenu.SetActive(true);
    }

    public void exitGame()
    {
        Application.Quit();
    }
    
    public void newGame()
    {
        DestroyAllEnemies();
        Time.timeScale = 1.0f;
        uc.MainMenu.SetActive(false);
        if(curPlayer != null) Destroy(curPlayer);

        curPlayer = Instantiate(playerManager[0], originWorld);
        curPlayer.transform.parent = null;
        curPlayer.name = "Player";
        
        if (virtualCamera != null && curPlayer != null)
        {
            virtualCamera.Follow = curPlayer.transform;
            virtualCamera.LookAt = curPlayer.transform;
        }
        
        curLevelCount = -1;
        nextLevel();
    }

    public void pauseGame()
    {
        Time.timeScale = 0.0f;
        uc.PauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        uc.PauseMenu.SetActive(false);
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && uc.StatusInteractUi)
        {
            nextLevel();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame();
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
            curPlayer.transform.position = curLevel.GetComponent<Map>().PlayerPosition.position;  
        }

        if (curLevelCount >= levelManager.Count - 1)
        {
            Time.timeScale = 0.0f;
            uc.WinMenu.SetActive(true);
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
