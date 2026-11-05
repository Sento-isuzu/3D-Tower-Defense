using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameLeaveMgr
{
    private static GameLeaveMgr instance=new GameLeaveMgr();
    public static GameLeaveMgr Instance => instance;
   
    public PlayerObj player;

    private List<MonsterPoint> points = new List<MonsterPoint>();

    //记录当前还有多少波怪物
    private int nowWaveNum = 0;
    //记录 一共有多少波怪物
    private int maxWaveNum = 0;

    ////当前场景上的怪物
    //private int nowMonsterNum = 0;

    private List<MonsterObj> monsterList=new List<MonsterObj>();
    private GameLeaveMgr() { }

    public void InitInfo(SceneInfo sceneInfo)
    {
        //显示UI界面
        UIManager.Instance.ShowPanel<GamePanel>();

        //玩家创建
        //获取之前的存储的当前玩家
        RoleInfo roleInfo = DataMgr.Instance.NowSelRole;
        //获取玩家出生位置
        Transform heroPos = GameObject.Find("HeroPos").transform;
        //创建玩家
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res),
            heroPos.position, heroPos.rotation);
        //对玩家进行初始化
        player=heroObj.GetComponent<PlayerObj>();
        //初始化玩家的数据
        player.InitPlayerInfo(roleInfo.atk,int.Parse(sceneInfo.money));

        //让摄像机动态看向玩家
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);

        //初始化 中央 保护区域的血量
        MainTowerObj.Instance.UpdateHp(sceneInfo.towerHp, sceneInfo.towerHp);
    }

    //记录出怪点的方法
    public void AddMonsterPoint(MonsterPoint point)
    {
        points.Add(point);
    }

    //更新 一共有多少波怪
    public void UpdateMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum=maxWaveNum;
        //更新UI上的波数
        UIManager.Instance.GetPanel<GamePanel>().UpdateWave(nowWaveNum,maxWaveNum);
    }

    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum-=num;
        //更新界面
        UIManager.Instance.GetPanel<GamePanel>().UpdateWave(nowWaveNum, maxWaveNum);
    }

    //判断是否结束
    public bool CheckOver()
    {
        for (int i = 0; i < points.Count; i++)
        {
            //只要有一个出怪点 还没有出完怪 就返回false
            if (!points[i].CheckOver())
                return false;
        }
        //所有怪物都死完了
        if (monsterList.Count > 0)
            return false;

        return true;
    }

    //添加怪物
    public void AddMonster(MonsterObj obj)
    {
        monsterList.Add(obj);
    }

    //移除怪物
    public void RemoveMonster(MonsterObj obj)
    {
        monsterList.Remove(obj);
    }

    //寻找满足条件的怪物
    public MonsterObj FindMonster(Vector3 pos,int range)
    {
        //遍历怪物，同时判断这只怪物是否在范围内以及没有死，是就可以返回出去
        for (int i = 0; i < monsterList.Count; i++) 
        {
            if (!monsterList[i].isDead&& Vector3.Distance(pos, monsterList[i].transform.position)<=range)
            {
                return monsterList[i];
            }
        }
        return null;
    }
    //寻找满足条件的所有怪物
    public List<MonsterObj> FindMonsters(Vector3 pos,int range)
    {
        //寻找满足范围内的怪物，并添加到列表当中
        List<MonsterObj> list = new List<MonsterObj>();
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                list.Add(monsterList[i]);
            }
        }
        return list;
    }

    ////改变当前场景上怪物的数量
    //public void ChangeMonsterNum(int num)
    //{
    //    nowMonsterNum += num;
    //}

    //置空
    public void ClearInfo()
    {
        points.Clear();
        monsterList.Clear();
        nowWaveNum=maxWaveNum=0;
        player=null;
    }
}
