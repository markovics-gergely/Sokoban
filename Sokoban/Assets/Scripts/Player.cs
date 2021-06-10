using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool Move(Vector2 dir)
    {
        //Ne mozogjon diagon�lisan
        if (Mathf.Abs(dir.y) < 0.5) dir.y = 0;
        else dir.x = 0;

        //Egys�gmozg�st hajtunk v�gre
        dir.Normalize();

        //Ha szabad hely mozogjon oda
        if(canMove(transform.position, dir))
        {
            transform.Translate(dir);
            return true;
        }
        else return false;
    }

    private bool canMove(Vector3 pos, Vector2 dir)
    {
        Vector2 newPos = new Vector2(pos.x, pos.y) + dir;

        //Falnak ne mehessen neki
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach(GameObject w in walls)
        {
            Vector2 wallPos = new Vector2(w.transform.position.x, w.transform.position.y);
            if (wallPos == newPos) return false;
        }

        //Dobozokat tolja el, ha tudja
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach(GameObject b in boxes)
        {
            Vector2 boxPos = new Vector2(b.transform.position.x, b.transform.position.y);
            //Ha doboz �tban van
            if(boxPos == newPos)
            {
                //Mozogjon a doboz is, ha l�tezik
                Box box = b.GetComponent<Box>();
                if (box && box.Move(dir)) return true;
                else return false;
            }
        }
        //Akad�ly n�lk�l egyszer�en mozoghat
        return true;
    }
}
