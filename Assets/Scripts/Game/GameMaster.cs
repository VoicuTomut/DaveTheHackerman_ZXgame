using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject shooterGame;
    public GameObject circuitGame;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    private GameObject currentActive;
    private PlayerController player;
    private bool isTimeStopped = false;

    // Start is called before the first frame update
    void Start()
    {
        SetAllInactive();
        currentActive = mainMenu;
        currentActive.SetActive(true);
    }

    public void StartGame()
    {
        SetAllInactive();
        currentActive.SetActive(false);
        currentActive = shooterGame;
        currentActive.SetActive(true);
        player = FindObjectOfType<PlayerController>();
    }

    public void BackToMainMenu()
    {
        SetAllInactive();
        currentActive.SetActive(false);
        currentActive = mainMenu;
        currentActive.SetActive(true);
    }

    public void PauseUnpauseGame()
    {
        isTimeStopped = !isTimeStopped;
        if (isTimeStopped)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            player.enabled = false;
        }
        else
        {
            pauseMenu.SetActive(false);
            player.enabled = true;
            Time.timeScale = 1;

        }
    }




    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpauseGame();
        }

    }

    private void SetAllInactive()
    {
        mainMenu.SetActive(false);
        shooterGame.SetActive(false);
        pauseMenu.SetActive(false);
        circuitGame.SetActive(false);
    }
}
