/***
 *
 *    Project:0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:子弹位移
 *
 *    Description:
 *    子弹向自己角度的右方位移，速度可以设置
 *    处理子弹出屏自毁
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

public class BulletMove : MonoBehaviour 
{
    public float FloBulletSpeed=1.0F;
	// Use this for initialization
	void Start () 
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), 0) * FloBulletSpeed;//添加初始速度
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (IsOutBoundary())
        {
            Destroy(this.gameObject);
        }
	}

    private bool IsOutBoundary()
    {
        if (this.gameObject.transform.position.y > 5 + StageController.ScInstantce.FloMapTopBulletBoundaryWidth || this.gameObject.transform.position.y < -5 - StageController.ScInstantce.FloMapBottomBulletBoundaryWidth || this.gameObject.transform.position.x > 7.8125 + StageController.ScInstantce.FloMapRightBulletBoundaryWidth || this.gameObject.transform.position.x < -7.8125 - StageController.ScInstantce.FloMapLeftBulletBoundaryWidth)
        {
            return true;
        }
        return false;
    }
}
