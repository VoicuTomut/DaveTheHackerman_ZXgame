using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialEntry", menuName = "ScriptableObjects/TutorialEntry", order = 3)]
public class TutorialEntry : ScriptableObject
{
    public string title;
    public Sprite image1;
    public Sprite image2;
    [TextArea(15,20)]
    public string info1;
    [TextArea(15, 20)]
    public string info2;
}
