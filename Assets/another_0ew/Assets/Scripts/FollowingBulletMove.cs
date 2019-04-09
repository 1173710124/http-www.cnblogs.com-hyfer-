/***
 *
 *    Project:0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:诱导弹脚本
 *
 *    Description:
 *    自己写的诱导弹脚本 效果很不好
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

public class FollowingBulletMove : MonoBehaviour 
{
    private GameObject GoAim;

    private float FloAccNow;//当前帧加速度

	// Use this for initialization
	void Start () 
    {
        GoAim = StageController.ScInstantce.GoBoss;
        FloAccNow = StageController.ScInstantce.FloPlayerBulletAcceleration;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), 0) * StageController.ScInstantce.FloPlayerBulletSpeed;//添加初始速度
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 delta =  GoAim.transform.position-this.transform.position ;
        this.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(delta.x, delta.y) * FloAccNow * 0.1F;
        FloAccNow += StageController.ScInstantce.FloPlayerBulletDeltaAcceleration;
	}
}
