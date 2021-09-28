using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ZxDungeon.Logic;

namespace ZxDungeon.UI
{
    public class VisualCircuit : MonoBehaviour
    {
        public List<UiElement> uiElements = new List<UiElement>();
        public Circuit circuit { get; set; }
        public Canvas canvas;
        public UiElementProperties uiElementProperties;
        public TextProperties textProperties;
        private float border=100;



        // Start is called before the first frame update
        void Start()
        {
            InitCircuit(circuit);

        }


        private void InitCircuit(Circuit circuit)
        {
            int inputCount = circuit.Elements.Where(x => x.elementType == ElementType.Input).ToList().Count;
            List<UiElement> e = DrawUiElements(circuit.Elements,canvas);
            DrawLineElements(circuit,e );
        }

        private List<UiElement> DrawUiElements(List<Element> elements,Canvas canvas)
        {
            List<UiElement> e = new List<UiElement>();
            int[] lineCount = new int[5] { 0, 0, 0, 0, 0 };
            float[] widthSteps = GetWidthSteps(elements,(Screen.width));
            float heightStep = GetHeightStep(elements, (Screen.height));
            int spawnedInputs = 1;
            int spawnedOutputs = 1;

            foreach (Element element in elements)
            {
                switch(element.elementType)
                {
                    case ElementType.Input:
                        {
                            DrawUiElement(e, uiElementProperties.Input, element, border, Screen.height- (heightStep * element.lineIndex - 1));
                            lineCount[element.lineIndex-1]++;
                            spawnedInputs++;
                            break;
                        }
                    case ElementType.ZG:
                        {
                            DrawUiElement(e, uiElementProperties.ZG,  element, border + widthSteps[element.lineIndex - 1] * lineCount[element.lineIndex - 1], Screen.height - (heightStep * element.lineIndex - 1));
                            lineCount[element.lineIndex-1]++;
                            break;
                        }
                    case ElementType.ZR:
                        {
                            DrawUiElement(e, uiElementProperties.ZR, element, border + widthSteps[element.lineIndex - 1] * lineCount[element.lineIndex - 1], Screen.height - (heightStep * element.lineIndex - 1));
                            lineCount[element.lineIndex - 1]++;
                            break;
                        }
                    case ElementType.Output:
                        {
                            DrawUiElement(e, uiElementProperties.Output,  element, Screen.width - border, Screen.height - (heightStep * element.lineIndex - 1));
                            spawnedOutputs++;
                            break;
                        }
                }
            }
                return e;

        }
        private void DrawLineElements(Circuit graph, List<UiElement> uiElements)
        {
            List<Element> elements = graph.Elements;
            int[,] matrix = graph.AdjancenceMatrix;
            int a = matrix.GetLength(0);
            int b = matrix.GetLength(1);
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 1; j < matrix.GetLength(1); j++)
                {
                    if(matrix[i,j] ==1)
                    { 
                        if(i>=j)
                        {
                            GameObject go = Instantiate(uiElementProperties.LineObject,transform);
                            LineElement le = go.GetComponent<LineElement>();
                            le.connectionOne = uiElements.Single(x => x.id == matrix[i, 0]);
                            uiElements.Find(x => x.id == le.connectionOne.id).connections.Add(le);
                            le.connectionTwo = uiElements.Single(x => x.id == matrix[0, j]);
                            uiElements.Find(x => x.id == le.connectionTwo.id).connections.Add(le);


                        }
                    }
                }
            }
        }
        private void DrawUiElement(List<UiElement> uiElements, GameObject obj,  Element e, float x, float y)
        {
            Vector3 spawnPoint = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10));
            GameObject go = Instantiate(obj,transform);
            go.transform.localPosition = spawnPoint;
            UiElement ue = go.GetComponent<UiElement>();
            ue.id = e.id;
            ue.initialPosition = spawnPoint;
            ue.type = e.elementType;
            ue.value = e.value;
            ue.textParent = canvas.transform;
            uiElements.Add(ue);
            ue.Init(canvas.transform);
        }
        private float[] GetWidthSteps(List<Element> elements, float availableWidth)
        {
            float[] steps = new float[5];
            steps[0] = elements.Where(x => x.lineIndex == 1).ToList().Count;
            steps[1] = elements.Where(x => x.lineIndex == 2).ToList().Count;
            steps[2] = elements.Where(x => x.lineIndex == 3).ToList().Count;
            steps[3] = elements.Where(x => x.lineIndex == 4).ToList().Count;
            steps[4] = elements.Where(x => x.lineIndex == 5).ToList().Count;
            for (int i = 0; i < 5; i++)
            {
                if (steps[i] != 0) steps[i] = availableWidth / steps[i]-1;
            }
            return steps;
        }
        private float GetHeightStep(List<Element> elements, float availableHeight)
        {
            
            float steps = elements.Where(x => x.elementType == ElementType.Input).ToList().Count;
            steps = availableHeight / (steps + 1);
            return steps;
        }


    }
}
