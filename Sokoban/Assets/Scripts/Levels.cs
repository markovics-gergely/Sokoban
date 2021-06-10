using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Pálya adatai
[System.Serializable]
public class Level
{
    public List<Vector2> Walls = new List<Vector2>();
    public List<Vector2> Boxes = new List<Vector2>();
    public List<Vector2> Grounds = new List<Vector2>();
    public List<Vector2> EndPoints = new List<Vector2>();
    public Vector2 Player = new Vector2();

    public int Height;
    public int Width;

    public int ID;
}

//Kivitt pályák adatai
[System.Serializable]
public class CompleteLevel
{
    public List<bool> Completed = new List<bool>();
}

public class Levels : MonoBehaviour
{
    public string levelFileBase;
    //Szintek listája
    public List<Level> levels = new List<Level>();

    public const int MaxLevel = 25;

    //Pályák betöltése
    private void Awake()
    {
        for (int id = 1; id <= MaxLevel; id++)
        {
            TextAsset textAsset = (TextAsset)Resources.Load(levelFileBase + id);
            if (!textAsset) return;

            var newLevel = JsonUtility.FromJson<Level>(textAsset.text);
            if(newLevel.ID != id) return;

            fillGround(newLevel.Player, newLevel, 0);
            levels.Add(newLevel);
        }
    }

    //Pálya padlójának kitöltése
    private void fillGround(Vector2 pos, Level level, int iter)
    {
        //Ha hiba lenne a falon
        if (++iter > 50) return;

        level.Grounds.Add(pos);

        Vector2 up = pos + new Vector2(0, 1);
        Vector2 down = pos + new Vector2(0, -1);
        Vector2 right = pos + new Vector2(1, 0);
        Vector2 left = pos + new Vector2(-1, 0);

        if (!level.Walls.Contains(up) && !level.Grounds.Contains(up)) fillGround(up, level, iter);
        if (!level.Walls.Contains(down) && !level.Grounds.Contains(down)) fillGround(down, level, iter);
        if (!level.Walls.Contains(right) && !level.Grounds.Contains(right)) fillGround(right, level, iter);
        if (!level.Walls.Contains(left) && !level.Grounds.Contains(left)) fillGround(left, level, iter);
    }
}
