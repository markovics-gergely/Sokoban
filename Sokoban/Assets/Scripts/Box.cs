using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public bool Placed;

    //Doboz mozgat�sa
    public bool Move(Vector2 dir)
    {
        //Ha nincs semmi �tban, mozogjon
        if (canMove(transform.position, dir))
        {
            transform.Translate(dir);
            isPlaced();
            return true;
        } 
        else return false;
    }

    //Ellen�rzi hely�n van-e
    private void isPlaced()
    {
        GameObject[] endpoints = GameObject.FindGameObjectsWithTag("EndPoint");
        foreach(var ep in endpoints)
        {
            Vector2 epPos = new Vector2(ep.transform.position.x, ep.transform.position.y);
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            if(epPos == pos)
            {
                GetComponent<SpriteRenderer>().color = Color.green;
                Placed = true;
                return;
            }
        }
        GetComponent<SpriteRenderer>().color = Color.white;
        Placed = false;
    }

    //Megn�zi mozgathat�-e az ir�nyba
    private bool canMove(Vector3 pos, Vector2 dir)
    {
        Vector2 newPos = new Vector2(pos.x, pos.y) + dir;

        //Falnak ne mehessen neki
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject w in walls)
        {
            Vector2 wallPos = new Vector2(w.transform.position.x, w.transform.position.y);
            if (wallPos == newPos) return false;
        }

        //T�bb doboz m�r nem tolhat� egyszerre
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (GameObject b in boxes)
        {
            Vector2 boxPos = new Vector2(b.transform.position.x, b.transform.position.y);
            //Ha doboz �tban van
            if (boxPos == newPos) return false;
        }
        //Akad�ly n�lk�l egyszer�en mozoghat
        return true;
    }

    public void setPlaced()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        Placed = true;
    }
}
