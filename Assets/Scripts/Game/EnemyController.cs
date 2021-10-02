using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{


    [SerializeField]
    private Transform projectilePivot;
    [SerializeField]
    private GameObject spawnEffects;
    [SerializeField]
    private GameObject deathEffects;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color damageColor;
    [SerializeField]
    private ProjectileSpawner[] projectileSpawnPoints;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float projectilePivotRotateSpeed;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    float delayBeforeStartShooting;
    [SerializeField]
    private bool isStatic;
    [SerializeField]
    private bool canWander;
    [SerializeField]
    private bool canTurn;
    [SerializeField]
    private bool canRotateProjectilePivot;
    private AudioSource audioSource;
    private float cooldownTime = 100;
    private int currentHealth;
    private new Rigidbody rigidbody;
    private PlayerController player;
    private ObjectPool effectsPool;
    private MeshRenderer meshRenderer;
    [SerializeField]
    private float wanderRadius;
    [SerializeField]
    Vector3 targetDestination;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        effectsPool = GetComponent<ObjectPool>();
        audioSource.playOnAwake = false;
        currentHealth = maxHealth;
        meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(DelayShooting(delayBeforeStartShooting * (Random.Range(10, 21) / 10)));
        if(canWander)
        {
            targetDestination = (Random.insideUnitSphere * wanderRadius) + transform.position;
            targetDestination.y = transform.position.y;
        }
    }


    // Update is called once per frame
    void Update()
    {
        HandleShooting();
        if(canTurn)
        {
            HandleRotation();
        }
        if(canRotateProjectilePivot)
        {
            HandlePivotRotation();
        }

    }

    void FixedUpdate()
    {
        if (!isStatic && !canWander)
        {
            FollowPlayer();
        }
        if(canWander && !isStatic)
        {
            HandleWandering();
        }
    }

    private void HandleWandering()
    {
        RaycastHit hit;
        Debug.DrawLine(transform.position, transform.position - targetDestination, Color.red);
        Physics.Raycast(transform.position, targetDestination, out hit, 3f);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("WallCollider"))
            {
                SetNewDestination();
            }
        }
        if ((transform.position - targetDestination).magnitude < 1)
        {
            SetNewDestination();
        }
       
        rigidbody.velocity = (targetDestination-transform.position).normalized * movementSpeed;
    }

    private void SetNewDestination()
    {
        targetDestination = (Random.insideUnitSphere * wanderRadius) + transform.position;
        targetDestination.y = transform.position.y;
    }

    private void FollowPlayer()
    {
        Vector3 directionOffset = Vector3.zero;
        Vector3 targetDirection = transform.forward;
        int neighboursCount = 0;
        Collider[] hitcolliders = Physics.OverlapSphere(transform.position, 5);
        foreach (var collider in hitcolliders)
        {
            if(collider.gameObject.CompareTag("Enemy") && collider.transform != transform)
            {
                Vector3 distance = transform.position - collider.gameObject.transform.position;
                distance = distance.normalized / Mathf.Abs(distance.magnitude);
                directionOffset += distance;
                //Debug.DrawLine(transform.position, collider.gameObject.transform.position, Color.red);

                neighboursCount++;
            }
        }
        if(neighboursCount>0)
        {
            //directionOffset /= neighboursCount;
            directionOffset.y = 0;
            targetDirection += directionOffset;
        }
        

        rigidbody.velocity = movementSpeed * targetDirection;
    }

    private void HandleRotation()
    {
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
    }

    private void HandlePivotRotation()
    {
        projectilePivot.transform.RotateAround(transform.position, Vector3.up, projectilePivotRotateSpeed * Time.deltaTime);
    }

    private void HandleShooting()
    {
       if(cooldownTime <= 0) Shoot();
        
        cooldownTime -= Time.deltaTime;
    }

    private void Shoot()
    {
        for (int i = 0; i < projectileSpawnPoints.Length; i++)
        {
            projectileSpawnPoints[i].Shoot();
        }
        cooldownTime = fireRate;
    }


    public Vector3 RotateAround(Vector3 position, Vector3 center, Vector3 axis, float angle)
    {
        Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center);
        Vector3 resultVec3 = center + point;
        return resultVec3;
    }

    public void TakeDamage()
    {
       

        currentHealth--;
        if (currentHealth <= 0)
        {
            GameObject go = Instantiate(deathEffects);
            go.transform.position = transform.position;
            DeathEffects de = go.GetComponent<DeathEffects>();
            de.PlayDeathEffects();
            gameObject.SetActive(false);
            return;
        }
        GameObject obj = effectsPool.InstantiateObject(transform);
        obj.GetComponent<DamageEffects>().SetParentPool(effectsPool);
        audioSource.Play();
        meshRenderer.material.color = damageColor;
        StartCoroutine(ResetColor(0.05f));
    }

    IEnumerator ResetColor(float amount)
    {
        yield return new WaitForSeconds(amount);
        meshRenderer.material.color = normalColor;
    }

    IEnumerator DelayShooting(float amount)
    {
        yield return new WaitForSeconds(amount);
        cooldownTime = 0;
    }
    private void OnEnable()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        GameObject go = Instantiate(spawnEffects, transform);
        StartCoroutine(Spawn(go));
    }

    IEnumerator Spawn(GameObject g)
    {
        yield return new WaitForSeconds(0.5f);
        meshRenderer.enabled = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(g);
    }
}
