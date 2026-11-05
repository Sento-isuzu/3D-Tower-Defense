using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    //定义一个委托传给外部使用，当动画结束后
    private UnityAction cameraAction;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    //摄像机左转动画
    public void TurnLeft(UnityAction unityAction)
    {
        animator.SetTrigger("LeftTg");
        cameraAction=unityAction;
    }
    //摄像机右转动画
    public void TurnRight(UnityAction unityAction)
    {
        animator.SetTrigger("RightTg");
        cameraAction = unityAction;
    }
    //结束播放执行
    public void PlayerOver()
    {
        cameraAction?.Invoke();
        //每次执行完播放后的逻辑，委托设置为空
        cameraAction = null;
    }
}
