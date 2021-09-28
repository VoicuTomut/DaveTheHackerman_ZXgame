using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZxDungeon.Logic
{
    /// <summary>
    /// Representation of the quantum operation. 
    /// </summary>
    public class Circuit
    {
        /// <summary>
        /// Elements contained in the graph/quantum operation.
        /// </summary>
        public List<Element> Elements { get; }

        /// <summary>
        /// Dictates how elements are connected together
        /// </summary>
        public int[,] AdjancenceMatrix { get; }

        /// <summary>
        /// Constructor of the Graph class
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="adjancenceMatrix"></param>
        public Circuit(List<Element> Elements, int[,] AdjancenceMatrix)
        {
            this.Elements = Elements;
            this.AdjancenceMatrix = AdjancenceMatrix;
        }

        /// <summary>
        /// Adds the value of two elements together
        /// </summary>
        public Circuit FuseElements(Circuit circuit, int id1, int id2)
        {

            Element e1= circuit.Elements[0];
            Element e2 = circuit.Elements[1];

            for (int i =0; i < circuit.Elements.Count; i++)
            {
                if (circuit.Elements[i].id == id1)
                {
                   e1 = circuit.Elements[i];
                }
                if (circuit.Elements[i].id == id2)
                {
                    e2 = circuit.Elements[i];
                }
            }

            bool elementTypeFlag = true;
            if (e1.elementType!= e2.elementType)
            {
                elementTypeFlag = false;
            }

            List<int> e1Neighbours = GetNeighbours(circuit, id1);
            List<int> e2Neighbours = GetNeighbours(circuit, id2);

            bool neighbourFlag = false;
            for(int i = 0; i < e1Neighbours.Count; i++)
            {
                if (e1Neighbours[i] == id2)
                {
                    neighbourFlag = true;
                }
            }


            if (neighbourFlag && elementTypeFlag)
            {
                // add nodes values
                for (int i = 0; i < circuit.Elements.Count; i++)
                {
                    if (circuit.Elements[i].id == id1)
                    {
                        circuit.Elements[i].value = circuit.Elements[i].value + e2.value;
                    }
                }

                circuit = RemoveEdge(circuit, id1, id2);

                //enherit the edges 
                foreach (int neighboure2 in e2Neighbours)
                {
                    int commonFlag = 0;
                    foreach (int neighboure1 in e1Neighbours)
                    {
                        if (neighboure1 == neighboure2)
                        {
                            commonFlag = 1;
                        }
                    }

                    if (commonFlag == 0)
                    {
                        circuit = AddEdge(circuit, id1, neighboure2);
                    }
                }




                circuit = RemoveElement(circuit, id2);

            }

            return circuit;

        }


        /// <summary>
        /// Push the second node thru first one 
        /// </summary>
        public Circuit PushElements(Circuit circuit, int id1, int id2)
        {
            //Inprogres
            return circuit;

        }


        /// <summary>
        /// Changes the type of an element to another type
        /// </summary>
        public Circuit ChangeType(Circuit circuit, int id)
        {
            // change the elemebt type
            bool flag = true;
            for(int i = 0; i < circuit.Elements.Count; i++)
            {
                if (circuit.Elements[i].id == id)
                {
                    if (circuit.Elements[i].elementType== ElementType.ZG)
                    {
                        circuit.Elements[i].elementType = ElementType.ZR;
                    }
                    else if(circuit.Elements[i].elementType == ElementType.ZR)
                    {
                        circuit.Elements[i].elementType = ElementType.ZG;
                    }
                    else
                    {
                        circuit.Elements[i].elementType = circuit.Elements[i].elementType;
                        flag = false;
                    }
                }
            }

            if (flag)
            {

                
                // Add the H gate (beteen the element and neighbous
                List<int> elementNeighbours = GetNeighbours(circuit, id);
                foreach (int neighbour in elementNeighbours)
                {

                    // check for an empty id
                    int idtake = GetNewId(circuit);

                    Element hsquare = new Element(idtake, ElementType.Square, 0, 1);
                    int[,] hNeighbours = new int[neighbour, id];
                    circuit = AddNewElement(circuit, hsquare, hNeighbours);
                }
            }
            

            return circuit;
           
        }


        public int GetNewId(Circuit circuit)
        {
            // check for an empty id
            bool idtake = true;
            int newID = circuit.AdjancenceMatrix[0, circuit.AdjancenceMatrix.GetLength(1)-1];
            while (idtake)
            {
                newID = newID + 1;
                idtake = false;
                for (int i = 0; i < circuit.Elements.Count; i++)
                {
                    if (circuit.Elements[i].id == newID)
                    {
                        newID = newID + 1;
                        idtake = true;
                    }
                }
            }

            return newID;
        }

        /// <summary>
        /// Adds a new element to the circuit graph
        /// </summary>
        public Circuit AddNewElement(Circuit circuit, Element element , int[,] neighbours)
        {

            var elements = circuit.Elements;
            elements.Add(element);

            // create the new AdjancenceMatrix
            int numberOfElements = circuit.AdjancenceMatrix.GetLength(1); //number of elemens in the initial circuit +1
            int[,] newMatrix = new int[numberOfElements + 1, numberOfElements + 1];

            // extend the borders
            newMatrix[0, numberOfElements] = element.id;
            newMatrix[numberOfElements,0] = element.id;

            for (int i =0; i< numberOfElements; i++)
            {
                //Add new connection between neighbours and new element
                if (i != 0)
                {
                    int flag = 0;
                    for (int k = 0; k < neighbours.Length; k++)
                    {
                        if (circuit.AdjancenceMatrix[0, i] == k)
                        {
                            flag = 1;
                        }
                    }
                    newMatrix[numberOfElements , i] = flag;
                    newMatrix[i, numberOfElements ] = flag;
                }

                //Keep the old connections 
                for (int j = i; j < numberOfElements; j++)
                {
                    newMatrix[i,j] = circuit.AdjancenceMatrix[i,j];
                    newMatrix[j, i] = circuit.AdjancenceMatrix[j, i];
                }
                newMatrix[i, i] = 0;
            }
            newMatrix[numberOfElements, numberOfElements] = 0;

            return new Circuit (elements, newMatrix);
        }

        /// <summary>
        /// Removes an element from the circuit  and adiacent edges 
        /// </summary>
        public Circuit  RemoveElement(Circuit circuit, int id)
        {

            List<Element> remainingElements = new List<Element>();

            //remouve the circuit elements list from the list 
            for( int i=0; i < circuit.Elements.Count; i++)
            {
                if (circuit.Elements[i].id != id)
                {
                    remainingElements.Add(circuit.Elements[i]);
                }
            }
            

            // create the new AdjancenceMatrix
            int numberOfElements = circuit.AdjancenceMatrix.GetLength(1);
            int[,] newMatrix = new int[numberOfElements - 1, numberOfElements - 1];

            //recreate the first line of the matrix and first row
            int k = 0; // the line index in new matrix
            int position = 0; //position of the outcast id
            for(int i=0;i< numberOfElements ; i++)
            {
                if (circuit.AdjancenceMatrix[0, i] != id)
                {
                    newMatrix[0, k] = circuit.AdjancenceMatrix[0, i];
                    newMatrix[k, 0] = circuit.AdjancenceMatrix[0, i];
                    k = k + 1;
                }
                else
                {
                    position = k;
                }
            }

            k = 1;
            //put the rest of the matrix in order 
            for (int i = 1; i < numberOfElements; i++)
            {

                if (circuit.AdjancenceMatrix[0, i] != id)
                {
                    int l = 1; // the row index in new matrix
                    for (int j = 1; j < numberOfElements ; j++)
                    {
                        if (circuit.AdjancenceMatrix[j, 0] != id)
                        {
                            newMatrix[k, l] = circuit.AdjancenceMatrix[i, j];
                
                            l = l + 1;
                        }
                    }
                     
                    newMatrix[k, k] = 0;
                    k = k + 1;
                }
            }
            

            return new Circuit(remainingElements, newMatrix);
        }


        /// <summary>
        /// Removes an edge from the circuit graph 
        /// </summary>
        public Circuit RemoveEdge(Circuit circuit, int id1,int id2)
        {

            List<Element> remainingElements = new List<Element>();



            // create the new AdjancenceMatrix
            int numberOfElements = circuit.AdjancenceMatrix.GetLength(1);
            int[,] newMatrix = new int[numberOfElements , numberOfElements ];

        
            //eliminate edge from matrix 
            for (int i = 0; i < numberOfElements; i++)
            {
                for (int j = i; j < numberOfElements; j++)
                {

                    newMatrix[i, j] = circuit.AdjancenceMatrix[i, j];
                    newMatrix[j, i] = circuit.AdjancenceMatrix[i, j];
                }
            }

            for (int i = 0; i < numberOfElements; i++)
            {
                if (circuit.AdjancenceMatrix[0, i] == id1)
                {
                    for (int j = 0; j < numberOfElements; j++)
                    {
                        if (j == id2)
                        {
                            newMatrix[i, j] = 0;
                            newMatrix[j, i] = 0; 
                        }

                    }
                }
            }

            return new Circuit(circuit.Elements, newMatrix);
        }


        /// <summary>
        /// Add an edge from the circuit graph 
        /// </summary>
        public Circuit AddEdge(Circuit circuit, int id1, int id2)
        {

            List<Element> remainingElements = new List<Element>();



            // create the new AdjancenceMatrix
            int numberOfElements = circuit.AdjancenceMatrix.GetLength(1);
            int[,] newMatrix = new int[numberOfElements, numberOfElements];


            //eliminate edge from matrix 
            for (int i = 0; i < numberOfElements; i++)
            {
                for (int j = i; j < numberOfElements; j++)
                {

                    newMatrix[i, j] = circuit.AdjancenceMatrix[i, j];
                    newMatrix[j, i] = circuit.AdjancenceMatrix[i, j];
                }
            }

            for (int i = 0; i < numberOfElements; i++)
            {
                if (circuit.AdjancenceMatrix[0, i] == id1)
                {
                    for (int j = 0; j < numberOfElements; j++)
                    {
                        if (j == id2)
                        {
                            newMatrix[i, j] = 1;
                            newMatrix[j, i] = 1;
                        }

                    }
                }
            }

            return new Circuit(circuit.Elements, newMatrix);
        }


        /// <summary>
        /// Return the list of the id's of the neighbouring nodes
        /// </summary>
        public List<int> GetNeighbours(Circuit circuit, int id1)
        {
            List<int> neighbours= new List<int>();
            int numberOfElements = circuit.AdjancenceMatrix.GetLength(1); //number of elemens in the initial circuit +1

            for(int i=1;i< numberOfElements; i++)
            {
                if (circuit.AdjancenceMatrix[i,0] == id1)
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

    }
}
