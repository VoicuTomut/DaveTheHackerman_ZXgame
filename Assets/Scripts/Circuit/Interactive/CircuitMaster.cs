using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZxDungeon.Logic;
using ZxDungeon.UI;

namespace ZxDungeon.Interactive
{
    public class CircuitMaster : MonoBehaviour
    {
        public GameObject canvasObject;
        public GameObject visualCircuitObject;
        Circuit circuit;
        private CircuitLogic circuitLogic;
       

        // Start is called before the first frame update
        void Start()
        {
            circuitLogic = new CircuitLogic();
            circuit = circuitLogic.GenerateCircuit();
            circuit = circuit.FuseElements(circuit, 5, 14);
            GameObject visCircuit = Instantiate(visualCircuitObject, transform);
            GameObject canvas = Instantiate(canvasObject, transform);
            VisualCircuit cv = visCircuit.GetComponent<VisualCircuit>();
            cv.circuit = circuit;
            cv.canvas = canvas.GetComponent<Canvas>();
        }


        // Update is called once per frame
        void Update()
        {

        }

    }
}
