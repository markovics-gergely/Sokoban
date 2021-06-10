using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public int levelID;

    //Szint választása
    public void setLevel()
    {
        staticSetter.startMode = staticSetter.mode.CHOOSE;
        LevelBuilder.currentLevel = levelID;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
