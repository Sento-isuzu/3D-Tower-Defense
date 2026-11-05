using UnityEngine;

public class TipsControll
{
    private static TipsControll instance=new TipsControll();
    public static TipsControll Instance=>instance;

    //场景上的canvas的位置
    private Transform canvas;
    private SmallTips smallTips;
    private TipsControll() 
    {
        //由于面板先比提示先创建。所以直接找到面板上的canvas即可
        GameObject obj = GameObject.Find("Canvas(Clone)");
        if (obj != null)
        {
            canvas = obj.transform;
        }
        else
        {
            Debug.Log("没有找到Canavs");
        }
    }
    public void CreateAndDestroy(string name)
    {

        //动态创建小提示
        GameObject tips = GameObject.Instantiate(Resources.Load<GameObject>("UI/SmallTips"));
        //设置为父对象
        tips.transform.SetParent(canvas,false);
        smallTips = tips.GetComponent<SmallTips>();
        //改名
        smallTips.ChangeName(name);

    }
}
