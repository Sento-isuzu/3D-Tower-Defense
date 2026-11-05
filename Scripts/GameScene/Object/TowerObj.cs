using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TowerObj : MonoBehaviour
{
    //炮台头部 用于旋转 指向目标
    public Transform head;
    //发射点位置
    public Transform gunPoint;
    //炮台旋转速度 可以写在配置表中
    private float roundSpeed = 20;
    //炮台关联的数据
    private TowerInfo info;

    //当前要攻击的目标
    private MonsterObj targetObj;
    //当前要攻击的目标们
    private List<MonsterObj> targetObjs;

    //用于计时的 判断攻击间隔时间的
    private float nowTime;

    private Vector3 monsterPos;

    //初始化炮台相关数据
    public void InitInfo(TowerInfo info)
    {
        this.info = info;
    }

    // Update is called once per frame
    void Update()
    {
        //单体攻击逻辑
        if (info.type == 1)
        {
            if (targetObj == null || targetObj.isDead||
                Vector3.Distance(this.transform.position,targetObj.transform.position)>info.atkRange)
            {
                targetObj=GameLeaveMgr.Instance.FindMonster(this.transform.position,info.atkRange);
            }

            //如果找不到任何怪物，炮台就不进行旋转
            if (targetObj == null)
                return;

            //得到怪物偏移y的位置，防止炮塔上下倾斜
            monsterPos=targetObj.transform.position;
            monsterPos.y=head.position.y;
            //让炮塔缓缓地转向怪物
            head.rotation = Quaternion.Slerp(head.rotation,
                Quaternion.LookRotation(monsterPos-head.position),Time.deltaTime*roundSpeed);

            if (Vector3.Angle(head.forward, monsterPos - head.position) <5&&
                Time.time-nowTime>=info.offsetTime)
            {
                //让目标受伤
                targetObj.Wound(info.atk);

                //播放音效
                DataMgr.Instance.PlayerSound("Music/Tower");
                //创建开火特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff),gunPoint.position,gunPoint.rotation);
                Destroy(effObj,0.2f);

                //记录开火时间,便于下一次间隔开火时间判断
                nowTime=Time.time;
            }
        } 
        //群体攻击逻辑
        else
        {
            targetObjs = GameLeaveMgr.Instance.FindMonsters(this.transform.position,info.atkRange);

            if (targetObjs.Count > 0 && Time.time - nowTime > info.offsetTime)
            {
                //让目标们受到伤害
                for (int i = 0; i < targetObjs.Count; i++)
                {
                    targetObjs[i].Wound(info.atk);
                }
                //创建开火特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);
                Destroy(effObj, 1f);
                //记录开火时间,便于下一次间隔开火时间判断
                nowTime = Time.time;
            }
        }
    }
}
