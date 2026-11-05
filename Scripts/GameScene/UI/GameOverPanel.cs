using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Text overTitle1;
    public Text overTitle2;
    public Text overMoney;
    public Button btnSure;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            //隐藏自己以及游戏UI
            UIManager.Instance.HidePanel<GameOverPanel>();
            UIManager.Instance.HidePanel<GamePanel>();

            //清空关卡数据
            GameLeaveMgr.Instance.ClearInfo();
            //回到开始界面
            SceneManager.LoadScene("BeginScene");
        });
        //光标显示
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void InitInfo(int money,bool isWin)
    {
        overTitle1.text = isWin ? "胜利":"失败";
        overTitle2.text = isWin ? "获得通关奖励" : "获得失败奖励";

        overMoney.text = "$" + money;

        //根据奖励 改变玩家数据
        DataMgr.Instance.playerData.haveMoney += money;
        DataMgr.Instance.SavePlayerData();
    }
}
