using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{

    public ObjectPool objectPool;
    [SerializeField]
    private Material destructableBulletMaterial;
    [SerializeField]
    private Material indestructableBulletMaterial;
    [SerializeField]
    private int destructableBurstSize;
    [SerializeField]
    private int indestructableBurstSize;
    [SerializeField]
    private bool canSwitchBurst;
    private int currentBurst=0;
    bool isDestructableBurst = true;



    // Start is called before the first frame update
    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    private void SpawnProjectile(bool destructable)
    {
        GameObject obj = objectPool.InstantiateObject( transform.position);
        if(destructable)
        {
            obj.GetComponent<MeshRenderer>().material = destructableBulletMaterial;
        }
        else obj.GetComponent<MeshRenderer>().material = indestructableBulletMaterial;

        Projectile p = obj.GetComponent<Projectile>();
        if (p != null)
        {
            Vector3 shootDirection = transform.forward;
            shootDirection.y = 0;
            p.moveDirection = shootDirection.normalized;
            p.transform.forward = p.moveDirection;
            p.SetParentPool(objectPool);
            p.diesOnProjectiles = destructable;
        }
        else objectPool.DestroyObject(obj);
    }

    public void Shoot()
    {
        if(canSwitchBurst)
        {
            if (isDestructableBurst)
            {
                if (currentBurst > destructableBurstSize)
                {
                    isDestructableBurst = !isDestructableBurst;
                    currentBurst = 0;
                }
                SpawnProjectile(isDestructableBurst);
                currentBurst++;
                return;

            }
            else
            {
                if (currentBurst > indestructableBurstSize)
                {
                    isDestructableBurst = !isDestructableBurst;
                    currentBurst = 0;
                }
                SpawnProjectile(isDestructableBurst);
                currentBurst++;
                return;
            }
        }
        else
        {
            SpawnProjectile(isDestructableBurst);
        }
   
    }
}
