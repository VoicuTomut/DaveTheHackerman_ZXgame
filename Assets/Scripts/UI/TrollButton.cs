using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class TrollButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool interactible;
    public Image decoration;
    public Sprite normalBackground;
    public Sprite hoveredBackground;
    public int normalFontSize;
    public int highlightFontSize;
    public string title;
    public TextMeshProUGUI text;
    private bool isSelected;
    public Color normalColor;
    public Color highlightColor;
    public AudioClip onHoverClip;
    public UnityEvent onClick;
    private AudioSource audioSource;
    private Image background;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        text.text = "Sorry, Alt+F4";

        if (interactible)
        {
            Select();
            isSelected = true;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactible)
        {
            audioSource.PlayOneShot(onHoverClip);
        }
        Select();

    }

    public void OnPointerExit(PointerEventData eventData)
    {

            Deselect();

        

    }
    public void Deselect()
    {
        if (text == null) Init();
        decoration.enabled = false;
        text.fontSize = normalFontSize;
        text.color = normalColor;

    }

    public void Select()
    {
        if (text == null) Init();
        text.fontSize = highlightFontSize;
        text.color = highlightColor;
        decoration.enabled = true;
    }


    void Init()
    {
        audioSource = GetComponent<AudioSource>();
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.fontSize = normalFontSize;
        text.color = normalColor;
        if (decoration != null) decoration.enabled = false;
    }
}
