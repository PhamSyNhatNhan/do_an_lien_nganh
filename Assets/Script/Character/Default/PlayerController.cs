using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator amt;
    private Controller controller;
    private Rigidbody2D rb;
    private PlayerStat ps;

    private bool canInput = true;
    
    [Header("Movement")]
    private bool isMove = false;
    private Vector2 moveVector;
    private float inputDirect;
    private bool canMove = true;
    private Vector2 moveSnap = Vector2.right;
    
    [Header("Flipping")]
    private bool canFlip = true;
    private int flipDirect = 1;
    
    [Header("Dash")] 
    private bool isDash = false;
    private bool canDash = true;
    private Coroutine flipCoroutine;
    
    private void Awake()
    {
        controller = new Controller();
    }

    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        amt = GetComponent<Animator>();
        ps = GetComponent<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckFlipping();
        AnimationControl();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public Controller Controller => controller;

    private void CheckInput()
    {
        if (canInput == false) return;
        
        moveVector = controller.Player.Move.ReadValue<Vector2>();
        inputDirect = moveVector.x;
        //Debug.Log(moveVector);
    }

    private void Movement()
    {
        if (isDash) return;
            
        //Debug.Log("Movement");
        if (moveVector != Vector2.zero && canMove && !isDash)
        {
            isMove = true;
            rb.velocity = new Vector2(moveVector.x * ps.MovementSpeed, moveVector.y * ps.MovementSpeed);
            moveSnap = moveVector;
            //Debug.Log("speed: " + ps.MovementSpeed);
        }
        else if(isDash)
        {
            isMove = false;
        }
        else
        {
            isMove = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void CheckFlipping()
    {
        if (isDash) return;
        if (flipDirect > 0.0f && inputDirect < 0.0f)
        {
            Flipping();
        }
        else if (flipDirect < 0.0f && inputDirect > 0.0f)
        {
            Flipping();
        }

        if (rb.velocity.x != 0 || rb.velocity.y !=0)
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }
    }

    private void Flipping()
    {
        if (canFlip)
        {
            flipDirect *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    public void subFlipping()
    {
        Flipping();
    }
    public void subFlipping(float time)
    {
        Flipping();
        canFlip = false;
        
        if (flipCoroutine != null)
        {
            StopCoroutine(flipCoroutine);
        }
        
        flipCoroutine = StartCoroutine(flipReset(time));
    }

    IEnumerator flipReset(float time)
    {
        yield return new WaitForSeconds(time);

        canFlip = true;
    }

    private void AnimationControl()
    {
        amt.SetBool("isMove", isMove); 
        amt.SetBool("isDash", isDash);
    }

    public bool CanDash
    {
        get => canDash;
        set => canDash = value;
    }

    public Rigidbody2D Rb
    {
        get => rb;
        set => rb = value;
    }

    public Vector2 MoveVector
    {
        get => moveVector;
        set => moveVector = value;
    }

    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    public bool CanFlip
    {
        get => canFlip;
        set => canFlip = value;
    }

    public bool IsDash
    {
        get => isDash;
        set => isDash = value;
    }

    public Vector2 MoveSnap
    {
        get => moveSnap;
        set => moveSnap = value;
    }

    public int FlipDirect
    {
        get => flipDirect;
        set => flipDirect = value;
    }

    public bool CanInput
    {
        get => canInput;
        set => canInput = value;
    }
}
