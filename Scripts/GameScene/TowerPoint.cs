using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    //关联塔的对象
    private GameObject towerObj = null;
    //塔的数据
    public TowerInfo nowTowerInfo=null;
    //可以建造塔的id
    public List<int> chooseIds;

    public void CreateTower(int id)
    {
        TowerInfo info = DataMgr.Instance.towerInfoList[id - 1];
        //如果钱不够就不造塔
        if (info.money > GameLeaveMgr.Instance.player.money)
            return;

        //扣钱
        GameLeaveMgr.Instance.player.AddMoney(-info.money);
        //创建塔
        //先判断之前是否有塔 如果有 就删除
        if (towerObj!=null)
        {
            Destroy(towerObj);
            towerObj = null;
        }
        //建造，并设置位置角度
        towerObj = Instantiate(Resources.Load<GameObject>(info.res), 
            this.transform.position, Quaternion.identity);
        //初始化塔
        towerObj.GetComponent<TowerObj>().InitInfo(info);

        //记录当前塔的数据
        nowTowerInfo = info;

        if (nowTowerInfo.next != 0)
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else 
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //如果现在有塔了 就没有必要显示升级界面或者造塔界面了
        if (nowTowerInfo != null && nowTowerInfo.next == 0)
            return;
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }

    private void OnTriggerExit(Collider other)
    {
        //不显示，直接传空
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }
}
