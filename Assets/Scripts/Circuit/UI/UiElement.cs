using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZxDungeon.Logic;

namespace ZxDungeon.UI
{
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
        private new Collider collider;
        private bool isDragging = false;
        private bool canBeDragged = true;


        void Start()
        {
            collider = GetComponent<Collider>();
            if (type == ElementType.Input || type == ElementType.Output) { canBeDragged = false; }
        }

        //private void SwitchConnection(LineElement l,int id, UiElement e)
        //{
        //    if(l.connectionOne.id == id)
        //    {
        //        l.connectionOne = e;
        //    }
        //    else if (l.connectionTwo.id == id)
        //    {
        //        l.connectionTwo = e;
        //    }
        //    e.connections.Add(l);
        //}

        private void OnMouseDrag()
        {
            if(canBeDragged)
            {
                isDragging = true;
                if (type != ElementType.Input && type != ElementType.Output)
                {
                    collider.enabled = false;
                    Vector3 followPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    followPos.z = transform.position.z;
                    uiValue.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                    this.transform.position = followPos;
                }
            }
        }

        private void OnMouseUp()
        {
            if (isDragging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100))
                {
                    GameObject hitObject = hit.collider.transform.gameObject;
                    UiElement e = hitObject.GetComponent<UiElement>();
                    e.uiValue.gameObject.SetActive(false);
                    if (e != null)
                    {
                        Circuit c = visualCircuit.circuit.FuseElements(visualCircuit.circuit, id, e.id);
                        if (c != null)
                        {
                            visualCircuit.circuit = c;
                            visualCircuit.InitCircuit(c);
                        }
                        else
                        {
                            transform.position = initialPosition;
                            uiValue.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                            canBeDragged = false;
                        }
                    }
                    //{
                    //    if (IsConnectedTo(e) && (e.type == type))
                    //    {
                    //        foreach (LineElement l in connections)
                    //        {
                    //            if (IsConnection(l, id, e.id))
                    //            {
                    //                AdvancedObjectPool.instance.DestroyObject("Line", l.gameObject);
                    //            }
                    //            else SwitchConnection(l, id, e);
                    //        }
                    //        e.RecalculateValue(e.value + value);
                    //        AdvancedObjectPool.instance.DestroyObject("Text", uiValue);
                    //        Destroy(this.gameObject);
                    //        return;
                    //    }
                    //    
                    //    
                    //    
                }
            }
            isDragging = false;
            canBeDragged = true;
            gameObject.GetComponent<SphereCollider>().enabled = true;

        }

        //private bool IsConnection(LineElement l,int idOne,int idTwo )
        //{
        //    if ((l.connectionOne.id == idOne && l.connectionTwo.id == idTwo) || (l.connectionOne.id == idTwo && l.connectionTwo.id == idOne)) return true;

        //    return false;
        //}
        //private bool IsConnectedTo(UiElement e)
        //{
        //    for (int i = 0; i < connections.Count; i++)
        //    {
        //        for (int j = 0; j < e.connections.Count; j++)
        //        {
        //            if(connections[i]== e.connections[j])
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
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
                t.text = id.ToString();
                t.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                uiValue = go;
            }
            else go.SetActive(false);
        }
   
    }
}
