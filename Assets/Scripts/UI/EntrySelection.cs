using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EntrySelection : MonoBehaviour, IPointerClickHandler
{
    public int id;
    public string label;
    private TutorialSelectionManager selectionManager;
    private CustomButton button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<CustomButton>();
        if (id == 0) Select();
    }


    public void SetSelectionManager(TutorialSelectionManager manager)
    {
        selectionManager = manager;
    }
    public void SetId(int id)
    {
        this.id = id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        button.SetAsSelection(true);
         selectionManager.SetSelection(this);
    }

    public void Select()
    {
        if(button == null) button = GetComponent<CustomButton>();
        button.SetAsSelection(true);
        selectionManager.SetSelection(this);
    }    
    public void Deselect()
    {
        button.SetAsSelection(false);
    }
}
