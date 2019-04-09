/***
 *
 *    Project:0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:诱导弹脚本
 *
 *    Description:
 *    从LuaSTG仿写的诱导弹脚本
 *
 *    Verson:
 *
 *    Author:
 *
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallowingBulletMoveB : MonoBehaviour
{
    private GameObject _GoBoss;
    private float trail = 2;
    private float v = 3;

    // Use this for initialization

    void OnTriggerEnter2D(Collider2D col)  //撞boss自毁
    {
        print("a");
        if (col.gameObject.tag == "Boss")
        {
            StageController.ScInstantce.AddScore("Bullet");
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        _GoBoss = StageController.ScInstantce.GoBoss;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad)) * v;
        trail = StageController.ScInstantce.FloPlayerBulletTrail;
        v = StageController.ScInstantce.FloPlayerBulletSpeed;
    }

    void Update()
    {
        float SelfAngle = this.gameObject.transform.rotation.eulerAngles.z;
        Vector2 SelfPostion = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);//自身绝对坐标
        Vector2 BossPostion = new Vector2(_GoBoss.transform.position.x, _GoBoss.transform.position.y);//Boss绝对坐标
        Vector2 SelfBoss = BossPostion - SelfPostion;//boss相对自己的坐标
        Vector2 SelfRot = new Vector2(Mathf.Cos(SelfAngle * Mathf.Deg2Rad), Mathf.Sin(SelfAngle * Mathf.Deg2Rad));//自己的速度矢量
        float DelatAngle = VectorAngle(SelfBoss, SelfRot);//自身速度和自身与boss方向向量的差
        //print(SelfAngle + " " + Mathf.Cos(SelfAngle) + " " + Mathf.Sin(SelfAngle) + " " + SelfRot);
        //print(SelfBoss + " " + SelfRot + " " + DelatAngle);
        float ChangeAngle = trail / SelfBoss.magnitude + 0.01F;//角度修正量，离目标越近修正量越小
        Quaternion tempQuaternion = this.transform.rotation;//导入角度用临时四元数
        //print(VectorAngle(new Vector2(1, 0), SelfBoss) + " " + SelfBoss + " " + BossPostion + " " + SelfPostion);
        if (Mathf.Abs(DelatAngle) <= ChangeAngle)//如果本次修正量将会超过偏差量，及会矫枉过正
        {
            //print("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            //print(VectorAngle(new Vector2(1, 0), SelfBoss));
            tempQuaternion.eulerAngles = new Vector3(0, 0, VectorAngle(SelfBoss, new Vector2(1, 0)));
            this.gameObject.transform.rotation = tempQuaternion;                              //直接对准boss
        }
        else
        {
            float NewAngle = SelfAngle + Mathf.Sign(DelatAngle) * ChangeAngle;//新角度等于带符号的修正量
            tempQuaternion.eulerAngles = new Vector3(0, 0, NewAngle);
            this.gameObject.transform.rotation = tempQuaternion;                              //导入新角度
            //print(NewAngle);
        }
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad)) * v;//修改速度
    }

    float VectorAngle(Vector2 from, Vector2 to)//计算从向量A到向量B的旋转角
    {
        float angle;
        Vector3 cross = Vector3.Cross(from, to);
        angle = Vector2.Angle(from, to);
        return cross.z > 0 ? -angle : angle;
    }



    //void Update () 
    //{
    //    int anglet;//以自己为中心的目标角度
    //    int anglev;//自己的速度角度
    //    anglev = (int)this.transform.rotation.eulerAngles.z;
    //    anglet =(int) GetAngle(this.transform.position, _GoBoss.transform.position);
    //    //print(anglev+","+anglet);
    //}

    //void Update()
    //{

    //    a = (int)(GetAngle(this.transform.position, _GoBoss.transform.position) - this.transform.rotation.eulerAngles.z) % 360;
    //    if (a > 180)
    //    {
    //        a = a - 360;
    //    }
    //    da = trail / (GetDist(this.transform.position, _GoBoss.transform.position) + 1F);
    //    Quaternion temp = new Quaternion();
    //    if (da >= Mathf.Abs(a))
    //    {
    //        temp.eulerAngles = new Vector3(0, 0, GetAngle(this.transform.position, _GoBoss.transform.position));
    //        this.transform.rotation = temp;
    //    }
    //    else
    //    {
    //        temp.eulerAngles = new Vector3(0, 0, this.transform.rotation.eulerAngles.z + Mathf.Sign(a) * da);
    //    }
    //    this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad)) * v;
    //}

    //private float GetAngle(Vector3 vec1, Vector3 vec2)
    //{
    //    //float angle=Vector2.Angle(new Vector2(vec1.x, vec1.y), new Vector2(vec2.x, vec2.y));
    //    float angle = Vector2.Angle(new Vector2(1, 0), new Vector2(1, 1));
    //    print(angle);
    //    return angle;
    //}

    //private float GetAngle(Vector3 vec1, Vector3 vec2)
    //{
    //    float angle;
    //    float x, y;
    //    x = vec2.x - vec1.x;
    //    y = vec2.y - vec1.y;
    //    a = x ^ 2 + y ^ 2;
    //    if (x != 0)
    //    {
    //        angle = Mathf.Atan(y / x) * Mathf.Rad2Deg;
    //        print(x + "," + y + "," + angle);
    //        if (x < 0)
    //        {
    //            angle = -angle;
    //        }
    //    }
    //    else if (y >= 0)
    //    {
    //        angle = 90;
    //    }
    //    else
    //    {
    //        angle = -90;
    //    }
    //    return angle;
    //}

    //private float GetDist(Vector3 vec1, Vector3 vec2)
    //{
    //    Vector3 temp = vec2 - vec1;
    //    float dist = temp.sqrMagnitude;
    //    return dist;
    //}

}
