using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Image imageHp;
    public Text texHp;

    public Text texWave;
    public Text texMoney;

    public Button btnQuit;
    //可以在外部改变的血条长度，同时作为血量长度的计算值
    public float hpW = 580;

    //下方内容的父对象，控制显隐
    public Transform botTransform;

    //管理三个复合控件
    public List<TowerBtn> towerBtnList=new List<TowerBtn>();

    //当前选中的造塔点
    private TowerPoint nowSelTowerPonit;

    private bool checkInput;
    public override void Init()
    {
        btnQuit.onClick.AddListener(() =>
        {
            //隐藏游戏界面
            UIManager.Instance.HidePanel<GamePanel>();
            //返回到开始界面
            SceneManager.LoadScene("BeginScene");
            //其它
        });

        //一开始隐藏下方造塔相关的UI
        botTransform.gameObject.SetActive(false);
        //锁定鼠标以及光标隐藏
        Cursor.visible=false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    //给外部更新血量的方法
    public void UpdateTowerHp(int hp,int maxHp)
    {
        texHp.text = hp + "/" + maxHp;
        //更新血条的长度
        (imageHp.transform as RectTransform).sizeDelta=new Vector2((float)hp/maxHp*hpW,32);
    }

    //更新波数
    public void UpdateWave(int nowNum,int maxNum)
    {
        texWave.text=nowNum+"/"+maxNum;
    }
    //更新金币数量
    public void UpdateMoney(int money)
    {
        texMoney.text = money.ToString();
    }
    //造塔点的变化
    public void UpdateSelTower (TowerPoint point)
    {
        //根据造塔点的信息 决定 界面上显示的内容
        nowSelTowerPonit = point;

        //如果传入的是空，直接隐藏下方界面
        if (nowSelTowerPonit == null)
        {
            checkInput = false;
            //隐藏下方造塔按钮
            botTransform.gameObject.SetActive(false);
        }
        else
        {
            checkInput = true;
            //显示下方造塔按钮
            botTransform.gameObject.SetActive(true);

            //如果没有,选择造塔
            if (nowSelTowerPonit.nowTowerInfo == null)
            {
                for (int i = 0; i < towerBtnList.Count; i++)
                {
                    towerBtnList[i].gameObject.SetActive(true);
                    towerBtnList[i].InitInfo(nowSelTowerPonit.chooseIds[i], "快捷键" + (i + 1));
                }
            }
            //如果有,选择升级
            else
            {
                for (int i = 0; i < towerBtnList.Count; i++)
                {
                    towerBtnList[i].gameObject.SetActive(false);
                }
                towerBtnList[1].gameObject.SetActive(true);
                towerBtnList[1].InitInfo(nowSelTowerPonit.nowTowerInfo.next, "空格键");
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        //如果没有显示面板就没必要按键
        if (!checkInput)
            return;

        //按键造塔 或者升级
        if (nowSelTowerPonit.nowTowerInfo == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPonit.CreateTower(nowSelTowerPonit.chooseIds[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPonit.CreateTower(nowSelTowerPonit.chooseIds[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPonit.CreateTower(nowSelTowerPonit.chooseIds[2]);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nowSelTowerPonit.CreateTower(nowSelTowerPonit.nowTowerInfo.next);
            }
        }
    }
}
