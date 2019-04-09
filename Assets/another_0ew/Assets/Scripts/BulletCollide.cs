/***
 *
 *    Project: 0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:子弹碰撞脚本
 *
 *    Description:
 *    处理子弹碰撞玩家的事件
 *    处理子弹碰撞炸弹的事件
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

public class BulletCollide : MonoBehaviour 
{

    private bool _BolPlayerKillableFlag=true;
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void OnTriggerEnter2D(Collider2D col)  //撞boss自毁
    {
        if (col.gameObject.tag == "Boom")//如果碰触炸弹
        {
            _BolPlayerKillableFlag = false;//不再允许杀人
            this.gameObject.AddComponent<HitResult>();            //挂载淡出脚本
        }
        if(_BolPlayerKillableFlag)//如果允许杀人
        {
            if (col.gameObject.tag == "Player")//如果碰触玩家
            {
                _BolPlayerKillableFlag = false;
                StageController.ScInstantce.CollideBullet();//呼叫中弹处理方法
                this.gameObject.AddComponent<HitResult>();
            }
        }
    }

}
