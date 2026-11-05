using LitJson;
using System.IO;
using UnityEngine;

public enum JsonType
{
    JsonUtlity,
    LitJson
}
public class JsonMgr
{
    private static JsonMgr instance=new JsonMgr();
    public static JsonMgr Instance=>instance;
    private JsonMgr() { }

    //存储json文件
    public void SaveData(object data,string fileName,JsonType jsonType=JsonType.LitJson)
    {
        string path=Application.persistentDataPath+"/"+fileName+".json";

        string jsonStr = "";
        switch (jsonType)
        {
            case JsonType.JsonUtlity:
                jsonStr=JsonUtility.ToJson(data);               
                break;
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(data);
                break;
        }
        File.WriteAllText(path, jsonStr);
    }

    //读取json文件
    public T LoadData<T>(string fileName, JsonType jsonType = JsonType.LitJson) where T : new() 
    {
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        if (!File.Exists(path))
            path= Application.persistentDataPath + "/" + fileName + ".json";
        if (!File.Exists(path))
        {
            Debug.Log($"没有找到相应的{fileName}的json文件");
            return new T();
        }

        string jsonStr = File.ReadAllText(path);
        T data = default(T);
        switch (jsonType)
        {
            case JsonType.JsonUtlity:
                data=JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data=JsonMapper.ToObject<T>(jsonStr);
                break;
        }
        return data;
    }
}
