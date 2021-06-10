using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    //J�t�k folytat�sa
    public void Play()
    {
        staticSetter.startMode = staticSetter.mode.CONTINUE;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //�j j�t�k kezd�se
    public void NewGame()
    {
        staticSetter.startMode = staticSetter.mode.NEWGAME;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Alkalmaz�s bez�r�sa
    public void Quit()
    {
        Application.Quit();
    }

    public void OnApplicationQuit()
    {
        staticCompleted.saveToFile();
    }
}
