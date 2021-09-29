using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineElement : MonoBehaviour
{
    public int id;
    public UiElement connectionOne;
    public UiElement connectionTwo;
    private LineRenderer lineRenderer;
    [SerializeField]
    private float lineWidth;
    private CapsuleCollider capsule;

    private void Start()
    {
        if (connectionOne != null && connectionTwo != null)
        {

            lineRenderer = GetComponent<LineRenderer>();
            capsule = gameObject.AddComponent<CapsuleCollider>();
            capsule.radius = lineWidth / 2;
            capsule.center = Vector3.zero;
            capsule.direction = 2; // Z-axis for easier "LookAt" orientation

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (connectionOne != null && connectionTwo != null)
        {
            lineRenderer.SetPosition(0, connectionOne.transform.position);
            lineRenderer.SetPosition(1, connectionTwo.transform.position);

            capsule.transform.position = connectionOne.transform.position + (connectionTwo.transform.position - connectionOne.transform.position) / 2;
            capsule.transform.LookAt(connectionOne.transform.position);
            capsule.height = (connectionTwo.transform.position - connectionOne.transform.position).magnitude;
        }
    }

}

