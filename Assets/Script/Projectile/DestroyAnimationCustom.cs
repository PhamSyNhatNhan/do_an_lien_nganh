using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimationCustom : MonoBehaviour
{
    [SerializeField] private float lifetime;
    
    void Start()
    {
        StartCoroutine(destroy());
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(lifetime);
        
        Destroy(gameObject);
    }
}
