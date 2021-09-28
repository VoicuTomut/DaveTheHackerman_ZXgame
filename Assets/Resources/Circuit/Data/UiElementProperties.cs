using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZxDungeon.UI
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UiElementProperties", order = 2)]
    public class UiElementProperties : ScriptableObject
    {
        public GameObject Input;
        public GameObject Output;
        public GameObject ZG;
        public GameObject ZR;
        public GameObject LineObject;
    }
}
