using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaster : MonoBehaviour
{
    public GameObject[] Levels;
    public GameObject transitionScreen;
    private int currentLevel = 0;
    // Start is called before the first frame update
    void Start()
    {
        Levels[currentLevel].GetComponent<Level>().SetLevelMaster(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel(int id)
    {
        transitionScreen.SetActive(true);
        Destroy(Levels[id]);

        int index = id++;
        if(!(index>Levels.Length))
        Instantiate(Levels[id + 1]);
    }
}
