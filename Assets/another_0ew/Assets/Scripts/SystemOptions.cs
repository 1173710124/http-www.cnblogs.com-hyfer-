/***
 *
 *    Project:0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:系统设置
 *
 *    Description:
 *    记录系统设置，进行跨场景传参数
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

public class SystemOptions : MonoBehaviour
{
    public float FloMusicVolume=1F;
    public bool BolOperatingLicense = true;

    public static SystemOptions SoInstantce;//单例

    // Use this for initialization
    void Start ()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        SoInstantce = this;
	}
	
}
