/***
 *
 *    Project:0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:遮罩控制
 *
 *    Description:
 *    控制前进黑色遮罩的淡入和淡出
 *
 *    Verson:
 *
 *    Author:
 *
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MaskController : MonoBehaviour {

    public GameObject[] GoCanvas;//要切换的画面集

    public Image ImaMusk;
    public static MaskController McInstantce;

    private bool OnMarking = false;
    // Use this for initialization
    void Start ()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        McInstantce = this;
        StartCoroutine("StartGameEnumerator");
    }
	
    public void ChangeCanvas(int canvas1,int canvas2)
    {
        StartCoroutine(ChangeCanvasIEnumerator(canvas1, canvas2));
    }

    public void ChangeScene(int scene)
    {
        StartCoroutine(ChangeSceneIEnumerator(scene));
    }

    public void EndGame()
    {
        StartCoroutine("EndGameEnumerator");
    }

    public IEnumerator StartGameEnumerator()
    {
        StartCoroutine("MaskOffEnumerator");
        yield return new WaitForSeconds(2.0F);
        ChangeCanvas(2,0);
        yield break;
    }

    public IEnumerator EndGameEnumerator()
    {
        StartCoroutine("MaskOnEnumerator");
        yield return new WaitWhile(() => OnMarking);
        Application.Quit();
    }

    public IEnumerator ChangeCanvasIEnumerator(int canvas1,int canvas2)
    {
        StartCoroutine("MaskOnEnumerator");
        yield return new WaitWhile(()=>OnMarking);
        GoCanvas[canvas1].SetActive(false);
        GoCanvas[canvas2].SetActive(true);
        StartCoroutine("MaskOffEnumerator");
        yield break;
    }

    public IEnumerator ChangeSceneIEnumerator(int scene)
    {
        StartCoroutine("MaskOnEnumerator");
        yield return new WaitWhile(() => OnMarking);
        SceneManager.LoadScene(scene);
        yield return new WaitWhile(() => Application.isLoadingLevel);
        StartCoroutine("MaskOffEnumerator");
        yield break;
    }

    public void MaskOn()
    {
        StartCoroutine("MaskOnEnumerator");
    }

    public void MaskOff()
    {
        StartCoroutine("MaskOffEnumerator");
    }

    private IEnumerator MaskOnEnumerator()
    {
        OnMarking = true;
        SystemOptions.SoInstantce.BolOperatingLicense = false;
        while (ImaMusk.color.a < 1F)
        {
            Color colTemp = ImaMusk.color;
            colTemp.a = Mathf.Clamp01(colTemp.a +0.02F);
            ImaMusk.color = colTemp;
            yield return null;
        }
        OnMarking = false;
        yield break;
    }

    private IEnumerator MaskOffEnumerator()
    {
        while (ImaMusk.color.a > 0)
        {
            Color colTemp = ImaMusk.color;
            colTemp.a =Mathf.Clamp01(colTemp.a - 0.02F) ;
            ImaMusk.color = colTemp;
            yield return null;
        }
        SystemOptions.SoInstantce.BolOperatingLicense = true;
        yield break;
    }
}
