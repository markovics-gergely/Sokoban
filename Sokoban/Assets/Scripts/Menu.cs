using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    //Játék folytatása
    public void Play()
    {
        staticSetter.startMode = staticSetter.mode.CONTINUE;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Új játék kezdése
    public void NewGame()
    {
        staticSetter.startMode = staticSetter.mode.NEWGAME;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Alkalmazás bezárása
    public void Quit()
    {
        Application.Quit();
    }

    public void OnApplicationQuit()
    {
        staticCompleted.saveToFile();
    }
}
