using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 组合控件
/// </summary>
public class TowerBtn : MonoBehaviour
{
    public Image imgPic;    
    public Text texTip;
    public Text texMoney;
    
    //给外部的初始化方法
    public void InitInfo(int id,string inputStr)
    {
        TowerInfo info = DataMgr.Instance.towerInfoList[id - 1];
        imgPic.sprite = Resources.Load<Sprite>(info.imgRes);       
        texTip.text = inputStr;

        //判断钱够不够
        if (info.money > GameLeaveMgr.Instance.player.money)
            texMoney.text = "金钱不足";
        else
            texMoney.text = "$" + info.money;
    }
}
