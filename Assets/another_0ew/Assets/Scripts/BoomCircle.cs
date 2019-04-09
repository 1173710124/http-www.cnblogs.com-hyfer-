/***
 *
 *    Project: 0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title: 炸弹环脚本
 *
 *    Description:
 *    管理炸弹环的扩张，淡出
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

public class BoomCircle : MonoBehaviour 
{
    private Vector3 Vec3Temp;
    private float FloKilltime;
	// Use this for initialization
	void Start () 
    {
        Vec3Temp = this.gameObject.transform.localScale;
        FloKilltime = Time.time + StageController.ScInstantce.FloPlayerBoomCircleTime;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vec3Temp.x += StageController.ScInstantce.FloPlayerBoomCircleSpeed;
        Vec3Temp.y += StageController.ScInstantce.FloPlayerBoomCircleSpeed;
        this.gameObject.transform.localScale = Vec3Temp;
        if(Time.time>FloKilltime)
        {
            this.gameObject.AddComponent<HitResult>();
        }
	}
}
