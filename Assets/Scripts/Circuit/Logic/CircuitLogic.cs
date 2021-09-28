using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZxDungeon.Logic
{
    public class CircuitLogic 
    {
        List<Element> elements = new List<Element>();
        int[,] adjacencyMatrix;
        // Start is called before the first frame update
        
        public Circuit GenerateCircuit()
        {
            Element e1 = new Element(1,   ElementType.Input, 0,1);
            Element e2 = new Element(2,   ElementType.Input, 1,2);
            Element e3 = new Element(3,   ElementType.Input, 2,3);
            Element e4 = new Element(4,   ElementType.Input, 3,4);
            Element e5 = new Element(5,   ElementType.ZG, 8,3);
            Element e6 = new Element(6,   ElementType.ZR, 9,4);
            Element e7 = new Element(7,   ElementType.ZR, 11,2);
            Element e8 = new Element(8,   ElementType.ZG, 10,3);
            Element e9 = new Element(9,   ElementType.ZR, 13,1);
            Element e10 = new Element(10, ElementType.ZG, 12, 4);
            Element e11 = new Element(11, ElementType.ZG, 14, 3);
            Element e12 = new Element(12, ElementType.ZR, 15, 4);
            Element e13 = new Element(13, ElementType.ZR, 17, 1);
            Element e14 = new Element(14, ElementType.ZG, 16, 2);
            Element e15 = new Element(15, ElementType.Output, 1, 1);
            Element e16 = new Element(16, ElementType.Output, 1, 2);
            Element e17 = new Element(17, ElementType.Output, 1, 3);
            Element e18 = new Element(18, ElementType.Output, 1, 4);



            elements.Add(e1);
            elements.Add(e2);
            elements.Add(e3);
            elements.Add(e4);
            elements.Add(e5);
            elements.Add(e6);
            elements.Add(e7);
            elements.Add(e8);
            elements.Add(e9);
            elements.Add(e10);
            elements.Add(e11);
            elements.Add(e12);
            elements.Add(e13);
            elements.Add(e14);
            elements.Add(e15);
            elements.Add(e16);
            elements.Add(e17);
            elements.Add(e18);
            adjacencyMatrix = new int[19, 19]
                {{0,  1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18},
                 {1,  0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {2,  0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {3,  0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {4,  0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                 {5,  0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                 {6,  0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0},
                 {7,  0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                 {8,  0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                 {9,  1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                 {10, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
                 {11, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
                 {12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
                 {13, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0},
                 {14, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
                 {15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                 {16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                 {17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
                 {18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},};
            return new Circuit(elements, adjacencyMatrix);
        }
    }

}