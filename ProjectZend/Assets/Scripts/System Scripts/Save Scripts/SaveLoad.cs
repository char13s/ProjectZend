using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
public class SaveLoad
{
    public static event UnityAction load;
    private static string path;

    public static string Path { get => path; set => path = value; }

    public static void Save(List<SkillSaver> skillsList,Stats stats)
    {
        Debug.Log("saves");
        BinaryFormatter bf = new BinaryFormatter();
        //string path=Application.persistentDataPath + "/savedGames.gd";
        FileStream file = new FileStream(Path,FileMode.Create);
        Game data =new Game(skillsList,stats);
        bf.Serialize(file, data);
        file.Close();
    }
    public static bool DoesFileExist(string path) {
        return File.Exists(path);
    }
    public static void DeleteFile(string path) {

        //FileStream file = File.Open(path, FileMode.Open);
        Save(new List<SkillSaver>(),new Stats());
        //File.Delete(path);
        //file.Dispose();
    }
    public static Game Load()
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            Game data =bf.Deserialize(file) as Game; 
            file.Close();
        return data;
        }
        else
        {
            Debug.Log("no loads, saves");
            Save(new List<SkillSaver>(), new Stats());
            return null;
        }
    }
    
}
