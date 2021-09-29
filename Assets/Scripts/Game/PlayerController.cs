using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform playerObject;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform cameraTarget;
    [SerializeField]
    private Transform projectileSpawn;
    [SerializeField]
    AudioSource audioSource;
    private ObjectPool objectPool;
    private Vector3 cameraVelocity = Vector3.zero;
    new Rigidbody rigidbody;


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
        GameObject obj = objectPool.InstantiateObject(projectileSpawn.position);
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
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            mouseProjectedPosition = cameraRay.GetPoint(rayLength);
            
        }

        playerObject.rotation = Quaternion.Euler(-90,  Mathf.Atan2( mouseProjectedPosition.x - transform.position.x, mouseProjectedPosition.z - transform.position.z) * Mathf.Rad2Deg  ,0);

    }

    private void HandleMovement()
    {
        //transform.position += moveDirection;
        rigidbody.velocity = moveVelocity;
    }

    private void HandleInput()
    {
        movementX = Input.GetAxis("Horizontal");
        movementY = Input.GetAxis("Vertical");
        //cameraVelocity = Vector3.zero;
        moveDirection = new Vector3(movementX, 0, movementY);
        moveDirection.Normalize();
        moveVelocity = new Vector3(moveDirection.x * movementSpeed,0, moveDirection.z * movementSpeed);
    }
}
