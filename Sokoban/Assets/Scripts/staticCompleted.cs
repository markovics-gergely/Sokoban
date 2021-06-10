using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class staticCompleted
{
    //Adatok el�r�si �tja
    private static readonly string filePath = Application.persistentDataPath + "/completed.json";

    //K�sz szintek t�rol�sa
    private static CompleteLevel _completed;
    public static CompleteLevel Completed
    {
        get
        {
            if(_completed == null)
            {
                _completed = loadFromFile();
            }
            return _completed;
        }
        set
        {
            _completed = value;
        }
    }

    //K�sz szintek lement�se
    public static void saveToFile()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Create);
        if (Completed == null)
        {
            Completed = new CompleteLevel();
            Completed.Completed = new List<bool>(new bool[25]);
        }
        formatter.Serialize(stream, Completed);
        stream.Close();
    }

    //K�sz szintek bet�lt�se
    public static CompleteLevel loadFromFile()
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);
            CompleteLevel newCompl = (CompleteLevel)formatter.Deserialize(stream);
            stream.Close();
            if(newCompl.Completed.Count != 25) newCompl.Completed = new List<bool>(new bool[25]);
            return newCompl;
        }
        else
        {
            //Ha nincs f�jl akkor t�lts�k be �resen
            CompleteLevel newCompl = new CompleteLevel();
            newCompl.Completed = new List<bool>(new bool[25]);
            return newCompl;
        }
    }
}
