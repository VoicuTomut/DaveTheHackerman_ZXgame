using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSelectionManager : MonoBehaviour
{
    public List<TutorialEntry> tutorialEntries;
    [SerializeField]
    private Image displayImage;
    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private GameObject buttonPanel;
    private EntrySelection selection;
    private EntrySelection firstSelection;

    private void OnEnable()
    {
        Init();
        //firstSelection.Select();
    }

    public void Init()
    {
        int index = 0;

        //InitItem(tutorialEntries[0], 0, true);

        for (int i = 0; i < tutorialEntries.Count; i++)
        {
            InitItem(tutorialEntries[i], i);
        }

    }

    void SelectEntry()
    {

    }
    void InitItem(TutorialEntry entry, int index)
    {
        GameObject go = Instantiate(button, buttonPanel.transform);
        CustomButton cb = go.GetComponent<CustomButton>();
        cb.title = entry.title;
        EntrySelection es = go.GetComponent<EntrySelection>();
        es.SetSelectionManager(this);
        es.SetId(index);
    }
    void InitItem(TutorialEntry entry, int index, bool isFirst)
    {
        GameObject go = Instantiate(button, buttonPanel.transform);
        CustomButton cb = go.GetComponent<CustomButton>();
        cb.title = entry.title;
        EntrySelection es = go.GetComponent<EntrySelection>();
        es.SetSelectionManager(this);
        es.SetId(index);
        firstSelection = es;
    }

    public void SetSelection(EntrySelection entrySelection)
    {
        if(selection!= null)
        {
            selection.Deselect();
        }
        selection = entrySelection;
        displayImage.sprite = tutorialEntries[selection.id].image;
        infoText.text = tutorialEntries[selection.id].info;
    }
}
