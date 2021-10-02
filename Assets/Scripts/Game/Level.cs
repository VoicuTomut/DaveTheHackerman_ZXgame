using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private int waveIndex=0;
    private int maxWave;
    bool isCleared = false;
    public Wave[] waves;

    PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInChildren<PlayerController>();
        maxWave = waves.Length;
    }

    // Update is called once per frame
    void Update()
    {

        if(waveIndex<maxWave && maxWave > 1)
        {
            CheckCurrentWave();
        }
        else 
        {
        bool flag = false;
     
            foreach(GameObject enemy in waves[maxWave-1].enemies)
            if(enemy.activeInHierarchy)
            {
                flag = true;
                break;
            }
        

            if (!flag && !isCleared)
            {
                player.rigidbody.velocity = Vector3.zero;
                player.enabled = false;
                GameMaster.instace.OnLevelClear();
                isCleared = true;
            }
        }
    }

    private void CheckCurrentWave()
    {
        bool flag = false;
        foreach (GameObject enemy in waves[waveIndex].enemies)
            if (enemy.activeInHierarchy)
            {
                flag = true;
                break;
            }
        if(!flag && (waveIndex+1)<maxWave)
        {
            SpawnNewWave(waveIndex+1);
            waveIndex++;
        }
    }

    private void SpawnNewWave(int index)
    {
        foreach(GameObject obj in waves[index].enemies)
        {
            obj.SetActive(true);
        }
       
    }
}
