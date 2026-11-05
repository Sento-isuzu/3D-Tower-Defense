using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 面板基类
/// </summary>
public abstract class BasePanel : MonoBehaviour 
{
    //用CanvasGroup统一管理面板的淡入淡出
    private CanvasGroup canvasGroup;
    //淡入、淡出的速度
    private float alphaSpeed = 30;
    //判断是否显隐的索引
    public bool isShowIndex=false;
    //淡入淡出后执行的行为，可以给外部调用
    private UnityAction action;
    
    //给外边一个在除了基类面板在start里执行的行为，还可以添加的行为
    public abstract void Init();

    //显示自己做的逻辑
    public virtual void ShowMe(UnityAction action)
    {
        canvasGroup.alpha = 0;
        isShowIndex=true;
        this.action = action;
    }
    //隐藏自己做的逻辑
    public virtual void HideMe(UnityAction action)
    {
        canvasGroup.alpha = 1;
        isShowIndex = false;
        this.action = action;
    }
    //一进来得到自己身上挂载的CanvasGroup，如果没有，直接自动添加
    protected virtual void Awake()
    {     
        if(canvasGroup == null)
            this.gameObject.AddComponent<CanvasGroup>();
        canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        Init();
    }
    protected virtual void Update()
    {
        //如果显示状态(透明度)不为1 就会不停对的加到1 加到1后为了不浪费性能，直接让alpha=1
        //不显示的时候逻辑同理
        //淡入
        if (isShowIndex && canvasGroup.alpha!=1)
        {
            canvasGroup.alpha += Time.deltaTime * alphaSpeed;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
                //淡入后执行的逻辑
                action?.Invoke();
            }
        }
        //淡出
        else if (!isShowIndex && canvasGroup.alpha!= 0)
        {
            canvasGroup.alpha -= Time.deltaTime * alphaSpeed;
            if(canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //淡出后执行的逻辑
                action?.Invoke();
            }
        }
    }

}
