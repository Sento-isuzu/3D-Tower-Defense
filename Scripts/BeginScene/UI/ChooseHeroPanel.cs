using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    //左右选择按钮
    public Button btnLeft;
    public Button btnRight;
    //返回，开始按钮
    public Button btnStart;
    public Button btnReturn;
    //解锁按钮
    public Button btnUnlock;
    public Text unLockMoney;

    //显示有多少钱
    public Text textMoney;
    //角色信息
    public Text texHeroTips;
    //角色位置
    private Transform heroPos;

    //角色索引，用于每一次切换的destroy
    private GameObject NowHero;
    //角色数据索引
    private RoleInfo nowRoleData;
    //List<RoleInfo>索引
    private int nowIndex=0;


    public override void Init()
    {

        //初始化位置
        heroPos = GameObject.Find("HeroPos").transform;

        //玩家左上角的钱
        textMoney.text=DataMgr.Instance.playerData.haveMoney.ToString();

        //按左右键切换角色
        btnLeft.onClick.AddListener(() =>
        {
            //按左键,索引减一
            nowIndex--;
            if (nowIndex<0)
                //重新赋值为list数组最后一位
                nowIndex = DataMgr.Instance.roleInfoList.Count - 1;
            ChangHero();
        });
        btnRight.onClick.AddListener(() =>
        {
            //右键 跟左键同理
            nowIndex++;
            if(nowIndex>= DataMgr.Instance.roleInfoList.Count)
                nowIndex = 0;
            ChangHero();
        });

        //开始游戏的按钮
        btnStart.onClick.AddListener(() =>
        {
            //保存选择的角色
            DataMgr.Instance.NowSelRole = nowRoleData;

            //隐藏自己
            UIManager.Instance.HidePanel<ChooseHeroPanel>();

            //切换到选场景界面
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });

        //返回开始界面
        btnReturn.onClick.AddListener(() =>
        {         
            //回到开始界面,摄像机播放完动画后做的逻辑
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>
            {
                //显示开始界面
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
            //隐藏自己
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
        });

        //解锁角色按钮
        btnUnlock.onClick.AddListener(() =>
        {
            //玩家拥有的钱是否能够解锁角色
            if (DataMgr.Instance.playerData.haveMoney>=nowRoleData.lockMoney)
            {
                //拥有的钱减去解锁玩家所需要的钱
                DataMgr.Instance.playerData.haveMoney -= nowRoleData.lockMoney;
                //显示在左上角
                textMoney.text = DataMgr.Instance.playerData.haveMoney.ToString();
                //记录购买了后的角色id
                DataMgr.Instance.playerData.buyHero.Add(nowRoleData.id);
                //保存数据
                DataMgr.Instance.SavePlayerData();

                //更新解锁按钮
                UpdatalockBtn();
                //可以加一个提示面板显示购买成功
                TipsControll.Instance.CreateAndDestroy("购买成功");
            }
            else
            {
                //提示面板 显示金额不足
                TipsControll.Instance.CreateAndDestroy("金额不足");
            }
        });
        //一开始显示角色
        ChangHero();
    }
    private void ChangHero()
    {
        if (NowHero != null)
        {
            Destroy(NowHero);
            NowHero = null;
        }
        //得到切换后的角色数据
        nowRoleData=DataMgr.Instance.roleInfoList[nowIndex];
        //角色介绍
        texHeroTips.text = nowRoleData.tips.ToString();
        //显示解锁角色所需要的金额
        unLockMoney.text=nowRoleData.lockMoney.ToString();
        //创建切换后的人物预设体,同时设置位置以及角度
        NowHero = Instantiate(Resources.Load<GameObject>(nowRoleData.res),heroPos.position,heroPos.rotation);
        //在选择角色的场景上不需要playerObj脚本
        Destroy(NowHero.GetComponent<PlayerObj>());

        //调用解锁角色后可以显示开始按钮，否则不显示开始按钮
        UpdatalockBtn();
    }

    private void UpdatalockBtn()
    {
        //判断我又没有这个角色，并且这个角色为需要的金额为0就可以直接开始
        if (nowRoleData.lockMoney > 0 && !DataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id))
        {
            btnUnlock.gameObject.SetActive(true);
            //更新按钮上面的钱
            unLockMoney.text="$"+nowRoleData.lockMoney;
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnlock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }
    //重写逻辑
    public override void HideMe(UnityAction action)
    {
        base.HideMe(action);
        //切换面板时，删除角色
        if (NowHero!=null)
        {
            Destroy(NowHero);
            NowHero = null;
        }
    }
}
