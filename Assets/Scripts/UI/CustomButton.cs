using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image decoration;
    public Sprite normalBackground;
    public Sprite hoveredBackground;
    public int normalFontSize;
    public int highlightFontSize;
    public Color normalColor;
    public Color highlightColor;
    public AudioClip onHoverClip;
    public AudioClip onClickClip;
    public UnityEvent onClick;
    private AudioSource audioSource;
    private Image background;
    private TextMeshProUGUI   text;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.fontSize = normalFontSize;
        text.color = normalColor;
        //background.sprite = normalBackground;
        decoration.enabled = false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.PlayOneShot(onClickClip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot(onHoverClip);
        //background.sprite = hoveredBackground;
        text.fontSize = highlightFontSize;
        text.color = highlightColor;
        decoration.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //background.sprite = normalBackground;
        decoration.enabled = false;
        text.fontSize = normalFontSize;
        text.color = normalColor;

    }
}
