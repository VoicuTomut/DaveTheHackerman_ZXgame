using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UiElement : MonoBehaviour
{
    public int id;
    public float value;
    public int lineIndex;
    public GameObject uiValue;
    public List<LineElement> connections;
    public ElementType type;
    public Vector3 initialPosition;
    public TextProperties textProperties;
    public VisualCircuit visualCircuit;
    public LayerMask raycastLayer;
    [SerializeField]
    private GameObject selectionEffect;
    private new Collider collider;
    private bool isDragging = false;
    private bool canBeDragged = true;


    void Start()
    {
        collider = GetComponent<Collider>();
        if (type == ElementType.Input || type == ElementType.Output) { canBeDragged = false; }
    }


    private void OnMouseDrag()
    {
        if (canBeDragged)
        {
            isDragging = true;
            if (type == ElementType.ZR || type == ElementType.ZG)
            {
                collider.enabled = false;
                Vector3 followPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                followPos.z = transform.position.z;
                transform.position = followPos;
                uiValue.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100, raycastLayer))
                {
                    UiElement e = hit.collider.gameObject.GetComponent<UiElement>();
                    if (e != null)
                        e.uiValue.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Select()
    {
        if(selectionEffect != null) selectionEffect.SetActive(true);
    }

    public void Deselect()
    {
        if(selectionEffect != null) selectionEffect.SetActive(false);
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, raycastLayer))
            {
                UiElement e = hit.collider.GetComponent<UiElement>();
                if (e != null)
                {
                    if(e.uiValue != null) e.uiValue.gameObject.SetActive(false);

                    Circuit c = visualCircuit.circuit.FuseElements(visualCircuit.circuit, id, e.id);
                    if (c != null)
                    {
                        visualCircuit.circuit = c;
                        visualCircuit.InitCircuit(c);
                    }
                }
            }
            //transform.position = initialPosition;
           if(uiValue!=null) uiValue.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            canBeDragged = false;
        }
        isDragging = false;
        canBeDragged = true;
        gameObject.GetComponent<SphereCollider>().enabled = true;

    }
    private void OnMouseExit()
    {
        if(type == ElementType.ZR || type == ElementType.ZG)uiValue.SetActive(true);
    }


    public void RecalculateValue(float value)
    {

        Text t = uiValue.GetComponent<Text>();
        t.text = value.ToString();
        t.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
    }
    public void Init(GameObject go)
    {
        //transform.forward = Camera.main.transform.forward;

        if (type == ElementType.ZG || type == ElementType.ZR)
        {
            //uiValue = Instantiate("Text", parent);
            Text t = go.GetComponent<Text>();
            if (type == ElementType.ZR) t.color = new Color(textProperties.ZR.r, textProperties.ZR.g, textProperties.ZR.b);
            if (type == ElementType.ZG) t.color = new Color(textProperties.ZG.r, textProperties.ZG.g, textProperties.ZG.b);
            t.text = value.ToString();
            t.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            uiValue = go;
        }
        else go.SetActive(false);
    }

}

