﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSelectionManager : MonoBehaviour
{
    public List<TutorialEntry> gameMechanicsEntries;
    public List<TutorialEntry> quantumMechanicsEntries;
    public GameObject gmScrollArea;
    public GameObject qmScrollArea;
    public GameObject entryArea;
    public GameObject storyArea;
    public CustomButton GM;
    public CustomButton QM;
    public CustomButton S;
    public Sprite defaultImage;

    [SerializeField]
    private Image displayImage1;
    [SerializeField]
    private TextMeshProUGUI infoText1;
    [SerializeField]
    private Image displayImage2;
    [SerializeField]
    private TextMeshProUGUI infoText2;
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private GameObject gmContent;
    [SerializeField]
    private GameObject qmContent;

    private EntrySelection selection;


    private void OnEnable()
    {
        Init();

    }

    public void Init()
    {
        for (int i = 0; i < gameMechanicsEntries.Count; i++)
        {
            InitItem(gameMechanicsEntries[i], gmContent.transform,i, "gm");
        }
        for (int i = 0; i < quantumMechanicsEntries.Count; i++)
        {
            InitItem(quantumMechanicsEntries[i], qmContent.transform, i, "qm");
        }
        SetGM();

    }

    void InitItem(TutorialEntry entry, Transform parent, int index, string tag)
    {
        GameObject go = Instantiate(button, parent);
        CustomButton cb = go.GetComponent<CustomButton>();
        cb.text.text = entry.title;
        EntrySelection es = go.GetComponent<EntrySelection>();
        es.SetSelectionManager(this);
        es.SetId(index);
        es.label = tag;
       
    }

    public void SetGM()
    {
        QM.SetAsSelection(false);
        qmScrollArea.SetActive(false);
        S.SetAsSelection(false);
        GM.SetAsSelection(true);
        storyArea.SetActive(false);
        gmScrollArea.SetActive(true);
        entryArea.SetActive(true);
    }
    public void SetQM()
    {
        GM.SetAsSelection(false);
        gmScrollArea.SetActive(false);
        S.SetAsSelection(false);
        storyArea.SetActive(false);
        qmScrollArea.SetActive(true);
        QM.SetAsSelection(true);
        entryArea.SetActive(true);

    }

    public void SetS()
    {
        qmScrollArea.SetActive(false);
        gmScrollArea.SetActive(false);
        entryArea.SetActive(false);
        storyArea.SetActive(true);
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
        if(entrySelection.label == "gm")
        {
            if (gameMechanicsEntries[selection.id].image1 != null)
            {
                displayImage1.sprite = gameMechanicsEntries[selection.id].image1;
            }
            else displayImage1.sprite = defaultImage;
            if (gameMechanicsEntries[selection.id].image2 != null)
            {
                displayImage2.sprite = gameMechanicsEntries[selection.id].image2;
            }
            else displayImage2.sprite = defaultImage;
            infoText1.text = gameMechanicsEntries[selection.id].info1;
            infoText2.text = gameMechanicsEntries[selection.id].info2;
        }
        if (entrySelection.label == "qm")
        {
            if (quantumMechanicsEntries[selection.id].image1 != null)
            {
                displayImage1.sprite = quantumMechanicsEntries[selection.id].image1;
            }
            else displayImage1.sprite = defaultImage;
            if (quantumMechanicsEntries[selection.id].image2 != null)
            {
                displayImage2.sprite = quantumMechanicsEntries[selection.id].image2;
            }
            else displayImage2.sprite = defaultImage;
            infoText1.text = quantumMechanicsEntries[selection.id].info1;

            infoText2.text = quantumMechanicsEntries[selection.id].info2;
        }

    }
}
