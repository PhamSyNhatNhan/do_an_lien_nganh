using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_ : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
