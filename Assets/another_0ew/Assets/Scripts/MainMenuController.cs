/***
 *
 *    Project:0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:主菜单控制
 *
 *    Description:
 *    控制主菜单
 *    可能会写成一个菜单通用模型
 *
 *    Verson:1.0
 *
 *    Author:郭为
 *
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Text[] TxtTextArray;//选项的文字数组
    public int IntOptionsNumber = 4;//总选项数，至少为1
    public int IntDefaultOption = 0;//默认的选项

    public Text TxtOpinionOn;//选项选中的预制效果，目前支持颜色
    public Text TxtOpinionOff;//选项未选中的预制效果，目前支持颜色

    public GameObject GoMusk;//黑屏遮罩
    public Image ImaMusk;//场景遮罩图片

    public GameObject GoOptionMenuCanvas;//选项菜单

    private int IntNowOption=0;//当前选项编号，首项编号为0
	// Use this for initialization
	void Start ()
    {
        //StartCoroutine("MaskOff");
        IntNowOption = 0;
        for(int i=0;i<TxtTextArray.Length;i++)
        {
            TxtTextArray[i].color = (i == IntDefaultOption) ? TxtOpinionOn.color : TxtOpinionOff.color;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        ChangeAndSelectOption();
	}

    private void ChangeAndSelectOption()
    {
        if (Input.GetButtonDown("Up")&&IntNowOption>0)
        {
            TxtTextArray[IntNowOption].color = TxtOpinionOff.color;
            IntNowOption--;
            TxtTextArray[IntNowOption].color = TxtOpinionOn.color;
        }
        else if(Input.GetButtonDown("Down")&&IntNowOption<IntOptionsNumber)
        {
            TxtTextArray[IntNowOption].color = TxtOpinionOff.color;
            IntNowOption++;
            TxtTextArray[IntNowOption].color = TxtOpinionOn.color;
        }
        else if(Input.GetButtonDown("Shoot"))
        {
            switch(IntNowOption)
            {
                case 0:
                    GameStart_mod1();
                    break;
                case 1:
                    GameStart();
                    break;
                case 2:
                    EnterOpinions();
                    break;
                case 3:
                    Exit();
                    break;
                default:
                    break;
            }
        }
    }

    private void GameStart()
    {
        //SceneManager.LoadScene(1);
        MaskController.McInstantce.ChangeScene(1);
    }
    private void GameStart_mod1()
    {
        //SceneManager.LoadScene(1);
        MaskController.McInstantce.ChangeScene(2);
    }

    private void EnterOpinions()
    {
        MaskController.McInstantce.ChangeCanvas(0, 1);
    }

    private void Exit()
    {
        MaskController.McInstantce.EndGame();
    }

}
