/***
 *
 *    Project:0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:设置菜单控制
 *
 *    Description:
 *    控制设置菜单
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

public class OptionMenuController : MonoBehaviour
{

    public Text[] TxtTextArray;//选项的文字数组
    public int IntOptionsNumber = 3;//总选项数，至少为1
    public int IntDefaultOption = 0;//默认的选项

    public Toggle TogOther;//要控制的Toggle
    public Slider SliSystemVolume;//系统音效的滑条

    public Text TxtOpinionOn;//选项选中的预制效果，目前支持颜色
    public Text TxtOpinionOff;//选项未选中的预制效果，目前支持颜色

    public GameObject GoParentMenuCanvas;//上一级菜单
    public GameObject GoMusk;//黑屏遮罩
    public Image ImaMusk;//场景遮罩图片

    private int IntNowOption = 0;//当前选项编号，首项编号为0
                                 // Use this for initialization
    void Start()
    {
        IntNowOption = 0;
        for (int i = 0; i < TxtTextArray.Length; i++)
        {
            TxtTextArray[i].color = (i == IntDefaultOption) ? TxtOpinionOn.color : TxtOpinionOff.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAndSelectOption();
    }

    private void ChangeAndSelectOption()
    {
        if (Input.GetButtonDown("Up") && IntNowOption > 0)
        {
            TxtTextArray[IntNowOption].color = TxtOpinionOff.color;
            IntNowOption--;
            TxtTextArray[IntNowOption].color = TxtOpinionOn.color;
        }
        else if (Input.GetButtonDown("Down") && IntNowOption < IntOptionsNumber - 1)
        {
            TxtTextArray[IntNowOption].color = TxtOpinionOff.color;
            IntNowOption++;
            TxtTextArray[IntNowOption].color = TxtOpinionOn.color;
        }
        else if (Input.GetButtonDown("Shoot"))
        {
            switch (IntNowOption)
            {
                case 0:
                    break;
                case 1:
                    TogOther.isOn = !TogOther.isOn;
                    break;
                default:
                    break;
            }
        }
        else if (Input.GetButton("Left"))
        {
            switch (IntNowOption)
            {
                case 0:
                    SystemOptions.SoInstantce.FloMusicVolume -= 0.02F;
                    SystemOptions.SoInstantce.FloMusicVolume = Mathf.Clamp01(SystemOptions.SoInstantce.FloMusicVolume);
                    SliSystemVolume.value = SystemOptions.SoInstantce.FloMusicVolume;
                    break;
                case 1:
                    break;
                default:
                    break;
            }
        }
        else if (Input.GetButton("Right"))
        {
            switch (IntNowOption)
            {
                case 0:
                    SystemOptions.SoInstantce.FloMusicVolume += 0.02F;
                    SystemOptions.SoInstantce.FloMusicVolume = Mathf.Clamp01(SystemOptions.SoInstantce.FloMusicVolume);
                    SliSystemVolume.value = SystemOptions.SoInstantce.FloMusicVolume;
                    break;
                case 1:
                    break;
                default:
                    break;
            }
        }
        else if (Input.GetButtonDown("Boom"))
        {
            MaskController.McInstantce.ChangeCanvas(1, 0);
        }
    }

}
