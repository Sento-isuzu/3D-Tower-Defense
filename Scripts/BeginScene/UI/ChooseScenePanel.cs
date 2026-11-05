using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    //按钮
    public Button btnStart;
    public Button btnReturn;
    public Button btnLeft;
    public Button btnRight;
    //文本以及图片
    public Image imageMap;
    public Text texIntroduce;
    //用于切换的索引
    private int nowIndex=0;
    //用于切换的场景索引
    private SceneInfo nowSceneInfo;

    public override void Init()
    {
        btnStart.onClick.AddListener(() =>
        {
            //隐藏自己
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            //切到游戏场景
            AsyncOperation ao=SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);
            //进行关卡初始化
            ao.completed += ((data) =>
            {
                //异步加载场景后在进行初始化
                GameLeaveMgr.Instance.InitInfo(nowSceneInfo);
            });
        });
        btnReturn.onClick.AddListener(() =>
        {
            //回到选人界面
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            //隐藏自己
            UIManager.Instance.HidePanel<ChooseScenePanel>();
        });
        btnLeft.onClick.AddListener(() =>
        {
            //左键向前切换
            nowIndex--;
            if(nowIndex < 0)
                //当小于0时，赋值为数组最后一位
                nowIndex=DataMgr.Instance.sceneInfoList.Count-1;
            ChangImg();
        });
        btnRight.onClick.AddListener(() =>
        {
            //右键向后切换
            nowIndex++;
            //同理，赋值为最前一位
            if (nowIndex >= DataMgr.Instance.sceneInfoList.Count)
                nowIndex = 0;
            ChangImg();
        });
        //一打开面板就更新信息
        ChangImg();
    }

    private void ChangImg()
    {
        //得到当前Scene的数据
        nowSceneInfo=DataMgr.Instance.sceneInfoList[nowIndex];
        //切换为当前的图片
        imageMap.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);
        //得到此时的场景信息
        texIntroduce.text = "名字：\n" + nowSceneInfo.name + "\n" + "描述：\n" + nowSceneInfo.tips;
    }
}
