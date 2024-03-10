    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.AI;

    public class Enemy_3 : MonoBehaviour
    {
        private EnemyHealth eh;
            
        private Transform target;

        private NavMeshAgent agent;
        [SerializeField] private bool isEnabledNav = true;
        
        [Header("Attack")]
        private bool canAttack = false;
        [SerializeField] private float findRadius;
        [SerializeField] private LayerMask whatIsDamageEnabled;
        [SerializeField] private Vector2 offsetAttack;
        [SerializeField] private GameObject bullet;
        [SerializeField] private int bulletRow;
        [SerializeField] private int bulletColumn;
        private bool subAttack = true;
        
        [Header("Melee Attack")]
        [SerializeField] private float meleeAttackRadius;
        [SerializeField] private GameObject e3_meleeAttack;
        
        
        void Start()
        {
            eh = GetComponent<EnemyHealth>();
            
            target = GameObject.Find("Player").transform;
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        
        void Update()
        {
            if (canAttack && subAttack)
            {
                // StartCoroutine(SpawnAttack(transform));
                Attack();
                canAttack = false;
                subAttack = false;
            }
            else if (isEnabledNav)
            {
                agent.SetDestination(target.position);
                eh.IsMove = true;
                checkFlip();

                if ((transform.position - target.position).magnitude <= meleeAttackRadius)
                {
                    canAttack = true;
                    agent.enabled = false;
                    eh.IsMove = false;
                    isEnabledNav = false;
                    // StartCoroutine(SpawnAttack(transform));
                }
            }
        }


        private void checkFlip()
        {
            if (eh.EnemyDirect > 0 && agent.velocity.x < 0.0f)
            {
                eh.Flipping();
            }
            else if (eh.EnemyDirect < 0 && agent.velocity.x > 0.0f)
            {
                eh.Flipping();
            }
        }

        private void Attack()
        {
        
            Collider2D[] DetectObject = Physics2D.OverlapCircleAll(transform.position, findRadius, whatIsDamageEnabled);
            
            List<GameObject> Player = new List<GameObject>();
        
            foreach (Collider2D collider in DetectObject)
            {
                Player.Add(collider.gameObject);
            }

            Player.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));
            
            if (Player.Count > 0)
            { 
                GameObject nearestEnemy = Player[0];
            
                if (transform.position.x < Player[0].transform.position.x)
                {
                    if (eh.EnemyDirect != 1)
                    {
                        eh.subFlipping(0.4f);
                    }
                }
                else if(transform.position.x > Player[0].transform.position.x)
                {
                    if (eh.EnemyDirect != -1)
                    {
                        eh.subFlipping(0.4f);
                    }
                }
                
                StartCoroutine(RotateOverTime(gameObject.transform.GetChild(0).gameObject, 45f, 0.2f));
            }

            StartCoroutine(SpawnAttack(transform));
        }

        
        // IEnumerator RotateOverTime(GameObject objectRotate, float targetAngle, float duration)
        // {
        //     float startAngle = objectRotate.transform.rotation.eulerAngles.z;
        //     float currentAngle = startAngle + targetAngle * eh.EnemyDirect;
        //     float elapsedTime = 0f;
        //     
        //     while (elapsedTime < duration)
        //     {
        //         currentAngle = Mathf.Lerp(startAngle, startAngle + targetAngle, elapsedTime / duration);
        //         
        //         objectRotate.transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        //         
        //         elapsedTime += Time.deltaTime ;
        //         
        //         yield return null;
        //     }
        //     
        //     objectRotate.transform.rotation = Quaternion.Euler(0f, 0f, startAngle);
        // }
        
        IEnumerator RotateOverTime(GameObject objectRotate, float targetAngle, float duration)
        {
            float elapsedTime = 0;
            Quaternion startRotation = objectRotate.transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(
                objectRotate.transform.rotation.eulerAngles.x,
                objectRotate.transform.rotation.eulerAngles.y,
                objectRotate.transform.rotation.eulerAngles.z + targetAngle * -1.0f 
            );
        
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                objectRotate.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        
        IEnumerator SpawnAttack(Transform nearestEnemy)
        {
            yield return new WaitForSeconds(0.2f);
            //melee
            Vector3 position_ = new Vector3(transform.position.x + 0.4f * eh.EnemyDirect, transform.position.y);
            
            // eh.Parameters.setStat(eh.CurAttack * 1.5f, eh.DmgBonus, eh.CurDefPierce, eh.Rb.transform.position, 0.0f, 0.0f);
            DamageParameters parameters = new DamageParameters(eh.CurAttack * 1.5f, eh.DmgBonus, eh.CurDefPierce,
                eh.Rb.transform.position, 0.0f, 0.0f, "", 0.0f);
            GameObject e3_mA = Instantiate(e3_meleeAttack, position_, Quaternion.identity);
            e3_mA.transform.parent = null;  
            e3_mA.GetComponent<Enemy_3_MeleeAttack>().onInstantiate(parameters, 1.4f);
            //bullet
            // float angleOffset = 360/bulletRow;
            //
            // Vector3 direction = nearestEnemy.transform.position - transform.position;
            // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            //
            // for (int i = 0; i < bulletColumn; i++)
            // { 
            //     for (int j = 0; j < bulletRow; j++)
            //     {
            //         Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward);;
            //         if (j > 0 && j % 2 == 1)
            //         {
            //             bulletRotation = Quaternion.AngleAxis(angle - angleOffset * (int)((j + 1) / 2), Vector3.forward);
            //         }
            //         else if (j > 0 && j % 2 == 0)
            //         {
            //             bulletRotation = Quaternion.AngleAxis(angle + angleOffset * (int)((j + 1) / 2), Vector3.forward);
            //         }
            //         
            //         Vector3 leftAttackPosition = transform.position + (bulletRotation * offsetAttack);
            //         eh.Parameters.setStat(eh.CurAttack * 0.7f, eh.DmgBonus, eh.CurDefPierce, eh.Rb.transform.position, 0.0f, 0.0f);
            //         GameObject newBullet1 = Instantiate(bullet, e3_mA.transform.position, bulletRotation);
            //         newBullet1.GetComponent<CircleHitboxDefaultTime>().onInstantiate(eh.Parameters, 0.5f);
            //     }
            //     
            //     yield return new WaitForSeconds(0.2f);
            // }
            bullets(nearestEnemy, e3_mA.transform);
            
            agent.enabled = true;
            isEnabledNav = true;
            StartCoroutine(RotateOverTime(gameObject.transform.GetChild(0).gameObject ,-45f, 0.2f));
            StartCoroutine(endAttack(2.0f));
        }

        private void bullets(Transform nearestEnemy, Transform position_)
        {
            float angleOffset = 360/bulletRow;

            Vector3 direction = nearestEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            for (int i = 0; i < bulletColumn; i++)
            { 
                for (int j = 0; j < bulletRow; j++)
                {
                    Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward);;
                    if (j > 0 && j % 2 == 1)
                    {
                        bulletRotation = Quaternion.AngleAxis(angle - angleOffset * (int)((j + 1) / 2), Vector3.forward);
                    }
                    else if (j > 0 && j % 2 == 0)
                    {
                        bulletRotation = Quaternion.AngleAxis(angle + angleOffset * (int)((j + 1) / 2), Vector3.forward);
                    }
                    
                    Vector3 leftAttackPosition = transform.position + (bulletRotation * offsetAttack);
                    eh.Parameters.setStat(eh.CurAttack * 0.7f, eh.DmgBonus, eh.CurDefPierce, eh.Rb.transform.position, 0.0f, 0.0f);
                    GameObject newBullet1 = Instantiate(bullet, position_.transform.position, bulletRotation);
                    newBullet1.GetComponent<CircleHitboxDefaultTime>().onInstantiate(eh.Parameters, 0.5f);
                }
                
            }
        }
        IEnumerator endAttack(float time)
        {
            //Debug.Log("Attack Reset");
            yield return new WaitForSeconds(time);
            subAttack = true;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, findRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, meleeAttackRadius);
        }
    }
