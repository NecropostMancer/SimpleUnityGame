using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetsLoadSystem
{

    private static string root = Application.persistentDataPath + "/";

    private static string fileName = "save";
    public static Object LoadJson(string path)
    {
        
        return new Object();
    }

    public static Object LoadSpawnManifest(string path)
    {
        return new Object();
    }

    public static Object LoadSave(int order)
    {
        return new Object();
        //return JsonUtility.FromJson<ProfileData>(LoadTextFromFile(path));
        //JsonUtility.FromJson;
    }

    public static void ReadSave(int order)
    {

    }

    private static string LoadTextFromFile(string path)
    {
        var streamReader = new StreamReader(path,System.Text.Encoding.UTF8);
        string tmp = streamReader.ReadToEnd();
        streamReader.Close();
        return tmp ;
        
    }
    
    private static void WriteTextIntoFile(string path, string text)
    {
        var streamWriter = new StreamWriter(path, false, System.Text.Encoding.UTF8);
        streamWriter.Write(text);
        streamWriter.Close();
    }
}
