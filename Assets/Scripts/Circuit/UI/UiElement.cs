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
        public Transform textParent;
        public List<LineElement> connections;
        public ElementType type;
        public Vector3 initialPosition;
        public TextProperties textProperties;

        private bool isDragging = false;
        private bool canBeDragged = true;


        private void Start()
        {
 
        }
        // Update is called once per frame
        void Update()
        {
            if(isDragging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    GameObject hitObject = hit.collider.transform.gameObject;
                    UiElement e = hitObject.GetComponent<UiElement>();
                    if (e != null)
                    {
                        if (IsConnectedTo(e) && (e.type == type))
                        {
                                //foreach (LineElement l in connections)
                                //{
                                //    if (IsConnection(l, id, e.id))
                                //    {
                                //        Destroy(l.gameObject);
                                //    }
                                //    else SwitchConnection(l, id, e);
                                //}
                                //e.RecalculateValue(e.value + value);
                                //Destroy(uiValue);
                                //Destroy(this.gameObject);
                        }
                            transform.position = initialPosition;
                            canBeDragged = false;
   
                    }

                }
            }
       
            
        }

        private void SwitchConnection(LineElement l,int id, UiElement e)
        {
            if(l.connectionOne.id == id)
            {
                l.connectionOne = e;
            }
            else if (l.connectionTwo.id == id)
            {
                l.connectionTwo = e;
            }
            e.connections.Add(l);
        }

        private void OnMouseDrag()
        {
            if(canBeDragged)
            {
                isDragging = true;
                gameObject.GetComponent<SphereCollider>().enabled = false;
                if (type != ElementType.Input && type != ElementType.Output)
                {
                    Vector3 followPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    followPos.z = transform.position.z;
                    uiValue.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                    this.transform.position = followPos;
                }
            }
        }

        private void OnMouseUp()
        {
            isDragging = false;
            canBeDragged = true;
            gameObject.GetComponent<SphereCollider>().enabled = true;

        }

        private bool IsConnection(LineElement l,int idOne,int idTwo )
        {
            if ((l.connectionOne.id == idOne && l.connectionTwo.id == idTwo) || (l.connectionOne.id == idTwo && l.connectionTwo.id == idOne)) return true;

            return false;
        }
        private bool IsConnectedTo(UiElement e)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                for (int j = 0; j < e.connections.Count; j++)
                {
                    if(connections[i]== e.connections[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void RecalculateValue(float value)
        {
         
            Text t = uiValue.GetComponent<Text>();
            t.text = value.ToString();
            t.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
        }
        public void Init(Transform parent)
        {
            transform.forward = Camera.main.transform.forward;
            
            if (type == ElementType.ZG || type == ElementType.ZR)
            {
                uiValue = Instantiate(textProperties.textObject, parent);
                Text t = uiValue.GetComponent<Text>();
                if (type == ElementType.ZR) t.color = new Color(textProperties.ZR.r, textProperties.ZR.g, textProperties.ZR.b);
                if (type == ElementType.ZG) t.color = new Color(textProperties.ZG.r, textProperties.ZG.g, textProperties.ZG.b);
                t.text = value.ToString() ;
                t.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
            }
        }
   
    }
}
