using System;
using UnityEngine;
using UnityEngine.UI;

public class SmallTips : MonoBehaviour
{
    //统一管理淡入淡出
    private CanvasGroup canvasGroup;
    //淡入、淡出的速度
    public float alphaSpeed=5;
    //判断是否显影的索引
    public int isShowIndex=10;
    //持续的时间索引
    private float index;
    //持续的时间
    public float indexTime = 3;
    //显示的内容
    public Text cotent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //一进来就或得脚本
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }
    // Update is called once per frame
    void Update()
    {
        StayTime();
        if (isShowIndex==2)
        {
            Destroy(this.gameObject);
        }
    }
    private void StayTime()
    {
        switch (isShowIndex)
        {
            case 1:
                canvasGroup.alpha -= Time.deltaTime * alphaSpeed;
                if (canvasGroup.alpha <= 0)
                {
                    canvasGroup.alpha = 0;
                    isShowIndex = 2;
                }
                break;
            case 0:
                index += Time.deltaTime;
                if (index >= indexTime)
                {
                    index = 0;
                    isShowIndex = 1;
                }
                break;
            case -1:
                canvasGroup.alpha += Time.deltaTime * alphaSpeed;
                if (canvasGroup.alpha >= 1)
                {
                    canvasGroup.alpha = 1;
                    isShowIndex = 0;
                }
                break;
        }
    }
    //提供给外部改名的方法
    public void ChangeName(string name)
    {
        //改变显示索引值
        isShowIndex = -1;
        //改名
        cotent.text=name;
    }
}
