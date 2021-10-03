using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialEntry", menuName = "ScriptableObjects/TutorialEntry", order = 3)]
public class TutorialEntry : ScriptableObject
{
    public string title;
    public Sprite image1;
    public Sprite image2;
    [TextArea]
    public string info1;
    [TextArea]
    public string info2;
}
