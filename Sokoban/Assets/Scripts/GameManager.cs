using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LevelBuilder builder;
    public GameObject nextButton;
    public GameObject resetButton;
    public GameObject menuButton;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI PauseBanner;

    //Mozgó játékos
    private Player Player;
    //Egyszerre egy gombnyomást érzékeljen
    private bool movementReady;

    private bool inMenu = false;

    private void Start()
    {
        //Indítási mód választása
        if (staticSetter.startMode == staticSetter.mode.CONTINUE)
            Continue();
        else if (staticSetter.startMode == staticSetter.mode.NEWGAME)
            NewGame();
        else if(staticSetter.startMode == staticSetter.mode.CHOOSE)
        {
            StartCoroutine(ResetSceneAsync());
            LevelText.text = "Level " + (LevelBuilder.currentLevel + 1);
        }     

        nextButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Menü megnyitva
        if (inMenu)
        {
            inMenu = !Input.GetKeyDown(KeyCode.Backspace);
            if (!inMenu) { 
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
                Camera.main.backgroundColor = new Color(140.0f / 255, 98.0f / 255, 61.0f / 255);
                PauseBanner.gameObject.SetActive(false);
            }
        }
        else
        {
            //Egységmozgás beolvasása
            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            movement.Normalize();

            //Ha volt teljes gombnyomás
            if (movement.sqrMagnitude > 0.5)
            {
                if (movementReady)
                {
                    movementReady = false;
                    Player.Move(movement);
                    nextButton.SetActive(isLevelCleared());
                }
            }
            //Ha már nincs mozgás, érzékelje újra a gombokat
            else movementReady = true;

            inMenu = Input.GetKeyDown(KeyCode.Return);
            //Menü megnyitása
            if (inMenu)
            {
                EventSystem.current.SetSelectedGameObject(resetButton);
                Camera.main.backgroundColor = new Color(80.0f / 255, 57.0f / 255, 36.0f / 255);
                PauseBanner.gameObject.SetActive(true);
            }
        }
        
    }

    //Ugrás a következõ pályára
    public void NextLevel()
    {
        //Új pályánál nem lehet rögtön tovább lépni
        nextButton.SetActive(false);
        builder.nextLevel();
        StartCoroutine(ResetSceneAsync());
        LevelText.text = "Level " + (LevelBuilder.currentLevel + 1);
    }

    //Ugrás az elsõ nem megoldott pályára
    public void Continue()
    {
        //Új pályánál nem lehet rögtön tovább lépni
        nextButton.SetActive(false);
        builder.continueLevel();
        StartCoroutine(ResetSceneAsync());
        LevelText.text = "Level " + (LevelBuilder.currentLevel + 1);
    }

    public void NewGame()
    {
        //Új pályánál nem lehet rögtön tovább lépni
        nextButton.SetActive(false);
        builder.NewGame();
        StartCoroutine(ResetSceneAsync());
        LevelText.text = "Level " + (LevelBuilder.currentLevel + 1);
    }

    //Pálya elölrõl kezdése
    public void ResetScene()
    {
        StartCoroutine(ResetSceneAsync());
    }

    //Pálya tisztítása és újratöltése
    public IEnumerator ResetSceneAsync()
    {
        //Main mellett legalább egy LevelScene is fut
        if(SceneManager.sceneCount > 1)
        {
            //LevelScene GameObjectjeinek törlése
            AsyncOperation asyncClear = SceneManager.UnloadSceneAsync("LevelScene");
            while (!asyncClear.isDone)
            {
                yield return null;
            }
            Resources.UnloadUnusedAssets();
        }

        //Level újra betöltése frissen
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));
        //Elemek létrehozása
        builder.Build();
        Player = FindObjectOfType<Player>();

        //Menü deaktiválása
        inMenu = false;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        Camera.main.backgroundColor = new Color(140.0f / 255, 98.0f / 255, 61.0f / 255);
        PauseBanner.gameObject.SetActive(false);
    }

    //Pálya kivitelének ellenõrzése
    public bool isLevelCleared()
    {
        Box[] boxes = FindObjectsOfType<Box>();
        foreach (var b in boxes)
            if (!b.Placed) return false;
        builder.setCompleted();
        return true;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnApplicationQuit()
    {
        staticCompleted.saveToFile();
    }
}
