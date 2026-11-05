using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public Text TextContent;
    public Button btnSure;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            //隐藏自己
            UIManager.Instance.ShowPanel<TipPanel>();
        });
    }
    /// <summary>
    /// 改变提示面板的内容
    /// </summary>
    /// <param name="name"></param>
    public void ChangeText(string name)
    {
        TextContent.text = name;
    }
}
