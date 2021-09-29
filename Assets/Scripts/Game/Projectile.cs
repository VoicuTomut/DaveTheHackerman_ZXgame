using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Vector3 moveDirection;
    [SerializeField]
    private float speed;

    public bool diesOnProjectiles;

    private new Rigidbody rigidbody;
    private new   Collider collider;
    
    private ObjectPool parentPool;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {

        HandleMovement();
    }


    private void HandleMovement()
    {
        rigidbody.velocity = speed * moveDirection;
    }



    public void SetParentPool(ObjectPool objectPool)
    {
        this.parentPool = objectPool;
    }

    //this is a mess but I don't have the time and patience to implement something more manageable
    private void OnCollisionEnter(Collision collision)
    {
        string thisTag = gameObject.tag;
        string otherTag = collision.gameObject.tag;
        switch (thisTag)
        {
            case "PlayerBullet":
                {
                    
                    switch(otherTag)
                    {
                        case "WallCollider":
                            {
                                parentPool.DestroyObject(gameObject);
                                break;
                            }
                        case "Enemy":
                            {
                                EnemyController e = collision.gameObject.GetComponent<EnemyController>();
                                e.TakeDamage();
                                parentPool.DestroyObject(gameObject);
                                break;
                            }
                        case "Untagged":
                            {
                                parentPool.DestroyObject(gameObject);
                                break;
                            }
                    }
                    

                    break;
                }
            case "EnemyBullet":
                {
                    switch (otherTag)
                    {
                        case "WallCollider":
                            {
                                parentPool.DestroyObject(gameObject);
                                break;
                            }
                        case "Player":
                            {
                                //EnemyController e = collision.gameObject.GetComponent<EnemyController>();
                                //e.TakeDamage();
                                parentPool.DestroyObject(gameObject);
                                break;
                            }
                        case "PlayerBullet":
                            {
                                if(diesOnProjectiles)
                                { 
                                    parentPool.DestroyObject(gameObject);
                                }
                                else
                                {
                                    Physics.IgnoreCollision(collider, collision.collider);
                                }
                                break;
                            }
                        case "Enemy":
                            {
                                Physics.IgnoreCollision(collider, collision.collider);
                                break;
                            }
                        case "EnemyBullet":
                            {
                                Physics.IgnoreCollision(collider, collision.collider);
                                break;
                            }

                    }

                    break;
                }
        }

    
    }

  
}
