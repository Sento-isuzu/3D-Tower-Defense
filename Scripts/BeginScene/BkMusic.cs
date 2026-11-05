using UnityEngine;

public class BkMusic : MonoBehaviour
{
    private static BkMusic instance;
    public static BkMusic Instance => instance;

    private AudioSource audioSource;
    private void Awake()
    {
        instance = this;
        //得到音效脚本
        audioSource = GetComponent<AudioSource>();
        //初始化
        MusicData data = DataMgr.Instance.musicData;
        SetIsOpen(data.isOpenMusic);
        ChangeValue(data.MusicValue);
    }
    //提供给外部是否开关音乐、音效
    public void SetIsOpen(bool b)
    {
        audioSource.mute = !b;
    }
    //提供给外部改变音乐音效的大小
    public void ChangeValue(float v)
    {
        audioSource.volume = v;
    }
}
