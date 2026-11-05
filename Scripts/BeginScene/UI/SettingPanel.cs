using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Slider sliderMusic;
    public Slider sliderSound;
    public Toggle toggleMusic;
    public Toggle toggleSound;
    public override void Init()
    {
        //UI显示音乐音效大小的初始化
        MusicData data= DataMgr.Instance.musicData;
        //拖动条
        sliderMusic.value = data.MusicValue;
        sliderSound.value = data.SoundValue;
        //开关
        toggleMusic.isOn=data.isOpenMusic;
        toggleSound.isOn=data.isOpenSound;

        btnClose.onClick.AddListener(() =>
        {
            //防止浪费性能，所以直接在关闭设置面板存储改变过后的数据
            DataMgr.Instance.SaveMusicData();
            //隐藏面板
            UIManager.Instance.HidePanel<SettingPanel>();
        });
        sliderMusic.onValueChanged.AddListener((v) =>
        {
            //改变背景音乐大小
            BkMusic.Instance.ChangeValue(v);
            //记录背景音乐的大小
            DataMgr.Instance.musicData.MusicValue = v;
        });
        sliderSound.onValueChanged.AddListener((v) =>
        {
            //记录音效大小
            DataMgr.Instance.musicData.SoundValue = v;
        });
        toggleMusic.onValueChanged.AddListener((b) => 
        {
            //是否开启背景音乐
            BkMusic.Instance.SetIsOpen(b);
            //记录是否开启背景音乐
            DataMgr.Instance.musicData.isOpenMusic = b;

        });
        toggleSound.onValueChanged.AddListener((b) =>
        {
            //记录是否开启音效
            DataMgr.Instance.musicData.isOpenSound = b;
        });
    }
}
