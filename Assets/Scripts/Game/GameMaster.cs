using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public GameObject transitionScreenObject;
    public TransitionScreen transitionScreen;
    public GameObject pauseMenu;
    private PlayerController player;
    private bool isTimeStopped = false;

    public static GameMaster instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }
    //Start is called before the first frame update
    void Start()
    {
        transitionScreenObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpauseGame();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            player = FindObjectOfType<PlayerController>();
            isTimeStopped = !isTimeStopped;
            if (isTimeStopped)
            {
                //pauseMenu.SetActive(true);
                Time.timeScale = 0;
                if (player != null) player.enabled = false;
            }
            else
            {
                //pauseMenu.SetActive(false);
                if (player != null) player.enabled = true;
                Time.timeScale = 1;

            }
        }

    }
    public void StartGame()
    {
        StartCoroutine(LoadLevel(1));
    }

    public void OnLevelClear()
    {
        StartCoroutine(TransitionScreen());
    }

    IEnumerator LoadLevel(int index)
    {
        transitionScreenObject.SetActive(true);
        yield return new WaitForSeconds(transitionScreen.OnInit());
        SceneManager.LoadScene(index);
        yield return new WaitForSeconds(transitionScreen.OnStart());
        transitionScreenObject.SetActive(false);
    }
    IEnumerator TransitionScreen()
    {
        transitionScreenObject.SetActive(true);
        yield return new WaitForSeconds(transitionScreen.OnClear());
        yield return new WaitForSeconds(transitionScreen.OnSwitch());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return new WaitForSeconds(transitionScreen.OnStart());
        transitionScreenObject.SetActive(false);
    }
 


    public void BackToMainMenu()
    {
        PauseUnpauseGame();
        StartCoroutine(LoadLevel(0));

    }
    public void ResetToMainMenu()
    {
        StartCoroutine(LoadLevel(0));

    }


    public void PauseUnpauseGame()
    {
        if (SceneManager.GetActiveScene().buildIndex >0)
        {
            player = FindObjectOfType<PlayerController>();
            isTimeStopped = !isTimeStopped;
            if (isTimeStopped)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                if (player != null) player.enabled = false;
            }
            else
            {
                pauseMenu.SetActive(false);
                if (player != null) player.enabled = true;
                Time.timeScale = 1;

            }
        }
    }



}
