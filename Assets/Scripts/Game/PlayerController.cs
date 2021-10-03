using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    public int health = 3;
    public GameObject hpOne;
    public GameObject hpTwo;
    public new Rigidbody rigidbody;
    [SerializeField]
    private Transform playerObject;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Transform cameraTarget;
    [SerializeField]
    private Transform projectileSpawn;
    [SerializeField]
    AudioSource audioSource;
    private ObjectPool objectPool;
    private Vector3 cameraVelocity = Vector3.zero;
    public GameObject blueBlastEffects;
    public GameObject redBlastEffects;

    private Vector3 moveDirection;
    private Vector3 moveVelocity;
    [SerializeField]
    private float fireRate;
    private float cooldownTime;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float smoothing;
    private float movementX;
    private float movementY;
    [SerializeField]
    private UnityEvent OnCollision;
    [SerializeField]
    private UnityEvent OnDeath;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        objectPool = GetComponent<ObjectPool>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleRotation();
        HandleFire();
    }
    private void FixedUpdate()
    {
        HandleMovement();

    }

    private void LateUpdate()
    {
        HandleCameraMovement();
    }

    private void HandleFire()
    {
        if(Input.GetMouseButton(0) && cooldownTime <= 0)
        {
            Fire();
        }
        cooldownTime -= Time.deltaTime;
    }

    private void Fire()
    {
        GameObject obj = objectPool.InstantiateObject( projectileSpawn.position);
        Projectile p = obj.GetComponent<Projectile>();
        if(p!=null)
        {
            p.moveDirection = projectileSpawn.transform.forward.normalized;
            p.transform.forward = p.moveDirection;
            p.SetParentPool(objectPool);
            audioSource.Play();
        }
        cooldownTime = fireRate;
    }

    private void HandleCameraMovement()
    {
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, cameraTarget.position,ref cameraVelocity,smoothing);
    }

    private void HandleRotation()
    {

        Vector3 mouseProjectedPosition = Vector3.zero;
        Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(transform.up, transform.position);
        float rayLength;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            mouseProjectedPosition = cameraRay.GetPoint(rayLength);
            
        }

        playerObject.rotation = Quaternion.Euler(-90,  Mathf.Atan2( mouseProjectedPosition.x - transform.position.x, mouseProjectedPosition.z - transform.position.z) * Mathf.Rad2Deg  ,0);

    }

    public void SafetyBlast(float percentage)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Projectile>())
            {
                collider.gameObject.SetActive(false);
            }

        }
        if (percentage>0.65f)
        {
            GameObject go = Instantiate(blueBlastEffects, transform);
            go.transform.position = transform.position;
            DeathEffects de = go.GetComponent<DeathEffects>();
            de.PlayEffects();
        }
        else
        {

            GameObject go = Instantiate(redBlastEffects);
            go.transform.position = transform.position;
            DeathEffects de = go.GetComponent<DeathEffects>();
            de.PlayEffects();
            bool dead = CheckIsDead(--health);
            if (dead)
            {
                //OnDeath.Invoke();
                return;
            }
            go.transform.SetParent(transform);
        }
     
        StartCoroutine(DoubleSafetyBlast());

    }

    private IEnumerator DoubleSafetyBlast()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.05f);
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10);
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<Projectile>())
                {
                    collider.gameObject.SetActive(false);
                }
            }
        }

    }

    private void HandleMovement()
    {

        rigidbody.velocity = moveVelocity;
    }

    private void HandleInput()
    {
        movementX = Input.GetAxis("Horizontal");
        movementY = Input.GetAxis("Vertical");
     
        moveDirection = new Vector3(movementX, 0, movementY);
        moveDirection.Normalize();
        moveVelocity = new Vector3(moveDirection.x * movementSpeed,0, moveDirection.z * movementSpeed);
    }

    public void TakeDamage()
    {
        OnCollision.Invoke();
    }

    bool CheckIsDead(int health)
    {
        bool isDead = false;
        switch (health)
        {
            case 2:
                {
                    hpOne.SetActive(false);
                    break;
                }
            case 1:
                {
                    hpTwo.SetActive(false);
                    break;
                }
            case 0:
                {
                    isDead = true;
                    break;
                }
        }
        return isDead;
    }
}
