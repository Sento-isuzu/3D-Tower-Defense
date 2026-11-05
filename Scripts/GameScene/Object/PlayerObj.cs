using Unity.VisualScripting;
using UnityEngine;

public class PlayerObj : MonoBehaviour
{
    //攻击力 金币数
    private int atk;
    public int money;
    //旋转的速度
    private float roundSpeed = 50;
    //下蹲，起立速度
    private float upAndDownSpeed = 10;

    //动画相关
    private Animator animator;

    //蹲下起立的渐变索引值
    private float indexDown;
    private bool indexUp=false;

    //开火点
    public Transform GunPonit;

    //攻击力，金币初始化
    public void InitPlayerInfo(int atk, int money)
    {
        this.atk = atk;
        this.money = money;
        //更新界面上钱的数量
        UpdateMoney();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //得到状态机脚本
        animator=this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //左右 前后移动
        animator.SetFloat("HSpeed",Input.GetAxis("Horizontal"));
        animator.SetFloat("VSpeed",Input.GetAxis("Vertical"));
        //旋转
        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * roundSpeed * Time.deltaTime);

        //冲刺
        if (Input.GetKey(KeyCode.W)&&Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("IsFastRun",true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("IsFastRun", false);
        }

        //蹲下的渐变
        if (Input.GetKey(KeyCode.LeftControl))
        {
            indexDown += Time.deltaTime*upAndDownSpeed;
            animator.SetLayerWeight(1,Mathf.Clamp(indexDown,0,1));
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            indexDown= animator.GetLayerWeight(1);
            indexUp = true;
        }
        //站起的渐变
        else if(indexUp)
        {
            indexDown -= Time.deltaTime*upAndDownSpeed;
            animator.SetLayerWeight(1, indexDown);
            if (indexDown <= 0)
            {
                indexDown = 0;
                indexUp=false;
            }           
        }

        //向前滚动
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Roll");
        }
        //攻击
        else if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Fire1");
        }
    }

    //攻击碰撞判定
    public void KnifeEvent()
    {
        //进行范围检测，用一个Collider数组承接得到的碰撞器
        Collider[] colliders= Physics.OverlapSphere(this.transform.position + transform.forward * 1.5f,
            2, 1 << LayerMask.NameToLayer("Monster"));

        //播放音效
        DataMgr.Instance.PlayerSound("Music/Kin");

        //遍历数组
        for (int i = 0; i < colliders.Length; i++) 
        {
            //对攻击到的怪物进行检测
            MonsterObj monsterObj= colliders[i].gameObject.GetComponent<MonsterObj>();
            if(monsterObj != null&&!monsterObj.isDead)
            {
                monsterObj.Wound(atk);
                //只攻击一个怪物
                break;
            }
                
        }
    }

    //开枪射击判定
    public void ShootEvent()
    {
        //射线检测
        RaycastHit[] raycastHits=Physics.RaycastAll(new Ray(GunPonit.position,GunPonit.forward),
            1000,1<<LayerMask.NameToLayer("Monster"));

        //播放音效
        DataMgr.Instance.PlayerSound("Music/Gun");

        //遍历数组
        for (int i = 0;i < raycastHits.Length; i++)
        {
            //对攻击到的怪物进行检测
            MonsterObj monsterObj = raycastHits[i].collider.gameObject.GetComponent<MonsterObj>();
            if (monsterObj != null&& !monsterObj.isDead)
            {
                //创建特效
                GameObject effObj = GameObject.Instantiate(Resources.Load<GameObject>(DataMgr.Instance.NowSelRole.hitEff));
                effObj.transform.position = raycastHits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(raycastHits[i].normal);

                Destroy(effObj,1);

                monsterObj.Wound(atk);
                //只攻击一个怪物
                break;
            }              
        }
    }

    //钱变化的逻辑
    public void UpdateMoney()
    {
        //间接更新 钱的数量
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }

    //提供给外部加钱的方法
    public void AddMoney(int money)
    {
        //加钱
        this.money+= money;
        //更新钱的数量
        UpdateMoney();
    }
}
