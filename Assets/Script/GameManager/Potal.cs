using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    private GameManager_ gm;
    private UiController uc;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager_>();
        uc = GameObject.Find("Canvas").GetComponent<UiController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            uc.setActiveInteract(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            uc.setActiveInteract(false);
        }
    }

}
