using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 面板管理器
/// </summary>
public class UIManager 
{
    //单例
    private static UIManager instance=new UIManager();
    public static UIManager Instance => instance;

    //用于存储每一个面板
    private Dictionary<string,BasePanel> panelDic = new Dictionary<string,BasePanel>();
    //方便得到场景中的Canvas对象，用于设置父对象
    private Transform canvasTrans;

    private UIManager() 
    {
        //得到场景中的Canvas对象
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        //过场景时不移除该对象 保证整个游戏过程中 只有一个canvas对象
        GameObject.DontDestroyOnLoad(canvas);
    }

    //显示面板
    public T ShowPanel<T>() where T : BasePanel
    {
        //保证泛型T的类型和面板预设体的名字一样
        string panelName=typeof(T).Name;

        //判断字典中是否已经显示了这个面板
        if(panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;

        //动态创建面板
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>($"UI/{panelName}"));
        //把面板 放到场景中的Canvas下面
        panelObj.transform.SetParent(canvasTrans,false);

        //显示面板，并且把面板保存起来
        T panel=panelObj.GetComponent<T>();
        //把面板存储到字典里面
        panelDic.Add(panelName, panel);
        //调用方法
        panel.ShowMe(() =>
        {
            //Debug.Log("TestShowMe");
        });
        return panel;
    }

    //隐藏面板
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        //跟显示逻辑相同
        string panelName = typeof(T).Name;
        //判断有没有这个面板
        if (panelDic.ContainsKey(panelName))
        {
            //是否要使用淡出效果，默认为使用(Ture)
            if (isFade)
            {
                //执行淡出
                panelDic[panelName].HideMe(() =>
                {
                    //Debug.Log("TestHideMe");
                    //删除在场景中的面板
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //同时把字典里面的存储的面板也删除了
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //跟上边同理
                GameObject.Destroy(panelDic[panelName].gameObject);
                panelDic.Remove(panelName);
            }
        }
        return;
    }

    //得到面板
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName=typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
             return panelDic[panelName] as T;
        //如字典里没有此面板，直接返回空(null)
        return null;
    }
}
