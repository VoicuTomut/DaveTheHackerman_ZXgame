using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitUIText : MonoBehaviour
{
    public Vector3 targetPosition = Vector3.zero;
    private void Start()
    {
        transform.position = Camera.main.WorldToScreenPoint(targetPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
