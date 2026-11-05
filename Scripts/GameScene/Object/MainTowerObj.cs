using UnityEngine;

public class MainTowerObj : MonoBehaviour
{
    private static MainTowerObj instance;
    public static MainTowerObj Instance => instance;

    //血量相关
    private int nowHp;
    private int maxHp;
    //是否死亡
    private bool isDead;

    private void Awake()
    {
        instance = this;
    }
    //更新血量
    public void UpdateHp(int hp,int maxHp)
    {
        this.nowHp = hp;
        this.maxHp = maxHp;

        //更新界面上的血量显示
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHp(hp, maxHp);
    }

    //自己受到的伤害
    public void Wound(int dmg)
    {
        //如果死亡就返回
        if (isDead)
            return;
        //受到伤害
        nowHp -= dmg;
        if(nowHp <= 0)
        {
            isDead = true;
            nowHp = 0;
            //游戏结束
            GameOverPanel panel= UIManager.Instance.ShowPanel<GameOverPanel>();
            //得到奖励的一半
            panel.InitInfo((int)(GameLeaveMgr.Instance.player.money*0.5f),false);
        }
        //更新血量
        UpdateHp(nowHp, maxHp); 
    }

    private void OnDestroy()
    {
        instance=null;
    }
}
