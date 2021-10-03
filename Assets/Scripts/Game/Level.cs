using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Wave[] waves;
    [SerializeField]
    PlayerController player;
    [SerializeField]
    CircuitMaster circuitMaster;
    [SerializeField]
    public Camera environmentCamera;
    public Camera circuitCamera;
    public GameObject playerDeathParticles;

    private int waveIndex=0;
    private int maxWave;
    bool isCleared = false;

    // Start is called before the first frame update
    void Start()
    {
        maxWave = waves.Length;
    }

    // Update is called once per frame
    void Update()
    {

            if (waveIndex < maxWave && maxWave > 1)
            {
                CheckCurrentWave();
            }
            else
            {
                bool flag = false;

                foreach (GameObject enemy in waves[maxWave - 1].enemies)
                    if (enemy.activeInHierarchy)
                    {
                        flag = true;
                        break;
                    }


                if (!flag && !isCleared)
                {
                    player.rigidbody.velocity = Vector3.zero;
                    player.enabled = false;
                    GameMaster.instance.OnLevelClear();
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

    public void KillPlayer()
    {
        GameObject go = Instantiate(playerDeathParticles, player.transform.position, Quaternion.identity);
        DeathEffects de = go.GetComponent<DeathEffects>();
        de.PlayEffects();
        player.gameObject.SetActive(false);
    }



    public void StartCircuitGame()
    {
        Time.timeScale = 0;
        environmentCamera.gameObject.SetActive(false);
        circuitCamera.tag = "MainCamera";
        circuitMaster.gameObject.SetActive(true);
        circuitMaster.Initialize(2);
        player.enabled = false;
    }
    public void EndCircuitGame(float percentage)
    {
        circuitMaster.gameObject.SetActive(false);
        environmentCamera.tag = "MainCamera";
        environmentCamera.gameObject.SetActive(true);
        player.enabled = true;
        Time.timeScale = 1;
        player.SafetyBlast(percentage);

    }
}
