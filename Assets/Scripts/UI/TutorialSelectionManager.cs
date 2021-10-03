using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSelectionManager : MonoBehaviour
{
    public List<TutorialEntry> gameMechanicsEntries;
    public CustomButton GM;
    public CustomButton QM;
    public CustomButton S;

    [SerializeField]
    private Image displayImage;
    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private GameObject buttonPanel;

    private EntrySelection selection;


    private void OnEnable()
    {
        Init();
        //firstSelection.Select();
    }

    public void Init()
    {
        for (int i = 0; i < gameMechanicsEntries.Count; i++)
        {
            InitItem(gameMechanicsEntries[i], i);
        }
        SetGM();

    }

    void InitItem(TutorialEntry entry, int index)
    {
        GameObject go = Instantiate(button, buttonPanel.transform);
        CustomButton cb = go.GetComponent<CustomButton>();
        cb.text.text = entry.title;
        EntrySelection es = go.GetComponent<EntrySelection>();
        es.SetSelectionManager(this);
        es.SetId(index);
       
    }

    public void SetGM()
    {
        QM.SetAsSelection(false);
        S.SetAsSelection(false);
        GM.SetAsSelection(true);
    }
    public void SetQM()
    {
        GM.SetAsSelection(false);
        S.SetAsSelection(false);
        QM.SetAsSelection(true);
    }

    public void SetS()
    {
        QM.SetAsSelection(false);
        GM.SetAsSelection(false);
        S.SetAsSelection(true);
    }

    public void SetSelection(EntrySelection entrySelection)
    {
        if(selection!= null)
        {
            selection.Deselect();
        }
        selection = entrySelection;
        displayImage.sprite = gameMechanicsEntries[selection.id].image1;
        infoText.text = gameMechanicsEntries[selection.id].info1;
    }
}
