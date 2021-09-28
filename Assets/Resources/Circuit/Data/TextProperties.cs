using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TextProperties", order = 3)]
public class TextProperties : ScriptableObject
{
    public GameObject textObject;
    public Color ZG;
    public Color ZR;
}
