/***
 *
 *    Project:0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:规定时间内的单次输入检测
 *
 *    Description:
 *    能够检测并响应一个单次的键盘按下
 *    出现一个节奏点就在对应时间之前注册一个贴着此脚本的对象
 *    接受到按下或者超时后向控制脚本反馈信息并在下一帧自动销毁
 *    响应前搜索所有Tag为HitCatch的物体，确保自己的ID最靠前时才予以响应
 *    也就是保证最先注册的最先被响应，一次只响应一个
 *
 *    Verson:1.0
 *
 *    Author:郭为
 *
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitCatch : MonoBehaviour
{
    private bool _BolDestroyFlag = false;//下一帧自毁标记
    private float FloNowTime ;//从注册经过的时间

    void Start()
    {
        FloNowTime = StageController.ScInstantce.FloHitAllowance50 - StageController.ScInstantce.FloCircleTime;//带倒计时的初始化

    }

    void Update() 
    {
		if(!_BolDestroyFlag)
        {
            if(FloNowTime<=StageController.ScInstantce.FloHitAllowance50*2)//未超时
            {
                string strTemp = StageController.ScInstantce.GetKey();//获取当前按键
                if(strTemp!="Null")//如果按键不为空
                {
                    if(IsTheEarliest())//检测本HitCatch的ID是否为所有HitCatch的最小者
                    {
                        StageController.ScInstantce.PlayerAction(strTemp);//玩家运动操作
                        if (FloNowTime < StageController.ScInstantce.FloHitAllowance50 - StageController.ScInstantce.FloHitAllowance100 ||
                            FloNowTime > StageController.ScInstantce.FloHitAllowance50 + StageController.ScInstantce.FloHitAllowance100)//检测响应的时间，给出不同的DealHit参数
                        {
                            StageController.ScInstantce.DealHit("50");
                        }
                        else if (FloNowTime <= StageController.ScInstantce.FloHitAllowance50 + StageController.ScInstantce.FloHitAllowance300&&
                            FloNowTime >= StageController.ScInstantce.FloHitAllowance50 - StageController.ScInstantce.FloHitAllowance300)
                        {
                            StageController.ScInstantce.DealHit("300");
                        }
                        else
                        {
                            StageController.ScInstantce.DealHit("100");
                        }
                        _BolDestroyFlag = true;//标记下一帧销毁
                    }
                }
            }
            else//超时
            {
                StageController.ScInstantce.PlayerAction("Null");
                StageController.ScInstantce.DealHit("Miss");
                _BolDestroyFlag = true;//标记下一帧销毁
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
        FloNowTime += Time.deltaTime;
	}

    public float GetLiveTime()
    {
        return FloNowTime;
    }

    public bool IsTheEarliest()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("HitCatch");
        for (int i = 0; i < gos.Length; i++)
        {
            if(gos[i].GetComponent<HitCatch>().GetLiveTime()>FloNowTime)
            {
                return false;
            }
        }
        return true;
    }

}
