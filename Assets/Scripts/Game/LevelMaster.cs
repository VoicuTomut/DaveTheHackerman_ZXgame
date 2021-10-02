using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaster : MonoBehaviour
{
    public GameObject[] Levels;
    public GameObject transitionScreenObject;
    public TransitionScreen transitionScreen;
    private int currentLevelIndex = 0;
    private GameObject currentLevel;
    // Start is called before the first frame update


    
    private void StartLevel(int id)
    {
        currentLevel = Levels[id];
        //currentLevel.GetComponent<Level>().SetLevelMaster(this);
    }

    public void OnLevelClear(int id)
    {
        StartCoroutine(TransitionScreen(id));      
    }

    IEnumerator TransitionScreen(int id)
    {
        transitionScreenObject.SetActive(true);  
        yield return new WaitForSeconds(transitionScreen.OnClear());
        yield return new WaitForSeconds(transitionScreen.OnSwitch());
        currentLevel = null;
        //Destroy(currentLevel);
        int index = id++;
        if (!(index > Levels.Length)) currentLevel = Instantiate(Levels[id + 1]);
        //currentLevel.GetComponent<Level>().SetLevelMaster(this);
        yield return new WaitForSeconds(transitionScreen.OnStart());
        transitionScreenObject.SetActive(false);
    }

    void OnEnable()
    {
        if(!currentLevel)
        {
            transitionScreenObject.SetActive(false);
            //Levels[currentLevelIndex].GetComponent<Level>().SetLevelMaster(this);
            StartLevel(currentLevelIndex);
        }
    }
    //void OnDisable()
    //{
    //    Destroy(currentLevel);
    //}
}
