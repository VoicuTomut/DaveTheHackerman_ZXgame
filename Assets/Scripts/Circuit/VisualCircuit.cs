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
    void Awake()
    {
        pool = GetComponent<AdvancedObjectPool>();
        pool.Initialise();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            {
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
        circuit = CircuitConstrain( circuit);
        if (!pool) pool = GetComponent<AdvancedObjectPool>(); else pool.ResetAll();
        if (selectedElement) selectedElement.Deselect();
        selectedElement = null;
        int inputCount = circuit.Elements.Where(x => x.elementType == ElementType.Input).ToList().Count;
        List<UiElement> e = DrawUiElements(circuit.Elements);
        DrawLineElements(circuit, e);
    }

    private List<UiElement> DrawUiElements(List<Element> elements)
    {
        List<UiElement> e = new List<UiElement>();
        List<Element> hGates = new List<Element>();
        List<Element> extras = new List<Element>();
        int[] lineCount = new int[5] { 0, 0, 0, 0, 0 };
        float[] widthSteps = GetWidthSteps(elements, (Screen.width));
        float heightStep = GetHeightStep(elements, (Screen.height));
        int spawnedInputs = 1;
        int spawnedOutputs = 1;
        int lastOutputId = 0;
        List<Element> outputs = elements.Where(x => x.elementType == ElementType.Output).ToList();
        foreach(Element output in outputs)
        {
            if (output.id > lastOutputId) lastOutputId = output.id;
        }
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
                        if (element.id < lastOutputId)
                        {
                            DrawUiElement("ZG", e, element, border + widthSteps[element.lineIndex - 1] * lineCount[element.lineIndex - 1], Screen.height - (heightStep * element.lineIndex - 1));
                            lineCount[element.lineIndex - 1]++;
                            break;
                        }
                        else extras.Add(element); break;
                    }
                case ElementType.ZR:
                    {
                        if (element.id < lastOutputId)
                        {
                            DrawUiElement("ZR", e, element, border + widthSteps[element.lineIndex - 1] * lineCount[element.lineIndex - 1], Screen.height - (heightStep * element.lineIndex - 1));
                            lineCount[element.lineIndex - 1]++;
                            break;
                        }
                        else extras.Add(element); break;
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
        foreach (Element el in extras)
        {
            List<UiElement> neighbours = GetElementNeighbours(el.id, e);
            Vector3 position = Vector3.zero;
            string tag = string.Empty;
            foreach(UiElement ue in neighbours)
            {
               position += ue.transform.position;
            }
            if (el.elementType == ElementType.ZG) tag = "ZG";
            if (el.elementType == ElementType.ZR) tag = "ZR";
            DrawUiElement(tag, e, el, position/2);
        }

        foreach (Element h in hGates)
        {
            List<int> neighbours = GetNeighbours(h.id);
            if (neighbours.Count != 2)
            {

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
    private void DrawUiElement(string tag, List<UiElement> elements, Element e, Vector3 position)
    {
       
        GameObject go = pool.InstantiateObject(tag, position, transform);
        go.transform.localPosition = position;
        UiElement ue = go.GetComponent<UiElement>();
        ue.id = e.id;
        ue.initialPosition = position;
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

    private List<UiElement> GetElementNeighbours(int id1, List<UiElement> elements)
    {
        List<UiElement> neighbours = new List<UiElement>();
        List<UiElement> checkedNeighbours = new List<UiElement>();
        int numberOfElements = circuit.AdjancenceMatrix.GetLength(1); //number of elemens in the initial circuit +1

        for (int i = 1; i < numberOfElements; i++)
        {
            if (circuit.AdjancenceMatrix[i, 0] == id1)
            {
                for (int j = 1; j < numberOfElements; j++)
                {
                    if (circuit.AdjancenceMatrix[i, j] == 1)
                    {
                        UiElement e1 =elements.Find(x => x.id == circuit.AdjancenceMatrix[0, j]);
                        if (e1 != null)
                        {
                            neighbours.Add(e1);
                        }
                        else
                        {
                            List<UiElement>depthNeighbours=GetNextNeighbourExcluding(circuit.AdjancenceMatrix[0, j], id1, elements);
                            neighbours.AddRange(depthNeighbours);
                        }
                    }
                }
            }
        }

        return neighbours;
    }
    private List<UiElement> GetNextNeighbourExcluding(int id, int exclusionID, List<UiElement> elements)
    {
        List<UiElement> neighbours = new List<UiElement>();
        int numberOfElements = circuit.AdjancenceMatrix.GetLength(1); //number of elemens in the initial circuit +1

        for (int i = 1; i < numberOfElements; i++)
        {
            if (circuit.AdjancenceMatrix[i, 0] == id )
            {
                for (int j = 1; j < numberOfElements; j++)
                {
                    if ((circuit.AdjancenceMatrix[i, j] == 1) && (circuit.AdjancenceMatrix[i,0] != exclusionID))
                    {
                        UiElement e1 = elements.Find(x => x.id == circuit.AdjancenceMatrix[0, j]);
                        if(e1!= null)
                        {
                            neighbours.Add(e1);
                        }
                        else
                        {
                            neighbours.AddRange(GetNextNeighbourExcluding(circuit.AdjancenceMatrix[0, j], exclusionID, elements));
                        }

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
    public float NumberConstrain(float number)
    {
        int int_part = (int)(number % 2);
        return number / 2.0f - (int)(number / 2) + int_part;


    }
    public Circuit CircuitConstrain(Circuit circuit)
    {

        for (int i = 0; i < circuit.Elements.Count; i++)
        {
            circuit.Elements[i].value = NumberConstrain(circuit.Elements[i].value);

           
        }
        return circuit;
    }

}

