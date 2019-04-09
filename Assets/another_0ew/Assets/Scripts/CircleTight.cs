/***
 *
 *    Project:0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:判定圈缩圈
 *
 *    Description:
 *    控制判定圈缩圈，判定圈的大小和速度可调
 *    缩圈完成后脚本自毁
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

public class CircleTight : MonoBehaviour 
{
    private float FloCircleNowTime=0;//当前圈的大小
    private float FloSizeChange=0;//本帧圈的大小改变量

	void Update () 
    {
        FloSizeChange = (Time.deltaTime / StageController.ScInstantce.FloCircleTime) * (StageController.ScInstantce.FloCircleStartSize - StageController.ScInstantce.FloCircleEndSize);
        FloCircleNowTime += Time.deltaTime;
        this.transform.localScale -= new Vector3(FloSizeChange, FloSizeChange, 0);
        if (FloCircleNowTime >= StageController.ScInstantce.FloCircleTime)
        {
            Destroy(this);
        }
	}
}
