using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MonsterPoint : MonoBehaviour
{
    //怪物有多少波
    public int maxWave;
    //每波怪物有多少只
    public int monsterNumOneWave;
    //用于记录当前怪物还有多少只没有创建
    private int nowNum;

    //怪物ID 允许有多个 这样可以随机创建不同的怪物
    public List<int> monsterIds;
    //用于记录 当前波 要创建什么Id的怪物
    private int nowId;

    //单只怪物之间的间隔时间
    public float createOffsetTime;

    //波与波之间的间隔时间
    public float delayTime;

    //第一波怪物创建的间隔时间
    public float firstDelayTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //延迟函数 延时创建
        Invoke("CreateWave", firstDelayTime);

        //记录出怪点
        GameLeaveMgr.Instance.AddMonsterPoint(this);
        //更新最大波数
        GameLeaveMgr.Instance.UpdateMaxNum(maxWave);
    }

    //开始创建的第一波怪物
    private void CreateWave()
    {
        //随机怪物
        nowId = monsterIds[Random.Range(0,monsterIds.Count)];
        //当前怪物有多少只
        nowNum=monsterNumOneWave;
        //创建怪物
        CreateMonster();
        //减少波数
        --maxWave;

        //出了一波怪
        GameLeaveMgr.Instance.ChangeNowWaveNum(1);
    }

    //怪物创建
    private void CreateMonster()
    {
        //取出怪物
        MonsterInfo info=DataMgr.Instance.monsterInfoList[nowId-1];

        //创建怪物预设体
        GameObject obj = Instantiate(Resources.Load<GameObject>(info.res),this.transform.position,Quaternion.identity);
        //给怪物添加脚本
        MonsterObj monsterObj= obj.AddComponent<MonsterObj>();
        monsterObj.InitInfo(info);  //初始化

        //添加进数组里面
        GameLeaveMgr.Instance.AddMonster(monsterObj);

        //创建完一只怪物后 让当前记录的怪物数减一
        nowNum--;
        if (nowNum==0)
        {
            //怪物创建完了，创建下一波怪
            if (maxWave > 0)
                Invoke("CreateWave",delayTime);
        }
        else
        {
            //怪物创建 直到创建完为止
            Invoke("CreateMonster",createOffsetTime);
        }
    }

    //提供给外部判断是否出完怪
    public bool CheckOver()
    {
        return nowNum == 0 && maxWave == 0;
    }
}
