using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZxDungeon.UI
{
    public class LineElement : MonoBehaviour
    {
        public int id;
        public UiElement connectionOne;
        public UiElement connectionTwo;
        private LineRenderer lineRenderer;

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            lineRenderer.SetPosition(0, connectionOne.transform.position);
            lineRenderer.SetPosition(1, connectionTwo.transform.position);

        }
    }
}
