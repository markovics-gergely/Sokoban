using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    //Szintek színkódjainak betöltése
    public void loadColor()
    {
        LevelSelect[] lbuttons = FindObjectsOfType<LevelSelect>();
        var compl = staticCompleted.Completed;
        foreach (var b in lbuttons)
        {
            Debug.Log(compl.Completed[b.levelID]);
            if (compl.Completed[b.levelID])
            {
                b.GetComponent<Image>().color = Color.green;
            }
            else b.GetComponent<Image>().color = Color.red;
        }
    }
}
