using System;
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
    private Image displayImage;
    [SerializeField]
    private TextMeshProUGUI infoText;
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
            InitItem(gameMechanicsEntries[i], qmContent.transform, i, "qm");
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
        es.tag = tag;
       
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
        if(entrySelection.tag == "gm")
        {
            if (gameMechanicsEntries[selection.id].image1 != null)
            {
                displayImage.sprite = gameMechanicsEntries[selection.id].image1;
            }
            else displayImage.sprite = defaultImage;
            if (gameMechanicsEntries[selection.id].image2 != null)
            {
                displayImage.sprite = gameMechanicsEntries[selection.id].image1;
            }
            else displayImage.sprite = defaultImage;
            infoText.text = gameMechanicsEntries[selection.id].info1;
        }
        if (entrySelection.tag == "qm")
        {
            if (quantumMechanicsEntries[selection.id].image1 != null)
            {
                displayImage.sprite = quantumMechanicsEntries[selection.id].image1;
            }
            else displayImage.sprite = defaultImage;
            if (quantumMechanicsEntries[selection.id].image2 != null)
            {
                displayImage.sprite = quantumMechanicsEntries[selection.id].image1;
            }
            else displayImage.sprite = defaultImage;
            infoText.text = gameMechanicsEntries[selection.id].info1;
        }

    }
}
