/***
 *
 *    Project: 0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:擦弹判定环脚本
 *
 *    Description:
 *    设置擦弹半径
 *    检测碰撞并呼叫擦弹方法
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

public class GrazeRadius : MonoBehaviour 
{
    private bool _BolGrazableFlag = true;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_BolGrazableFlag)
        {
            if(col.gameObject.tag=="Player")
            {
                StageController.ScInstantce.GrazeBullet();
                _BolGrazableFlag = false;
            }
        }
    }
}
