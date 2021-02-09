using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetsLoadSystem
{

    private static string sm_Root = Application.persistentDataPath + "/";

    private static string sm_FileName = "save";
    public static Object LoadJson(string path)
    {
        
        return new Object();
    }

    public static Object LoadSpawnManifest(string path)
    {
        return new Object();
    }

    public static ProfileData LoadSave(int order)
    {
        return JsonUtility.FromJson<ProfileData>(LoadTextFromFile(sm_Root + sm_FileName + order.ToString() + ".save"));
    }

    public static void UpdateSave(int order , ProfileData profileData)
    {
        
        WriteTextIntoFile(sm_Root + sm_FileName + order.ToString() + ".save", JsonUtility.ToJson(profileData));
        
    }

    public static GameSetting LoadSetting()
    {
        return JsonUtility.FromJson<GameSetting>(LoadTextFromFile(sm_Root + sm_FileName + ".setting"));
    }

    public static void UpdateSetting(GameSetting settingData)
    {
        WriteTextIntoFile(sm_Root + sm_FileName + ".setting", JsonUtility.ToJson(settingData));
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
