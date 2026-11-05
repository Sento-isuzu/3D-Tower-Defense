using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //摄像机看向的目标对象
    public Transform target;
    //摄像机的偏移位置
    public Vector3 offsetPos;
    //看向位置向上的偏移位置
    public float bodyHeight;

    //移动 旋转速度
    public float moveSpeed;
    public float rotateSpeed;

    private Vector3 targetPos;
    private Quaternion targetRotation;

    void Update()
    {
        //如果目标为空 直接返回
        if (target == null)
            return;

        //摄像机移动的逻辑
        //往后的偏移位置
        targetPos = target.position + target.forward * offsetPos.z;
        //向上的偏移位置
        targetPos += Vector3.up * offsetPos.y;
        //左右偏移的位置
        targetPos += target.right * offsetPos.x;
        //让摄像机不停地向目标点移动
        this.transform.position = Vector3.Lerp(this.transform.position,targetPos,Time.deltaTime*moveSpeed);

        //摄像机旋转的逻辑
        targetRotation = Quaternion.LookRotation(target.position+Vector3.up*bodyHeight- targetPos);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,targetRotation,Time.deltaTime*rotateSpeed);
    }

    //提供一个设置目标位置的方法给外部
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
