using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

//Kirajzolandó prefabok típusai
public enum objects
{
    Player, Box, Wall, EndPoint, Ground
}

//Prefabokhoz típus rendelése
[System.Serializable]
public class LevelElement
{
    public objects elementType;
    public GameObject Prefab;
}

//Szintek kirajzolása az adatok lapján
public class LevelBuilder : MonoBehaviour
{
    //Betöltött szint ID-ja
    public static int currentLevel;
    public List<LevelElement> elements;
    //Betöltött szint
    public Level level;

    //Prefab típus szerint
    GameObject GetPrefab(objects o)
    {
        LevelElement levelElement = elements.Find(le => le.elementType == o);
        if (levelElement != null)
            return levelElement.Prefab;
        else return null;
    }

    //Szint léptetése
    public void nextLevel()
    {
        currentLevel++;
        if(currentLevel >= GetComponent<Levels>().levels.Count)
        {
            currentLevel = 0;
        }
    }

    //Elsõ nem kivitt szintre lépés
    public void continueLevel()
    {
        var completedData = staticCompleted.Completed;
        int iter;
        for (iter = 0; iter < GetComponent<Levels>().levels.Count; iter++)
            if (!completedData.Completed[iter])
                break;
        if (iter >= GetComponent<Levels>().levels.Count) iter = 0;
        currentLevel = iter;
    }

    //Szintek frissítése és elsõ szintre lépés
    public void NewGame()
    {
        staticCompleted.Completed.Completed = new List<bool>(new bool[25]);
        currentLevel = 0;
    }

    //Szint kivitt állapotra állítása
    public void setCompleted()
    {
        if (currentLevel < staticCompleted.Completed.Completed.Count && currentLevel >= 0)
            staticCompleted.Completed.Completed[currentLevel] = true;
    }

    //Szint beállítása adott ID-val
    public void setLevel(int level)
    {
        if (level < GetComponent<Levels>().levels.Count && level >= 0)
        {
            currentLevel = level;
        }
    }

    //Játék elemeinek létrehozása a képernyõn
    private void createObjects(List<Vector2> coords, objects type, Vector2 corner)
    {
        foreach (var coord in coords)
        {
            GameObject prefab = GetPrefab(type);
            if (prefab)
            {
                Instantiate(prefab, new Vector3(corner.x + coord.x, corner.y + coord.y, 0), Quaternion.identity);
            }
        }
    }

    //Kezdetben jó helyen lévõ dobozok állapotának beállítása
    private void PlacedBoxAtStart(Vector2 corner)
    {
        Box[] boxes = FindObjectsOfType<Box>();
        foreach( var box in boxes)
        {
            Vector2 boxCoord = new Vector2(box.transform.position.x, box.transform.position.y) - corner;
            foreach (var ep in level.EndPoints)
                if (ep == boxCoord) box.setPlaced();
        }
    }

    //Pálya felépítése a szint adataiból
    public void Build()
    {
        level = GetComponent<Levels>().levels[currentLevel];
        Vector2 corner = new Vector2(-level.Width / 2, -level.Height / 2);

        //Minden onjektumtípusra
        createObjects(level.Walls, objects.Wall, corner);
        createObjects(level.Boxes, objects.Box, corner);
        createObjects(level.Grounds, objects.Ground, corner);
        createObjects(level.EndPoints, objects.EndPoint, corner);

        PlacedBoxAtStart(corner);

        //Játékos elhelyezése
        GameObject player = GetPrefab(objects.Player);
        if (player)
        {
            Instantiate(player, new Vector3(corner.x + level.Player.x, corner.y + level.Player.y, 0), Quaternion.identity);
        }
    }
}
