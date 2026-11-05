using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterObj : MonoBehaviour
{
    //动画相关
    private Animator animator;
    //位移相关 寻路组件
    private NavMeshAgent agent;
    //一些不变的基础数据
    private MonsterInfo monsterInfo;

    //当前血量
    private int hp;
    //是否死亡
    public bool isDead=false;

    //上一次攻击的时间
    private float frontTime = 0;

    private void Awake()
    {
        //得到寻路组件 以及 动画组件
        agent = this.GetComponent<NavMeshAgent>();
        animator=this.GetComponent<Animator>();
    }

    //初始化
    public void InitInfo(MonsterInfo info)
    {
        monsterInfo = info;
        //状态机加载
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        //要变的当前血量
        hp=info.hp;
        //速度 旋转速度 加速度初始化
        agent.speed =agent.acceleration =info.moveSpeed;
        agent.angularSpeed=info.roundSpeed;
    }

    public void Wound(int dmg)
    {
        if (isDead)
            return;
        //减少血量
        hp-=dmg;
        //播放受伤动画
        animator.SetTrigger("Wound");

        if (hp<=0)
        {
            //死亡
            Dead();
        }
        else
        {
            //播放受伤音效
            DataMgr.Instance.PlayerSound("Music/Wound");
        }
    }

    public void Dead()
    {
        isDead=true;
        //停止移动
        agent.isStopped = true;
        //播放死亡动画
        animator.SetBool("Dead",true);
        //播放音效
        DataMgr.Instance.PlayerSound("Music/dead");
        //加钱 后面可以写一个关卡管理类统一管理
        GameLeaveMgr.Instance.player.AddMoney(150);

    }

    //死亡动画播放后执行的函数
    public void DeadEvent()
    {
        //死亡动画播放后移除对象
        GameLeaveMgr.Instance.RemoveMonster(this);
        //移除对象
        Destroy(this.gameObject);

        //怪物死亡时 检测游戏是否胜利
        GameLeaveMgr.Instance.CheckOver();
        if (GameLeaveMgr.Instance.CheckOver())
        {
            //游戏胜利面板
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo(GameLeaveMgr.Instance.player.money,true);
        }
    }

    //出生动画播放后执行的逻辑
    public void BornOver()
    {
        //执行移动动画
        animator.SetBool("Run",true);
        //自动寻路的目标位置
        agent.SetDestination(MainTowerObj.Instance.transform.position);
    }

    //伤害检测
    public void AtkEvent()
    {
        //范围检测
        Collider[] colliders= Physics.OverlapSphere(this.transform.position+this.transform.forward*1.5f,1,1<<LayerMask.NameToLayer("MainTower"));

        DataMgr.Instance.PlayerSound("Music/Eat");

        //遍历数组
        for (int i = 0; i < colliders.Length; i++) 
        {
            //这里也可以保险起见,判断得到的对象是否是唯一的主塔
            if(colliders[i].gameObject == MainTowerObj.Instance.gameObject)
            {
                //塔的受伤的扣血逻辑
                //colliders[i].transform.GetComponent<MainTowerObj>().Wound(monsterInfo.atk);
                //因为只有一个塔，甚至可以不遍历这个数组，直接调用下面那就话就可以。
                MainTowerObj.Instance.Wound(monsterInfo.atk);
            }
        }
    }

    void Update()
    {
        //检测什么时候停下来攻击
        if (isDead)
            return;

        //根据速度 来决定动画播放什么
        animator.SetBool("Run",agent.velocity!=Vector3.zero);
        //检测停下来的时候就进行攻击
        if (Vector3.Distance(this.transform.position, MainTowerObj.Instance.transform.position) <= 5&&
            Time.time-frontTime>=monsterInfo.atkOffst)
        {
            //记录上一次的时间
            frontTime = Time.time;
            //播放攻击动画
            animator.SetTrigger("Atk");
        }
    }

}
