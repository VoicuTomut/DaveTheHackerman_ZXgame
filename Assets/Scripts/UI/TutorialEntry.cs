using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialEntry", menuName = "ScriptableObjects/TutorialEntry", order = 3)]
public class TutorialEntry : ScriptableObject
{
    public string title;
    public Sprite image;
    public string info;
}
