using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject tutorial;
    public GameObject credits;

    public void GoToMain()
    {
        tutorial.SetActive(false);
        credits.SetActive(false);
        main.SetActive(true);
    }
    public void GoToTutorial()
    {
        credits.SetActive(false);
        main.SetActive(false);
        tutorial.SetActive(true);
    }
    public void GoToCredits()
    {
        tutorial.SetActive(false);
        main.SetActive(false);
        credits.SetActive(true);

    }


}
