using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject_Time : MonoBehaviour
{
    [SerializeField] private float time;
    
    [SerializeField] private float moveUpDistance = 5f; 
    [SerializeField] private float fallDistance = 5f; 
    [SerializeField] private float moveSpeed = 1f;
    
    private Vector2 startPos;
    
    void Start()
    {
        StartCoroutine(destroyObject());
        
        startPos = transform.position;
        
        StartCoroutine(MoveUpAndFall());
    }
    
    IEnumerator MoveUpAndFall()
    {
        while (transform.position.y < startPos.y + moveUpDistance)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
            transform.Translate(-transform.right * 0.5f * Time.deltaTime);
            yield return null;
        }
        
        yield return new WaitForSeconds(0.0f);
        
        while (transform.position.y > startPos.y - fallDistance)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
            yield return null;
        }
        
    }
    

    IEnumerator destroyObject()
    {
        yield return new WaitForSeconds(time);
        
        Destroy(gameObject);
    }
}
