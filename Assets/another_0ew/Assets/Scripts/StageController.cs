/***
 *
 *    Project: 0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:关卡控制
 *
 *    Description:
 *    关卡总控
 *    做了很多事情，待总结
 *    
 *    可能用到的数据：Unity坐标下 地图大小1/1（10*15.625）     1/4（5*7.8125）
 *
 *    Verson:1.1
 *
 *    Author:郭为
 *
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    //单例
    public static StageController ScInstantce;//本脚本静态单例
    //文件
    public TextAsset TaStageAsset;//关卡文件
    //预设
    public GameObject GoPerfabBullet;//子弹预设
    public GameObject GoPerfabPlayerBullet;//玩家子弹预设
    public GameObject GoPerfabPlayerBoomCircle;//玩家炸弹环预设
    public GameObject GoPerfabCircle;//判定圈预设
    public GameObject GoPerfabHitResult300;//节奏打击反馈图300预设
    public GameObject GoPerfabHitResult100;//节奏打击反馈图100预设
    public GameObject GoPerfabHitResult50;//节奏打击反馈图50预设
    public GameObject GoPerfabHitResultMiss;//节奏打击反馈图Miss预设
    //对象
    public GameObject GoPlayer;//玩家对象
    public GameObject GoBoss;//Boss对象
    //控件
    public Text TexMusicName;//音乐名称
    public Text TexSorceNumber;//分数
    public Text TexDeathNumber;//死亡数
    public Text TexRhythmComboNumber;//节奏连击数
    public Text TexGrazeComboNumber;//擦弹连击数

    //子弹参数
    public float FloBulletGrazeCircleRadius = 0.5F;//擦弹半径
    //判定参数
    public float FloHitDelayTime = 0.0F;//打击延迟设置
    public float FloHitAllowance50 = 0.3F;//50分判定区间（半）
    public float FloHitAllowance100 = 0.2F;//100分判定区间
    public float FloHitAllowance300 = 0.1F;//300分判定区间
    //判定圈参数
    public float FloCircleStartSize = 0.45F;//圈的初始大小
    public float FloCircleEndSize = 0.15F;//圈的结束大小
    public float FloCircleTime = 0.6F;//缩圈的总时间
    //玩家位移参数
    public float FloPlayerHighSpeed = 10.0F;//高速状态单次移动距离（初速度大小）
    public float FloPlayerLowSpeed = 5.0F;//高速状态单次移动距离
    public float FloPlayerRigidbodyDrag = 10F;//玩家刚体阻力
    //玩家子弹参数
    public float FloPlayerBulletSpeed = 7F;//玩家子弹速度
    public float FloPlayerBulletTrail = 8F;//玩家子弹速度
    public int IntPlayerBulletNumber = 5;//玩家子弹数量
    public int IntPlayerBulletAngle = 30;//玩家子弹起始角度
    public int IntPlayerBulletDeltaAngle = 30;//玩家子弹间隔角度
    public float FloPlayerBulletDestroyTime = 3;//玩家子弹自行销毁时间
    //已弃用，适配FollowingBulletMove.cs
    public float FloPlayerBulletAcceleration = 0.03F;//玩家子弹加速度
    public float FloPlayerBulletDeltaAcceleration = 0.005F;//玩家子弹加速度帧增量
    //玩家炸弹参数
    public float FloPlayerBoomCircleSpeed = 0.1F;//玩家炸弹环扩张速度
    public float FloPlayerBoomCircleTime = 3F;//玩家炸弹环存活时间（不包含淡出部分）
    //地图参数
    public float FloMapTopBoundaryWidth = 0.3F;//地图顶端玩家边界宽度（向内）（玩家禁止跃出）
    public float FloMapBottomBoundaryWidth = 0.3F;//地图顶端玩家边界宽度（向内）（玩家禁止跃出）
    public float FloMapLeftBoundaryWidth = 0.3F;//地图顶端玩家边界宽度（向内）（玩家禁止跃出）
    public float FloMapRightBoundaryWidth = 0.3F;//地图顶端玩家边界宽度（向内）（玩家禁止跃出）
    public float FloMapTopBulletBoundaryWidth = 0.3F;//地图顶端子弹边界宽度（向外）（子弹飞出消失）
    public float FloMapBottomBulletBoundaryWidth = 0.3F;//地图顶端子弹边界宽度（向外）（子弹飞出消失）
    public float FloMapLeftBulletBoundaryWidth = 0.3F;//地图顶端子弹边界宽度（向外）（子弹飞出消失）
    public float FloMapRightBulletBoundaryWidth = 0.3F;//地图顶端子弹边界宽度（向外）（子弹飞出消失）
    //节奏打击反馈图参数
    public float FloHitResultDeltaAlpha = 0.01F;//反馈图淡化步进量

    //解析后的关卡数据
    //private int[,] _IntStageArray = new int[100, 4];//存放解析后的关卡信息的arraylist
    private StageClass Stage;//关卡信息管理类
    //加载完音乐的时间
    private float _FloMusicStartTime;
    //关卡阅读进度标记
    private int _IntBulletMakerCounter = 0;//子弹进度标记
    private int _IntCircleMakerCounter = 0;//判定圈进度标记
    //分数计算数据
    private int _IntScore = 0;//分数
    private int _IntRhythmCombo = 0;//节奏连击
    private int _IntGrazeCombo = 0;//擦弹连击
    private int _IntDeathNumber = 0;//死亡次数
    //按键情况标记
    private bool _BolSlowKeyFlag = false;//低速按键标记

    void Awake()
    {
        ScInstantce = this;//本脚本的静态单例实例化
    }

    void Start()
    {
        //ReadStageAsset();//读取关卡文件
        Stage = new StageClass(TaStageAsset.text);//关卡自动加载
        GoPlayer.GetComponent<Rigidbody2D>().drag = FloPlayerRigidbodyDrag;//玩家刚体阻力初始化
        _IntScore = 0;//分数初始化
        _IntRhythmCombo = 0;//节奏连击初始化
        _IntGrazeCombo = 0;//擦弹连击初始化
        _IntDeathNumber = 0;//死亡次数初始化
        AudioManager.SetAudioBackgroundVolumns(1.0F);
        AudioManager.SetAudioEffectVolumns(1.0F);
        //AudioManager.PlayAudioEffectA("GT");
        //AudioManager.PlayAudioEffectA("RBP");
        AudioManager.PlayAudioEffectA("GuillaumeTell");
        _FloMusicStartTime = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        BulletMaker();
        CircleMaker();
        UpdateKey();
        PlayerPositionCheck();
        RestartCheck();
        QuitCheck();
    }

    #region 测试用
    #region 用于打印数组的测试方法
    //public void testPrint()//用于打印的测试方法
    //{
    //    for (int i = 0; i < 10; i++)
    //    {
    //        for (int j = 0; j < 4; j++)
    //        {
    //            print(i + "," + j + "=" + _IntStageArray[i, j]);
    //        }
    //    }
    //}
    #endregion
    #endregion

    #region 重启处理
    void RestartCheck()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //print("a");
            Application.LoadLevel(Application.loadedLevel);
        }
    }
    #endregion

    #region 退出处理
    void QuitCheck()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //print("b");
            Application.Quit();
        }
    }
    #endregion

    #region 关卡文件读入数组
    //public void ReadStageAsset()
    //{
    //    string strTemp = TaStageAsset.text; //提取出文本信息
    //    string[] strRows = strTemp.Split('\n');//以\n为分解分割关卡信息，提取出每毫秒的操作
    //    for (int i = 0; i < strRows.Length; i++)
    //    {
    //        string[] strColumns = strRows[i].Split(' ');//以空格分割列信息
    //        for (int j = 0; j < strColumns.Length; j++)
    //        {
    //            _IntStageArray[i, j] = int.Parse(strColumns[j]);//存储到数组里
    //        }
    //    }
    //}
    #endregion

    #region 子弹处理
    //public void BulletMaker()
    //{
    //    if (_IntStageArray[_IntBulletMakerCounter, 0] != -1)
    //    {
    //        if ((Time.timeSinceLevelLoad - _FloMusicStartTime - FloHitDelayTime) * 1000 >= _IntStageArray[_IntBulletMakerCounter, 0])
    //        {
    //            MakeABullet(_IntStageArray[_IntBulletMakerCounter, 1], _IntStageArray[_IntBulletMakerCounter, 2], _IntStageArray[_IntBulletMakerCounter, 3]);
    //            _IntBulletMakerCounter++;
    //        }
    //    }
    //}

    public void BulletMaker()
    {
        if (_IntBulletMakerCounter< Stage.HitObjects.LisHitObjects.Count )
        {
            if ((Time.timeSinceLevelLoad - _FloMusicStartTime - FloHitDelayTime) * 1000 >= Stage.HitObjects.LisHitObjects[_IntBulletMakerCounter].IntTime)
            {
                switch(Stage.HitObjects.LisHitObjects[_IntBulletMakerCounter].IntType)
                {
                    case 1:
                        StartCoroutine("MakeASliderBullet", _IntBulletMakerCounter);
                        break;
                    default:
                        MakeABullet(Stage.HitObjects.LisHitObjects[_IntBulletMakerCounter].Vec2Position.x * 2.5F + 160, Stage.HitObjects.LisHitObjects[_IntBulletMakerCounter].Vec2Position.y * 2.5F + 32, 2);
                        break;
                }
                _IntBulletMakerCounter++;
            }
        }
    }

    /// <summary>
    /// 不完善的滑条处理，目前只处理时间方面的问题，在转折点和结束点注册一组子弹，位置是初位置
    /// </summary>
    /// <returns></returns>
    IEnumerator MakeASliderBullet(int intCount)
    {
        float FloSliderTime = (Stage.HitObjects.LisHitObjects[intCount].FloSliderPixelLength * Stage.FloSliderSpeed) / 1000;
        MakeABullet(Stage.HitObjects.LisHitObjects[intCount].Vec2Position.x * 2.5F + 160, Stage.HitObjects.LisHitObjects[intCount].Vec2Position.y * 2.5F + 32, 2);
        for (int i = 0; i < Stage.HitObjects.LisHitObjects[intCount].IntSliderRepeat; i++)
        {
            yield return new WaitForSeconds(FloSliderTime);
            MakeABullet(Stage.HitObjects.LisHitObjects[intCount].Vec2Position.x * 2.5F + 160, Stage.HitObjects.LisHitObjects[intCount].Vec2Position.y * 2.5F + 32, 2);
        }
    }

    public void MakeABullet(float x, float y, int type)
    {
        switch (type)
        {
            case 1:
                //克隆一个子弹对象
                Instantiate(GoPerfabBullet, TranslateVector3(new Vector3(x, y, 0)), Quaternion.Euler(0, 0, 0));
                break;
            case 2://一圈子弹
                Instantiate(GoPerfabBullet, TranslateVector3(new Vector3(x, y, 0)), Quaternion.Euler(0, 0, 0));
                Instantiate(GoPerfabBullet, TranslateVector3(new Vector3(x, y, 0)), Quaternion.Euler(0, 0, 36));
                Instantiate(GoPerfabBullet, TranslateVector3(new Vector3(x, y, 0)), Quaternion.Euler(0, 0, 72));
                Instantiate(GoPerfabBullet, TranslateVector3(new Vector3(x, y, 0)), Quaternion.Euler(0, 0, 108));
                Instantiate(GoPerfabBullet, TranslateVector3(new Vector3(x, y, 0)), Quaternion.Euler(0, 0, 144));
                Instantiate(GoPerfabBullet, TranslateVector3(new Vector3(x, y, 0)), Quaternion.Euler(0, 0, 180));
                Instantiate(GoPerfabBullet, TranslateVector3(new Vector3(x, y, 0)), Quaternion.Euler(0, 0, -36));
                Instantiate(GoPerfabBullet, TranslateVector3(new Vector3(x, y, 0)), Quaternion.Euler(0, 0, -72));
                Instantiate(GoPerfabBullet, TranslateVector3(new Vector3(x, y, 0)), Quaternion.Euler(0, 0, -108));
                Instantiate(GoPerfabBullet, TranslateVector3(new Vector3(x, y, 0)), Quaternion.Euler(0, 0, -144));
                break;
            default:
                break;
        }
    }

    public void MakePlayerBullet()
    {
        for (int i = 0, j = IntPlayerBulletAngle; i < IntPlayerBulletNumber; i++, j += IntPlayerBulletDeltaAngle)
        {
            Instantiate(GoPerfabPlayerBullet, new Vector3(GoPlayer.transform.position.x, GoPlayer.transform.position.y, 0), Quaternion.Euler(0, 0, j));
        }
    }
    #endregion

    #region 炸弹处理
    public void MakePlayerBoom()
    {
        Instantiate(GoPerfabPlayerBoomCircle, GoPlayer.transform.position, Quaternion.identity);      
    }
    #endregion

    #region 判定圈处理
    public void CircleMaker()
    {
        if (_IntCircleMakerCounter< Stage.HitObjects.LisHitObjects.Count)
        {
            if ((Time.timeSinceLevelLoad + FloCircleTime -FloHitDelayTime - _FloMusicStartTime) * 1000 >= Stage.HitObjects.LisHitObjects[_IntCircleMakerCounter].IntTime)
            {
                switch (Stage.HitObjects.LisHitObjects[_IntCircleMakerCounter].IntType)
                {
                    case 1:
                        StartCoroutine("MakeASliderCircle", _IntBulletMakerCounter);
                        break;
                    default:
                        MakeACircle();
                        break;
                }
                _IntCircleMakerCounter++;
            }
        }
    }

    IEnumerator MakeASliderCircle(int intCount)
    {
        float FloSliderTime = (Stage.HitObjects.LisHitObjects[intCount].FloSliderPixelLength * Stage.FloSliderSpeed) / 1000;
        MakeACircle();
        for (int i = 0; i < Stage.HitObjects.LisHitObjects[intCount].IntSliderRepeat; i++)
        {
            yield return new WaitForSeconds(FloSliderTime);
            MakeACircle();
        }
    }

    public void MakeACircle()
    {
        GameObject goTemp = Instantiate(GoPerfabCircle, GoPlayer.transform.position, GoPlayer.transform.rotation);
        goTemp.transform.parent = GoPlayer.transform;

    }
    #endregion

    #region 判定器反馈处理
    public void DealHit(string result)
    {
        switch (result)
        {
            case "Miss":
                //print("Miss");
                AddScore("Miss");
                ShowHitResult("Miss");
                break;
            case "300":
                //print("300");
                ShowHitResult("300");
                AddScore("300");
                break;
            case "100":
                //print("100");
                ShowHitResult("100");
                AddScore("300");
                break;
            case "50":
                //print("50");
                ShowHitResult("50");
                AddScore("300");
                break;
            default:
                break;
        }
    }

    public void PlayerAction(string str)
    {
        switch (str)
        {
            case "N":
                GoPlayer.GetComponent<Rigidbody2D>().velocity = Vector3.up * (_BolSlowKeyFlag ? FloPlayerLowSpeed : FloPlayerHighSpeed);
                _IntRhythmCombo += 1;
                break;
            case "NE":
                GoPlayer.GetComponent<Rigidbody2D>().velocity = (Vector3.up + Vector3.right) * (_BolSlowKeyFlag ? FloPlayerLowSpeed : FloPlayerHighSpeed) * 0.707F;
                _IntRhythmCombo += 1;
                break;
            case "E":
                GoPlayer.GetComponent<Rigidbody2D>().velocity = Vector3.right * (_BolSlowKeyFlag ? FloPlayerLowSpeed : FloPlayerHighSpeed);
                _IntRhythmCombo += 1;
                break;
            case "SE":
                GoPlayer.GetComponent<Rigidbody2D>().velocity = (Vector3.down + Vector3.right) * (_BolSlowKeyFlag ? FloPlayerLowSpeed : FloPlayerHighSpeed) * 0.707F;
                _IntRhythmCombo += 1;
                break;
            case "S":
                GoPlayer.GetComponent<Rigidbody2D>().velocity = Vector3.down * (_BolSlowKeyFlag ? FloPlayerLowSpeed : FloPlayerHighSpeed);
                _IntRhythmCombo += 1;
                break;
            case "SW":
                GoPlayer.GetComponent<Rigidbody2D>().velocity = (Vector3.down + Vector3.left) * (_BolSlowKeyFlag ? FloPlayerLowSpeed : FloPlayerHighSpeed) * 0.707F;
                _IntRhythmCombo += 1;
                break;
            case "W":
                GoPlayer.GetComponent<Rigidbody2D>().velocity = Vector3.left * (_BolSlowKeyFlag ? FloPlayerLowSpeed : FloPlayerHighSpeed);
                _IntRhythmCombo += 1;
                break;
            case "NW":
                GoPlayer.GetComponent<Rigidbody2D>().velocity = (Vector3.up + Vector3.left) * (_BolSlowKeyFlag ? FloPlayerLowSpeed : FloPlayerHighSpeed) * 0.707F;
                _IntRhythmCombo += 1;
                break;
            case "Shoot":
                MakePlayerBullet();
                _IntRhythmCombo += 1;
                break;
            case "Boom":
                MakePlayerBoom();
                _IntRhythmCombo += 1;
                _IntGrazeCombo = 0;
                break;
            case "Null":
                _IntRhythmCombo = 0;
                break;
            default:
                break;
        }
    }
    #endregion

    #region 擦弹处理
    public void GrazeBullet()
    {
        _IntGrazeCombo += 1;
        AddScore("Graze");
    }
    #endregion

    #region 撞弹处理
    public void CollideBullet()
    {
        _IntGrazeCombo = 0;
        _IntRhythmCombo = 0;
        _IntDeathNumber++;
        AddScore("Collide");
    }
    #endregion

    #region 玩家位置修正（防止越界）
    /// <summary>
    /// 玩家的位置修正
    /// 常驻本脚本Update
    /// 目前能够做防止越界的修正
    /// </summary>
    public void PlayerPositionCheck()
    {
        //防止越界的修正，其中的5和7.8125是整个版面在Unity坐标中的半长
        GoPlayer.transform.localPosition = new Vector3(Mathf.Clamp(GoPlayer.transform.position.x, -7.8125F + FloMapLeftBoundaryWidth, 7.8125F - FloMapRightBoundaryWidth), Mathf.Clamp(GoPlayer.transform.position.y, -5 + FloMapBottomBoundaryWidth, 5 - FloMapTopBoundaryWidth), 0);
    }
    #endregion

    #region 键位检测
    /// <summary>
    /// 用于各种单次的检测按键，返回处理过的组合键信息
    /// 使用时需要从本脚本单例中调用
    /// 目前能够处理方向键和Shoot Boom
    /// </summary>
    /// <returns>返回字符串，如果后面复杂了可能要出一张表</returns>
    public string GetKey()
    {
        if (Input.GetButtonDown("Up"))//根据方向键位得到计算出方向
        {
            if (Input.GetButtonDown("Down"))
            {
                if (Input.GetButtonDown("Left"))
                {
                    if (Input.GetButtonDown("Right"))
                    {
                        if (Input.GetButtonDown("Boom"))
                        {
                            return "Boom";
                        }
                        else if (Input.GetButtonDown("Shoot"))
                        {
                            return "Shoot";
                        }
                        else
                        {
                            return "Null";
                        }
                    }
                    else
                    {
                        return "W";
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Right"))
                    {
                        return "E";
                    }
                    else
                    {
                        if (Input.GetButtonDown("Boom"))
                        {
                            return "Boom";
                        }
                        else if (Input.GetButtonDown("Shoot"))
                        {
                            return "Shoot";
                        }
                        else
                        {
                            return "Null";
                        }
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown("Left"))
                {
                    if (Input.GetButtonDown("Right"))
                    {
                        return "N";
                    }
                    else
                    {
                        return "NW";
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Right"))
                    {
                        return "NE";
                    }
                    else
                    {
                        return "N";
                    }
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Down"))
            {
                if (Input.GetButtonDown("Left"))
                {
                    if (Input.GetButtonDown("Right"))
                    {
                        return "S";
                    }
                    else
                    {
                        return "SW";
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Right"))
                    {
                        return "SE";
                    }
                    else
                    {
                        return "S";
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown("Left"))
                {
                    if (Input.GetButtonDown("Right"))
                    {
                        if (Input.GetButtonDown("Boom"))
                        {
                            return "Boom";
                        }
                        else if (Input.GetButtonDown("Shoot"))
                        {
                            return "Shoot";
                        }
                        else
                        {
                            return "Null";
                        }
                    }
                    else
                    {
                        return "W";
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Right"))
                    {
                        return "E";
                    }
                    else
                    {
                        if (Input.GetButtonDown("Boom"))
                        {
                            return "Boom";
                        }
                        else if (Input.GetButtonDown("Shoot"))
                        {
                            return "Shoot";
                        }
                        else
                        {
                            return "Null";
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 用于更新各种按键信息，直接修改某些标记变量
    /// 常驻本脚本Update
    /// 目前能够处理Slow键的按下状态
    /// </summary>
    public void UpdateKey()
    {
        if (Input.GetButton("Slow"))
        {
            _BolSlowKeyFlag = true;
        }
        else
        {
            _BolSlowKeyFlag = false;
        }
    }
    #endregion

    #region 分数累加
    /* 计分规则
     * 随便写的，科学性待论证
     * 1.节奏加分=Combo*单键分值(300/100/50)
     * 2.反击加分(针对诱导弹，固定弹大概需要别的算法)=90*Combo
     * 3.擦弹加分=连续擦弹数*Combo
     * 4.撞弹扣分=分数减半
     * 5.所有的Miss中断Combo，撞弹中断Combo和连续擦弹，使用Boom中断连续擦弹
     */
    public void AddScore(string strItem)
    {
        switch (strItem)
        {
            case "50":
                _IntScore += _IntRhythmCombo * 50;
                break;
            case "100":
                _IntScore += _IntRhythmCombo * 100;
                break;
            case "300":                            //节奏
                _IntScore += _IntRhythmCombo * 300;
                break;
            case "TrailBullet":                    //诱导弹
                _IntScore += _IntRhythmCombo * 90;
                break;
            case "Graze":                          //擦弹
                _IntScore += _IntRhythmCombo * _IntGrazeCombo;
                break;
            case "Collide": //撞弹
                _IntScore = _IntScore / 2;
                break;
            case "Miss"://Miss 用于更新UI和后续扩充规则
                break;
            default:
                break;
        }
        UIDataUpdate();
    }
    #endregion

    #region UI更新
    void UIDataUpdate()
    {
        TexSorceNumber.text = _IntScore.ToString();
        TexRhythmComboNumber.text = _IntRhythmCombo.ToString();
        TexGrazeComboNumber.text = _IntGrazeCombo.ToString();
        TexDeathNumber.text = _IntDeathNumber.ToString();
    }
    void ShowHitResult(string Result)
    {
        switch(Result)
        {
            case "300":
                Instantiate(GoPerfabHitResult300, GoPlayer.transform.position, Quaternion.identity);
                break;
            case "100":
                Instantiate(GoPerfabHitResult100, GoPlayer.transform.position, Quaternion.identity);
                break;
            case "50":
                Instantiate(GoPerfabHitResult50, GoPlayer.transform.position, Quaternion.identity);
                break;
            case "Miss":
                Instantiate(GoPerfabHitResultMiss, GoPlayer.transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
    }
    #endregion

    #region 数学
    /// <summary>
    /// 将以左上为原点，以右和下为xy轴的osu坐标转换为以中心为原点，以右和上为xy轴的Unity坐标
    /// </summary>
    /// <param name="vec">需要变换的三维向量</param>
    /// <returns></returns>
    public Vector3 TranslateVector3(Vector3 vec)
    {
        Vector3 Result = vec;
        Result.x = (vec.x - 800) / 102.4F;
        Result.y = -(vec.y - 512) / 102.4F;
        return Result;
    }
    #endregion

}
