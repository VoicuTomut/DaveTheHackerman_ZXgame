using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public class VisualCircuit : MonoBehaviour
{
    public Circuit circuit;
    public Canvas canvas;
    public LayerMask uiElementLayer;
    public LayerMask lineElementLayer;
    private readonly float border = 100;
    private AdvancedObjectPool pool;
    [SerializeField]
    private UiElement selectedElement;



    // Start is called before the first frame update
    void Start()
    {
        pool = GetComponent<AdvancedObjectPool>();
        pool.Initialise();
        InitCircuit(circuit);

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            {
                Debug.DrawLine(ray.origin, ray.direction * 100, Color.red, 10f);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 100))
                {
                    UiElement u = hitInfo.collider.GetComponent<UiElement>();
                    if (u != null)
                    {
                        if (selectedElement != null)
                        {
                            selectedElement.Deselect();
                            selectedElement = null;
                        }
                        if (u.type != ElementType.Input && u.type != ElementType.Output)
                        {
                            u.Select();
                            selectedElement = u;
                        }
                    }
                }
                else if (selectedElement != null)
                {
                    selectedElement.Deselect();
                    selectedElement = null;
                }

            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            ChangeType();
        }
    }

    public void InitCircuit(Circuit circuit)
    {
        pool.ResetAll();
        selectedElement = null;
        int inputCount = circuit.Elements.Where(x => x.elementType == ElementType.Input).ToList().Count;
        List<UiElement> e = DrawUiElements(circuit.Elements);
        DrawLineElements(circuit, e);
    }

    private List<UiElement> DrawUiElements(List<Element> elements)
    {
        List<UiElement> e = new List<UiElement>();
        List<Element> hGates = new List<Element>();
        int[] lineCount = new int[5] { 0, 0, 0, 0, 0 };
        float[] widthSteps = GetWidthSteps(elements, (Screen.width));
        float heightStep = GetHeightStep(elements, (Screen.height));
        int spawnedInputs = 1;
        int spawnedOutputs = 1;

        foreach (Element element in elements)
        {
            switch (element.elementType)
            {
                case ElementType.Input:
                    {
                        DrawUiElement("Input", e, element, border, Screen.height - (heightStep * element.lineIndex - 1));
                        lineCount[element.lineIndex - 1]++;
                        spawnedInputs++;
                        break;
                    }
                case ElementType.ZG:
                    {
                        DrawUiElement("ZG", e, element, border + widthSteps[element.lineIndex - 1] * lineCount[element.lineIndex - 1], Screen.height - (heightStep * element.lineIndex - 1));
                        lineCount[element.lineIndex - 1]++;
                        break;
                    }
                case ElementType.ZR:
                    {
                        DrawUiElement("ZR", e, element, border + widthSteps[element.lineIndex - 1] * lineCount[element.lineIndex - 1], Screen.height - (heightStep * element.lineIndex - 1));
                        lineCount[element.lineIndex - 1]++;
                        break;
                    }
                case ElementType.Output:
                    {
                        DrawUiElement("Output", e, element, Screen.width - border, Screen.height - (heightStep * element.lineIndex - 1));
                        spawnedOutputs++;
                        break;
                    }
                case ElementType.Hadamart:
                    {
                        hGates.Add(element);
                        break;
                    }
            }

        }

        foreach (Element h in hGates)
        {
            List<int> neighbours = GetNeighbours(h.id);
            if (neighbours.Count != 2)
            {
                Debug.Log("Something fucked up");

                return null;
            }
            UiElement e1 = e.Single(x => x.id == neighbours[0]);
            UiElement e2 = e.Single(x => x.id == neighbours[1]);
            DrawHadamart("Square", h, e1, e2, e);
        }
        return e;

    }
    private void DrawHadamart(string tag, Element e, UiElement e1, UiElement e2, List<UiElement> elements)
    {
        Vector3 spawnPoint = (e1.transform.position + e2.transform.position) / 2;
        GameObject go = pool.InstantiateObject(tag, spawnPoint, transform);
        go.transform.localPosition = spawnPoint;
        UiElement ue = go.GetComponent<UiElement>();
        ue.id = e.id;
        ue.initialPosition = spawnPoint;
        ue.type = e.elementType;
        ue.value = e.value;
        ue.visualCircuit = this;
        elements.Add(ue);
        //ue.Init(pool.InstantiateObject("UiText", canvas.transform));
    }
    private void DrawLineElements(Circuit circuit, List<UiElement> uiElements)
    {
        List<Element> elements = circuit.Elements;
        int[,] matrix = circuit.AdjancenceMatrix;
        int a = matrix.GetLength(0);
        int b = matrix.GetLength(1);
        for (int i = 1; i < matrix.GetLength(0); i++)
        {
            for (int j = 1; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == 1)
                {
                    if (i >= j)
                    {
                        GameObject go = pool.InstantiateObject("Line", transform);
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
    private void DrawUiElement(string tag, List<UiElement> elements, Element e, float x, float y)
    {
        Vector3 spawnPoint = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10));
        GameObject go = pool.InstantiateObject(tag, spawnPoint, transform);
        go.transform.localPosition = spawnPoint;
        UiElement ue = go.GetComponent<UiElement>();
        ue.id = e.id;
        ue.initialPosition = spawnPoint;
        ue.type = e.elementType;
        ue.value = e.value;
        ue.visualCircuit = this;
        elements.Add(ue);
        ue.Init(pool.InstantiateObject("UiText", canvas.transform));
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
            if (steps[i] != 0) steps[i] = availableWidth / steps[i] - 1;
        }
        return steps;
    }
    private float GetHeightStep(List<Element> elements, float availableHeight)
    {

        float steps = elements.Where(x => x.elementType == ElementType.Input).ToList().Count;
        steps = availableHeight / (steps + 1);
        return steps;
    }

    private List<int>  GetNeighbours(int id1)
    {
        List<int> neighbours = new List<int>();
        int numberOfElements = circuit.AdjancenceMatrix.GetLength(1); //number of elemens in the initial circuit +1

        for (int i = 1; i < numberOfElements; i++)
        {
            if (circuit.AdjancenceMatrix[i, 0] == id1)
            {
                for (int j = 1; j < numberOfElements; j++)
                {
                    if (circuit.AdjancenceMatrix[i, j] == 1)
                    {
                        neighbours.Add(circuit.AdjancenceMatrix[0, j]);
                    }
                }
            }
        }
        return neighbours;
    }



    private void ChangeType()
    {
        if (selectedElement != null)
        {
            selectedElement.Deselect();
            circuit = circuit.ChangeType(circuit, selectedElement.id);
            InitCircuit(circuit);
            selectedElement = null;
        }
    }

}

