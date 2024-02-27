using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator amt;

    private int dummyDirect = 1;
    private bool canFlip = true;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        amt = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flipping()
    {
        if (canFlip)
        {
            dummyDirect *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
    
    public Rigidbody2D Rb
    {
        get => rb;
        set => rb = value;
    }

    public Animator Amt
    {
        get => amt;
        set => amt = value;
    }

    public int DummyDirect
    {
        get => dummyDirect;
        set => dummyDirect = value;
    }

    public bool CanFlip
    {
        get => canFlip;
        set => canFlip = value;
    }
}
