using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private Vector2 directionFromRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateDirectionFromRotation();
    }

    void Update()
    {
        Move();
    }

    void UpdateDirectionFromRotation()
    {
        float angle = transform.rotation.eulerAngles.z;
        float angleRadian = angle * Mathf.Deg2Rad;
        directionFromRotation = new Vector2(Mathf.Cos(angleRadian) , Mathf.Sin(angleRadian));
    }

    void Move()
    {
        rb.velocity = directionFromRotation * speed;
    }

    
}
