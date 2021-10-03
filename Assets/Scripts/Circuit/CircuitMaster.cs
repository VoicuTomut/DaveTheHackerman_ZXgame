using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CircuitMaster : MonoBehaviour
{

    Circuit circuit;
    public VisualCircuit visualCicuit;
    public Canvas circuitCanvas;
    public Level level;
    public Action<float> OnTerminate;


    //Start is called before the first frame update
    void Start()
    {
        visualCicuit.canvas = circuitCanvas;
    }

    public void Initialize(int dif)
    {
        circuit = new Circuit();
        circuit = GetRandomCircuit(circuit, dif);
        visualCicuit.circuit = new Circuit();
        visualCicuit.circuit = circuit;

        visualCicuit.InitCircuit(circuit);
    }

  public Circuit GetRandomCircuit(Circuit circuit, int dif )
    {
        
        int val = UnityEngine.Random.Range(0, 100);

        if (val < 10)
        {
           circuit = circuit.OneLineCircuitV1((4*dif+2),0.5);
        }
        else if (val<20)
        {
            circuit = circuit.OneLineCircuitV2(4*dif+2,0.5);
        }
        else if (val<40)
        {
            circuit = circuit.OneLineCircuitV3(4*dif+2, dif,dif);
        }
        else if (val< 60)
        {
            circuit = circuit.TwoLineCircuitv1(10,dif,dif*2);
        }
        else if (val < 100)
        {
            circuit = circuit.TreeLineCircuitv1((7), 4);
        }


        return circuit;
    }

    public float OptimisationScore(Circuit circuitInit, Circuit circuitFin)
    {
        List<Element> outputs = circuitInit.Elements.Where(x => x.elementType == ElementType.Output).ToList();
        float baseNumber = 2 * outputs.Count;
        float fin_nr = circuitFin.Elements.Count() - baseNumber;
        float reduced = (circuitInit.Elements.Count - baseNumber - fin_nr);
        float ideal = (circuitInit.Elements.Count - baseNumber - 2);
        float floatullumata = reduced / ideal;

        string a = string.Empty;
        return fin_nr == 1 ? 1 : (float)(floatullumata);


    }

    public void Terminate()
    {
         level.EndCircuitGame(OptimisationScore(circuit, visualCicuit.circuit));
    }
}

