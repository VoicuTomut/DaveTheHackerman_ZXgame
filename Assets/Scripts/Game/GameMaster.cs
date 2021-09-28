using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    private bool isTimeStopped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame()
    {

    }

    public void PauseGame()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isTimeStopped = !isTimeStopped;
            if(isTimeStopped)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

    }
}
