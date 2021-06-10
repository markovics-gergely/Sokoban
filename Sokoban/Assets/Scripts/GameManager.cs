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

    //Mozg� j�t�kos
    private Player Player;
    //Egyszerre egy gombnyom�st �rz�keljen
    private bool movementReady;

    private bool inMenu = false;

    private void Start()
    {
        //Ind�t�si m�d v�laszt�sa
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
        //Men� megnyitva
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
            //Egys�gmozg�s beolvas�sa
            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            movement.Normalize();

            //Ha volt teljes gombnyom�s
            if (movement.sqrMagnitude > 0.5)
            {
                if (movementReady)
                {
                    movementReady = false;
                    Player.Move(movement);
                    nextButton.SetActive(isLevelCleared());
                }
            }
            //Ha m�r nincs mozg�s, �rz�kelje �jra a gombokat
            else movementReady = true;

            inMenu = Input.GetKeyDown(KeyCode.Return);
            //Men� megnyit�sa
            if (inMenu)
            {
                EventSystem.current.SetSelectedGameObject(resetButton);
                Camera.main.backgroundColor = new Color(80.0f / 255, 57.0f / 255, 36.0f / 255);
                PauseBanner.gameObject.SetActive(true);
            }
        }
        
    }

    //Ugr�s a k�vetkez� p�ly�ra
    public void NextLevel()
    {
        //�j p�ly�n�l nem lehet r�gt�n tov�bb l�pni
        nextButton.SetActive(false);
        builder.nextLevel();
        StartCoroutine(ResetSceneAsync());
        LevelText.text = "Level " + (LevelBuilder.currentLevel + 1);
    }

    //Ugr�s az els� nem megoldott p�ly�ra
    public void Continue()
    {
        //�j p�ly�n�l nem lehet r�gt�n tov�bb l�pni
        nextButton.SetActive(false);
        builder.continueLevel();
        StartCoroutine(ResetSceneAsync());
        LevelText.text = "Level " + (LevelBuilder.currentLevel + 1);
    }

    public void NewGame()
    {
        //�j p�ly�n�l nem lehet r�gt�n tov�bb l�pni
        nextButton.SetActive(false);
        builder.NewGame();
        StartCoroutine(ResetSceneAsync());
        LevelText.text = "Level " + (LevelBuilder.currentLevel + 1);
    }

    //P�lya el�lr�l kezd�se
    public void ResetScene()
    {
        StartCoroutine(ResetSceneAsync());
    }

    //P�lya tiszt�t�sa �s �jrat�lt�se
    public IEnumerator ResetSceneAsync()
    {
        //Main mellett legal�bb egy LevelScene is fut
        if(SceneManager.sceneCount > 1)
        {
            //LevelScene GameObjectjeinek t�rl�se
            AsyncOperation asyncClear = SceneManager.UnloadSceneAsync("LevelScene");
            while (!asyncClear.isDone)
            {
                yield return null;
            }
            Resources.UnloadUnusedAssets();
        }

        //Level �jra bet�lt�se frissen
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));
        //Elemek l�trehoz�sa
        builder.Build();
        Player = FindObjectOfType<Player>();

        //Men� deaktiv�l�sa
        inMenu = false;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        Camera.main.backgroundColor = new Color(140.0f / 255, 98.0f / 255, 61.0f / 255);
        PauseBanner.gameObject.SetActive(false);
    }

    //P�lya kivitel�nek ellen�rz�se
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
