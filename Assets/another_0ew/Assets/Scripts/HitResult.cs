/***
 *
 *    Project:0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:节奏打击反馈图脚本
 *
 *    Description:
 *    使反馈图淡出
 *    完成淡出后自毁
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

public class HitResult : MonoBehaviour 
{
    private Color c;
	// Use this for initialization
	void Start () 
    {
        c = this.gameObject.GetComponent<SpriteRenderer>().color;
	}
	
	// Update is called once per frame
	void Update () 
    {
        c.a -= StageController.ScInstantce.FloHitResultDeltaAlpha;
        this.gameObject.GetComponent<SpriteRenderer>().color = c; 
        if(this.gameObject.GetComponent<SpriteRenderer>().color.a<=0)
        {
            Destroy(this.gameObject);
        }
	}
}
