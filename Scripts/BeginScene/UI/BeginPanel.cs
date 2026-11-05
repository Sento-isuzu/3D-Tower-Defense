using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnBegin;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnQuit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Init()
    {
        btnBegin.onClick.AddListener(() =>
        {
            //开始游戏
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            {
                //结束动画播放要做的逻辑
                //Debug.Log("选角面板");
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            });
            //隐藏开始面板
            UIManager.Instance.HidePanel<BeginPanel>();
        });
        btnSetting.onClick.AddListener(() =>
        {
            //打开设置面板
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        btnAbout.onClick.AddListener(() =>
        {
            //打开关于面板
        });
        btnQuit.onClick.AddListener(() =>
        {
            //退出游戏
            Application.Quit();
        });
    }
}
