using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数据管理类
/// </summary>
public class DataMgr 
{
    private static DataMgr instance=new DataMgr();
    public static DataMgr Instance => instance;

    //记录选择的角色
    public RoleInfo NowSelRole;

    //音效数据相关
    public MusicData musicData;

    //玩家数据
    public PlayerData playerData;

    //角色数据相关
    public List<RoleInfo> roleInfoList;

    //所有场景数据
    public List<SceneInfo> sceneInfoList;

    //怪物类
    public List<MonsterInfo> monsterInfoList;
    
    //防御塔数据
    public List<TowerInfo> towerInfoList;
    private DataMgr() 
    {
        //读取存储的音乐数据文件
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        //获取玩家数据
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        //读取角色数据
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        //读取场景数据
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        //读取怪物数据
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        //读取防御塔类
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }
    /// <summary>
    /// 存储音效数据
    /// </summary>
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }
    /// <summary>
    /// 存储玩家数据
    /// </summary>
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }
    /// <summary>
    /// 播放音效的方法
    /// </summary>
    public void PlayerSound(string resName)
    {
        GameObject musicObj = new GameObject();
        AudioSource ad=musicObj.AddComponent<AudioSource>();
        ad.clip = Resources.Load<AudioClip>(resName);
        ad.volume=musicData.SoundValue;
        ad.mute=!musicData.isOpenSound;
        ad.Play();
        //延迟删除
        GameObject.Destroy(musicObj,1);
    }
}
