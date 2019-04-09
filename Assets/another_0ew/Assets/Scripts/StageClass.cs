/***
 *
 *    Project:0 error(s), 0 warning(s) C Demo Remaked Verson in Unity
 *
 *    Title:Stage类
 *
 *    Description:
 *    阅读关卡文件
 *    管理关卡信息
 *    
 *    待完善：
 *    将ErrorFlag改为ErrorCode，使之能表示错误位置
 *    修改类名
 *
 *    Verson:1.0
 *
 *    Author:郭为
 *
 */


using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class StageClass
{
    public bool BolErrorFlag = false;//当关卡阅读错误时激活，可以用来检测其他版本谱面中的未考虑项
    //关卡静态数据(全部一次性来源于关卡文件，不再做修改)
    public string StrFileFormat;//字符串格式的关卡文件版本，就是文件的第一行
    public int IntFileFormat;//int格式的关卡文件版本，就是v后面的数字
    public GeneralClass General = new GeneralClass();
    public EditorClass Editor = new EditorClass();
    public MetadataClass Metadata = new MetadataClass();
    public DifficultyClass Difficulty = new DifficultyClass();
    public EventsClass Events = new EventsClass();
    public TimingPointsClass TimingPoints = new TimingPointsClass();
    public ColoursClass Colours = new ColoursClass();
    public HitObjectsClass HitObjects = new HitObjectsClass();
    //关卡动态数据(根据关卡的推进可能会修改的数据)
    public float FloBeatDuration;//随TimingPoint变化的MPB
    public float FloSliderSpeed;//计算出的滑条速度，直接乘以真实长度等于滑条时间

    /// <summary>
    /// 传入整个关卡文件的字符串
    /// </summary>
    /// <param name="strStage"></param>
    public StageClass(string strAllStage)
    {
        ReadStage(strAllStage);
        FloBeatDuration = TimingPoints.AlPoints[0].FloMillisecondsPerBeat;
        FloSliderSpeed = FloBeatDuration / (100 * Difficulty.FloSliderMultiplier);
    }

    private void ReadStage(string strAllStage)
    {
        string[] strParagraphs = Regex.Split(strAllStage, "\r\n\r\n");
        ReadFileFormat(strParagraphs[0]);
        for (int i = 1; i < strParagraphs.Length; i++)
        {
            string strTitle = strParagraphs[i].Substring((strParagraphs[i].IndexOf('[') + 1), (strParagraphs[i].IndexOf(']') - strParagraphs[i].IndexOf('[') - 1));
            switch(strTitle)
            {
                case "General":
                    ReadGeneral(strParagraphs[i]);
                    break;
                case "Editor":
                    ReadEditor(strParagraphs[i]);
                    break;
                case "Metadata":
                    ReadMetadata(strParagraphs[i]);
                    break;
                case "Difficulty":
                    ReadDifficulty(strParagraphs[i]);
                    break;
                case "Events":
                    ReadEvents(strParagraphs[i]);
                    break;
                case "TimingPoints":
                    ReadTimingPoints(strParagraphs[i]);
                    break;
                case "Colours":
                    ReadColours(strParagraphs[i]);
                    break;
                case "HitObjects":
                    ReadHitObjects(strParagraphs[i]);
                    break;
            }
        }
    }

    private void ReadFileFormat(string strFileFormat)
    {
        StrFileFormat = strFileFormat;
        string[] strWords = strFileFormat.Split(' ');
        string strFormatNumber = strWords[3].Substring(1);
        IntFileFormat = int.Parse(strFormatNumber);
    }

    private void ReadGeneral(string strGeneral)
    {
        string[] strLines = strGeneral.Split('\n');
        for (int i = 1; i < strLines.Length; i++)//i从1开始跳过了[General]那一行
        {
            string[] strNameAndParameter = strLines[i].Split(':');
            string strName = strNameAndParameter[0].Trim();
            string strParameter = strNameAndParameter[1].Trim();
            switch (strName)
            {
                case "AudioFilename":
                    General.StrAudioFilename = strParameter;
                    break;
                case "AudioLeadIn":
                    General.IntAudioLeadIn = int.Parse(strParameter);
                    break;
                case "PreviewTime":
                    General.IntPreviewTime = int.Parse(strParameter);
                    break;
                case "Countdown":
                    General.IntCountdown = int.Parse(strParameter);
                    break;
                case "SampleSet":
                    General.StrSampleSet = strParameter;
                    break;
                case "StackLeniency":
                    General.FloStackLeniency = float.Parse(strParameter);
                    break;
                case "Mode":
                    General.IntMode = int.Parse(strParameter);
                    break;
                case "LetterboxInBreaks":
                    General.IntLetterboxInBreaks = int.Parse(strParameter);
                    break;
                case "WidescreenStoryboard":
                    General.IntWidescreenStoryboard = int.Parse(strParameter);
                    break;
                default:
                    BolErrorFlag = true;
                    break;
            }
        }
    }

    private void ReadEditor(string strEditor)
    {
        string[] strLines = strEditor.Split('\n');
        for (int i = 1; i < strLines.Length; i++)//i从1开始跳过了[Editor]那一行
        {
            string[] strNameAndParameter = strLines[i].Split(':');
            string strName = strNameAndParameter[0].Trim();
            string strParameter = strNameAndParameter[1].Trim();
            switch (strName)
            {
                case "Bookmarks":
                    string[] strNumbers = strParameter.Split(',');
                    for (int j = 0; j < strNumbers.Length; j++)
                    {
                        int intNumber = int.Parse(strNumbers[j].Trim());
                        Editor.AlBookmarks.Add(intNumber);
                    }
                    break;
                case "DistanceSpacing":
                    Editor.FloDistanceSpacing = float.Parse(strParameter);
                    break;
                case "BeatDivisor":
                    Editor.IntBeatDivisor = int.Parse(strParameter);
                    break;
                case "GridSize":
                    Editor.IntGridSize = int.Parse(strParameter);
                    break;
                case "TimelineZoom":
                    Editor.FloTimelineZoom = float.Parse(strParameter);
                    break;
                default:
                    BolErrorFlag = true;
                    break;
            }
        }
    }

    private void ReadMetadata(string strMetadata)
    {
        string[] strLines = strMetadata.Split('\n');
        for (int i = 1; i < strLines.Length; i++)//跳过标题行
        {
            string[] strNameAndParameter = strLines[i].Split(':');
            string strName = strNameAndParameter[0].Trim();
            string strParameter = strNameAndParameter[1].Trim();
            switch (strName)
            {
                case "Title":
                    Metadata.StrTitle = strParameter;
                    break;
                case "TitleUnicode":
                    Metadata.StrTitleUnicode = strParameter;
                    break;
                case "Artist":
                    Metadata.StrArtist = strParameter;
                    break;
                case "ArtistUnicode":
                    Metadata.StrArtistUnicode = strParameter;
                    break;
                case "Creator":
                    Metadata.StrCreator = strParameter;
                    break;
                case "Version":
                    Metadata.StrVersion = strParameter;
                    break;
                case "Source":
                    Metadata.StrSource = strParameter;
                    break;
                case "Tags":
                    string[] strTags = strParameter.Split(' ');
                    for (int j = 0; j < strTags.Length; j++)
                    {
                        Metadata.AlTags.Add(strTags[j]);
                    }
                    break;
                case "BeatmapID":
                    Metadata.IntBeatmapID = int.Parse(strParameter);
                    break;
                case "BeatmapSetID":
                    Metadata.IntBeatmapSetID = int.Parse(strParameter);
                    break;
                default:
                    BolErrorFlag = true;
                    break;
            }
        }
    }

    private void ReadDifficulty(string strDifficulty)
    {
        string[] strLines = strDifficulty.Split('\n');
        for (int i = 1; i < strLines.Length; i++)//跳过标题行
        {
            string[] strNameAndParameter = strLines[i].Split(':');
            string strName = strNameAndParameter[0].Trim();
            string strParameter = strNameAndParameter[1].Trim();
            switch (strName)
            {
                case "HPDrainRate":
                    Difficulty.FloHPDrainRate = float.Parse(strParameter);
                    break;
                case "CircleSize":
                    Difficulty.FloCircleSize = float.Parse(strParameter);
                    break;
                case "OverallDifficulty":
                    Difficulty.FloOverallDifficulty = float.Parse(strParameter);
                    break;
                case "ApproachRate":
                    Difficulty.FloApproachRate = float.Parse(strParameter);
                    break;
                case "SliderMultiplier":
                    Difficulty.FloSliderMultiplier = float.Parse(strParameter);
                    break;
                case "SliderTickRate":
                    Difficulty.FloSliderTickRate = float.Parse(strParameter);
                    break;
                default:
                    BolErrorFlag = true;
                    break;
            }
        }
    }

    private void ReadEvents(string strEvents)
    {
        string[] strLines = strEvents.Split('\n');
        for (int i = 1; i < strLines.Length; i++)//跳过标题行
        {
            if (strLines[i].IndexOf("//") != -1)
            {
                strLines[i] = strLines[i].Remove(strLines[i].IndexOf("//"));
            }
            if (strLines[i] != "")
            {
                string[] strEventParameters = strLines[i].Split(',');
                Events.AlEvents.Add(strEventParameters);
            }
        }
    }

    private void ReadTimingPoints(string strTimingPoints)
    {
        string[] strLines = strTimingPoints.Split('\n');
        for (int i = 1; i < strLines.Length; i++)//跳过标题行
        {
            string[] strPointParameters = strLines[i].Split(',');
            if (strPointParameters.Length != 8)//参数数量检定
            {
                BolErrorFlag = true;
            }
            TimingPointClass TpPoint = new TimingPointClass();
            TpPoint.IntOffset = int.Parse(strPointParameters[0].Trim());
            TpPoint.FloMillisecondsPerBeat = float.Parse(strPointParameters[1].Trim());
            TpPoint.IntMeter = int.Parse(strPointParameters[2].Trim());
            TpPoint.IntSampleSet = int.Parse(strPointParameters[3].Trim());
            TpPoint.IntSampleIndex = int.Parse(strPointParameters[4].Trim());
            TpPoint.IntVolume = int.Parse(strPointParameters[5].Trim());
            TpPoint.IntInherited = int.Parse(strPointParameters[6].Trim());
            TpPoint.IntKiaiMode = int.Parse(strPointParameters[7].Trim());
            TimingPoints.AlPoints.Add(TpPoint);
        }
    }

    private void ReadColours(string strColours)
    {
        strColours = strColours.Remove(0, strColours.IndexOf('['));
        string[] strLines = strColours.Split('\n');
        for (int i = 1; i < strLines.Length; i++)//跳过标题行
        {
            string[] strNameAndParameter = strLines[i].Split(':');
            string[] strColourParameters = (strNameAndParameter[1].Trim()).Split(',');
            if (strColourParameters.Length != 3)//参数数量检定
            {
                BolErrorFlag = true;
            }
            Color colColor = new Color(0, 0, 0, 1);
            colColor.r = int.Parse(strColourParameters[0]);
            colColor.g = int.Parse(strColourParameters[1]);
            colColor.b = int.Parse(strColourParameters[2]);
            Colours.LisCombos.Add(colColor);
        }
    }

    private void ReadHitObjects(string strHitObjects)
    {
        strHitObjects = strHitObjects.Remove(0, strHitObjects.IndexOf('['));
        string[] strLines = (strHitObjects.Trim()).Split('\n');
        for (int i = 1; i < strLines.Length; i++)//跳过标题行
        {
            string[] strParameters = (strLines[i].Trim()).Split(',');
            HitObjectClass hoHitObject = new HitObjectClass();
            hoHitObject.Vec2Position.x = int.Parse(strParameters[0]);
            hoHitObject.Vec2Position.y = int.Parse(strParameters[1]);
            hoHitObject.IntTime = int.Parse(strParameters[2]);
            hoHitObject.IntUnresolvedType = int.Parse(strParameters[3]);
            ResolveHitObjectsType(hoHitObject.IntUnresolvedType, out hoHitObject.IntType, out hoHitObject.IntNewCombo, out hoHitObject.IntSkipColor);
            hoHitObject.IntUnresolvedHitSound = int.Parse(strParameters[4]);
            ResolveHitObjectsHitSound(hoHitObject.IntUnresolvedHitSound, out hoHitObject.IntHitSoundNormal, out hoHitObject.IntHitSoundWhistle, out hoHitObject.IntHitSoundFinish, out hoHitObject.IntHitSoundClap);
            switch (hoHitObject.IntType)
            {
                case 0:
                    ReadHitObjectsCircle(strParameters, ref hoHitObject);
                    break;
                case 1:
                    ReadHitObjectsSlider(strParameters, ref hoHitObject);
                    break;
                case 2:
                    ReadHitObjectsSpinner(strParameters, ref hoHitObject);
                    break;
                case 3:
                    ReadHitObjectsManiaHoldNote(strParameters, ref hoHitObject);
                    break;
            }
            HitObjects.LisHitObjects.Add(hoHitObject);
        }
    }

    private void ResolveHitObjectsType(int intUnresolvedType, out int intType, out int intNewCombo, out int intSkipColour)
    {
        int[] intResolvedType = new int[8];
        for (int i = 0; i < 8; i++)
        {
            intResolvedType[i] = intUnresolvedType % 2;
            intUnresolvedType = intUnresolvedType / 2;
        }

        intNewCombo = intResolvedType[2];

        if (intResolvedType[0] + intResolvedType[1] + intResolvedType[3] + intResolvedType[7] != 1)
        {
            BolErrorFlag = true;
            intType = 0;
        }
        if (intResolvedType[0] == 1)
        {
            intType = 0;
        }
        else if (intResolvedType[1] == 1)
        {
            intType = 1;
        }
        else if (intResolvedType[3] == 1)
        {
            intType = 2;
        }
        else if (intResolvedType[7] == 1)
        {
            intType = 3;
        }
        else
        {
            intType = 0;
        }

        intSkipColour = intResolvedType[4] + intResolvedType[5] * 2 + intResolvedType[6] * 4;
    }

    private void ResolveHitObjectsHitSound(int intUnresolvedHitSound, out int intHitSoundNormal, out int intHitSoundWhistle, out int intHitSoundFinish, out int intHitSoundClap)
    {
        int[] intResolvedHitSound = new int[4];
        for (int i = 0; i < 4; i++)
        {
            intResolvedHitSound[i] = intUnresolvedHitSound % 2;
            intUnresolvedHitSound = intUnresolvedHitSound / 2;
        }

        intHitSoundNormal = intResolvedHitSound[0];
        intHitSoundWhistle = intResolvedHitSound[1];
        intHitSoundFinish = intResolvedHitSound[2];
        intHitSoundClap = intResolvedHitSound[3];
    }

    private void ReadHitObjectsCircle(string[] strParameters, ref HitObjectClass hoHitObject)
    {
        if(strParameters.Length>5)
        {
            ReadHitObjectsExtra(strParameters[5], ref hoHitObject);
        }
    }

    private void ReadHitObjectsSlider(string[] strParameters, ref HitObjectClass hoHitObject)
    {
        string[] strSliderParameter = strParameters[5].Split('|');
        switch (strSliderParameter[0])
        {
            case "L":
                hoHitObject.IntSliderType = 0;
                break;
            case "P":
                hoHitObject.IntSliderType = 1;
                break;
            case "B":
                hoHitObject.IntSliderType = 2;
                break;
            case "C":
                hoHitObject.IntSliderType = 3;
                break;
            default:
                BolErrorFlag = true;
                hoHitObject.IntSliderType = 0;
                break;
        }
        for (int i = 1; i < strSliderParameter.Length; i++)
        {
            Vector2 vec2Point = new Vector2();
            string[] strPosition = strSliderParameter[i].Split(':');
            vec2Point.x = int.Parse(strPosition[0]);
            vec2Point.y = int.Parse(strPosition[1]);
            hoHitObject.LisSliderPoints.Add(vec2Point);
        }

        hoHitObject.IntSliderRepeat = int.Parse(strParameters[6]);

        hoHitObject.FloSliderPixelLength = float.Parse(strParameters[7]);
        if (strParameters.Length > 8)
        {
            string[] strEdgeHitSoundsParameter = strParameters[8].Split('|');
            for (int i = 0; i < strEdgeHitSoundsParameter.Length; i++)
            {
                hoHitObject.LisSliderEdgeHitSounds.Add(int.Parse(strEdgeHitSoundsParameter[i]));
            }
        }

        if (strParameters.Length > 9)
        {
            string[] strEdgeEdgeAdditionParameter = strParameters[9].Split('|');
            for (int i = 0; i < strEdgeEdgeAdditionParameter.Length; i++)
            {
                string[] strSampleSetAndAdditionSet = strEdgeEdgeAdditionParameter[i].Split(':');
                EdgeAddition eaEdgeAddition = new EdgeAddition();
                eaEdgeAddition.IntSampleSet = int.Parse(strSampleSetAndAdditionSet[0]);
                eaEdgeAddition.IntAdditionSet = int.Parse(strSampleSetAndAdditionSet[1]);
                hoHitObject.LisSliderEdgeAddition.Add(eaEdgeAddition);
            }
        }
        if (strParameters.Length > 10)
        {
            ReadHitObjectsExtra(strParameters[10], ref hoHitObject);
        }
    }

    private void ReadHitObjectsSpinner(string[] strParameters, ref HitObjectClass hoHitObject)
    {
        hoHitObject.IntEndtime = int.Parse(strParameters[5]);
        if(strParameters.Length>6)
        {
            ReadHitObjectsExtra(strParameters[6], ref hoHitObject);
        }
    }

    private void ReadHitObjectsManiaHoldNote(string[] strParameters, ref HitObjectClass hoHitObject)
    {
        string[] strEndTimeAndExtra = strParameters[5].Split(':');
        hoHitObject.IntEndtime = int.Parse(strEndTimeAndExtra[0]);
        ReadHitObjectsExtra(strEndTimeAndExtra[2], ref hoHitObject);
    }

    private void ReadHitObjectsExtra(string strExtra, ref HitObjectClass hoHitObject)
    {
        string[] strExtraParameter = strExtra.Split(':');
        hoHitObject.IntExtraSampleSet = int.Parse(strExtraParameter[0]);
        hoHitObject.IntExtraAdditionSet = int.Parse(strExtraParameter[1]);
        hoHitObject.IntExtraCustomIndex = int.Parse(strExtraParameter[2]);
        hoHitObject.IntExtraSampleVolume = int.Parse(strExtraParameter[3]);
        switch (strExtraParameter.Length)
        {
            case 4:
                hoHitObject.StrExtraFilename = "";
                break;
            case 5:
                hoHitObject.StrExtraFilename = strExtraParameter[4];
                break;
            default:
                hoHitObject.StrExtraFilename = "";
                BolErrorFlag = true;
                break;
        }
    }

    public void TestPrintStageClass()
    {
        //ErrorFlag
        Debug.Log("BolErrorFlag =" + BolErrorFlag);
        //Format
        Debug.Log("Format:");
        Debug.Log("StrFileFormat =" + StrFileFormat);
        Debug.Log("IntFileFormat =" + IntFileFormat);
        //General
        Debug.Log("General:");
        Debug.Log("AudioFilename =" + General.StrAudioFilename);
        Debug.Log("AudioLeadIn =" + General.IntAudioLeadIn);
        Debug.Log("PreviewTime =" + General.IntPreviewTime);
        Debug.Log("Countdown =" + General.IntCountdown);
        Debug.Log("SampleSet =" + General.StrSampleSet);
        Debug.Log("StackLeniency =" + General.FloStackLeniency);
        Debug.Log("Mode =" + General.IntMode);
        Debug.Log("LetterboxInBreaks =" + General.IntLetterboxInBreaks);
        Debug.Log("WidescreenStoryboard =" + General.IntWidescreenStoryboard);
        //Editor
        Debug.Log("Editor:");
        string strBookmarksParameter = "";
        for (int i = 0; i < Editor.AlBookmarks.Count; i++)
        {
            strBookmarksParameter = strBookmarksParameter + Editor.AlBookmarks[i] + ",";
        }
        Debug.Log("Bookmarks =" + strBookmarksParameter);
        Debug.Log("DistanceSpacing =" + Editor.FloDistanceSpacing);
        Debug.Log("BeatDivisor =" + Editor.IntBeatDivisor);
        Debug.Log("GridSize =" + Editor.IntGridSize);
        Debug.Log("TimelineZoom =" + Editor.FloTimelineZoom);
        //Metadata
        Debug.Log("Metadata:");
        Debug.Log("Title =" + Metadata.StrTitle);
        Debug.Log("TitleUnicode =" + Metadata.StrTitleUnicode);
        Debug.Log("Artist =" + Metadata.StrArtist);
        Debug.Log("ArtistUnicode =" + Metadata.StrArtistUnicode);
        Debug.Log("Creator =" + Metadata.StrCreator);
        Debug.Log("Version =" + Metadata.StrVersion);
        Debug.Log("Source =" + Metadata.StrSource);
        string strTagsParameter = "";
        for (int i = 0; i < Metadata.AlTags.Count; i++)
        {
            strTagsParameter = strTagsParameter + Metadata.AlTags[i] + " ";
        }
        Debug.Log("Tags =" + strTagsParameter);
        Debug.Log("BeatmapID =" + Metadata.IntBeatmapID);
        Debug.Log("BeatmapSetID=" + Metadata.IntBeatmapSetID);
        //Difficulty
        Debug.Log("Difficulty:");
        Debug.Log("HPDrainRate =" + Difficulty.FloHPDrainRate);
        Debug.Log("CircleSize =" + Difficulty.FloCircleSize);
        Debug.Log("OverallDifficulty =" + Difficulty.FloOverallDifficulty);
        Debug.Log("ApproachRate =" + Difficulty.FloApproachRate);
        Debug.Log("SliderMultiplier =" + Difficulty.FloSliderMultiplier);
        Debug.Log("SliderTickRate =" + Difficulty.FloSliderTickRate);
        //Events
        Debug.Log("Events:");
        for (int i = 0; i < Events.AlEvents.Count; i++)
        {
            string strEventParameter = "";
            for (int j = 0; j < Events.AlEvents[i].Length; j++)
            {
                strEventParameter = strEventParameter + Events.AlEvents[i][j] + " ";
            }
            Debug.Log("Event " + i + " : " + strEventParameter);
        }
        //TimingPoints
        Debug.Log("TimingPoints:");
        for (int i = 0; i < TimingPoints.AlPoints.Count; i++)
        {
            Debug.Log("Point " + i + " :");
            Debug.Log("   IntOffset = " + TimingPoints.AlPoints[i].IntOffset);
            Debug.Log("   FloMillisecondsPerBeat = " + TimingPoints.AlPoints[i].FloMillisecondsPerBeat);
            Debug.Log("   IntMeter = " + TimingPoints.AlPoints[i].IntMeter);
            Debug.Log("   IntSampleSet = " + TimingPoints.AlPoints[i].IntSampleSet);
            Debug.Log("   IntSampleIndex = " + TimingPoints.AlPoints[i].IntSampleIndex);
            Debug.Log("   IntVolume = " + TimingPoints.AlPoints[i].IntVolume);
            Debug.Log("   IntInherited = " + TimingPoints.AlPoints[i].IntInherited);
            Debug.Log("   IntKiaiMode = " + TimingPoints.AlPoints[i].IntKiaiMode);
        }
        //Colours
        Debug.Log("Colours:");
        for (int i = 0; i < Colours.LisCombos.Count; i++)
        {
            Debug.Log("Combo " + i + ": R:" + Colours.LisCombos[i].r + " G:" + Colours.LisCombos[i].g + " B:" + Colours.LisCombos[i].b);
        }
        //HitObjects
        Debug.Log("HitObjects:");
        for (int i = 0; i < HitObjects.LisHitObjects.Count; i++)
        {
            Debug.Log("Hitobject : " + i);
            Debug.Log("Vec2Position : (" + HitObjects.LisHitObjects[i].Vec2Position.x + "," + HitObjects.LisHitObjects[i].Vec2Position.x + ")");
            Debug.Log("IntTime :" + HitObjects.LisHitObjects[i].IntTime);
            Debug.Log("IntUnresolvedType :" + HitObjects.LisHitObjects[i].IntUnresolvedType);
            Debug.Log("IntType :" + HitObjects.LisHitObjects[i].IntType);
            Debug.Log("IntNewCombo :" + HitObjects.LisHitObjects[i].IntNewCombo);
            Debug.Log("IntSkipColor :" + HitObjects.LisHitObjects[i].IntSkipColor);
            Debug.Log("IntUnresolvedHitSound :" + HitObjects.LisHitObjects[i].IntUnresolvedHitSound);
            Debug.Log("IntHitSoundNormal :" + HitObjects.LisHitObjects[i].IntHitSoundNormal);
            Debug.Log("IntHitSoundWhistle :" + HitObjects.LisHitObjects[i].IntHitSoundWhistle);
            Debug.Log("IntHitSoundFinish :" + HitObjects.LisHitObjects[i].IntHitSoundFinish);
            Debug.Log("IntHitSoundClap :" + HitObjects.LisHitObjects[i].IntHitSoundClap);
            Debug.Log("IntSliderType :" + HitObjects.LisHitObjects[i].IntSliderType);
            Debug.Log("SliderPoints : ");
            for (int j = 0; j < HitObjects.LisHitObjects[i].LisSliderPoints.Count; j++)
            {
                Debug.Log("Point " + i + " : (" + HitObjects.LisHitObjects[i].LisSliderPoints[j].x + "," + HitObjects.LisHitObjects[i].LisSliderPoints[j].y + ")");
            }
            Debug.Log("IntSliderRepeat :" + HitObjects.LisHitObjects[i].IntSliderRepeat);
            Debug.Log("FloSliderPixelLength :" + HitObjects.LisHitObjects[i].FloSliderPixelLength);
            Debug.Log("SliderEdgeHitSounds :");
            for (int j = 0; j < HitObjects.LisHitObjects[i].LisSliderEdgeHitSounds.Count; j++)
            {
                Debug.Log("Sounds " + i + " : " + HitObjects.LisHitObjects[i].LisSliderEdgeHitSounds[j]);
            }
            Debug.Log("SliderEdgeAddition :");
            for (int j = 0; j < HitObjects.LisHitObjects[i].LisSliderEdgeAddition.Count; j++)
            {
                Debug.Log("Additiom " + i + " : (" + HitObjects.LisHitObjects[i].LisSliderEdgeAddition[j].IntSampleSet + "," + HitObjects.LisHitObjects[i].LisSliderEdgeAddition[j].IntAdditionSet + ")");
            }
            Debug.Log("IntEndtime :" + HitObjects.LisHitObjects[i].IntEndtime);
            Debug.Log("IntExtraSampleSet :" + HitObjects.LisHitObjects[i].IntExtraSampleSet);
            Debug.Log("IntExtraAdditionSet :" + HitObjects.LisHitObjects[i].IntExtraAdditionSet);
            Debug.Log("IntExtraCustomIndex :" + HitObjects.LisHitObjects[i].IntExtraCustomIndex);
            Debug.Log("IntExtraSampleVolume :" + HitObjects.LisHitObjects[i].IntExtraSampleVolume);
            Debug.Log("StrExtraFilename :" + HitObjects.LisHitObjects[i].StrExtraFilename);
        }

    }
}

public class GeneralClass
{
    public string StrAudioFilename;
    public int IntAudioLeadIn;
    public int IntPreviewTime;
    public int IntCountdown;
    public string StrSampleSet;
    public float FloStackLeniency;
    public int IntMode;
    public int IntLetterboxInBreaks;
    public int IntWidescreenStoryboard;
}

public class EditorClass
{
    public List<int> AlBookmarks = new List<int>();
    public float FloDistanceSpacing;
    public int IntBeatDivisor;
    public int IntGridSize;
    public float FloTimelineZoom;
}

public class MetadataClass
{
    public string StrTitle;
    public string StrTitleUnicode;
    public string StrArtist;
    public string StrArtistUnicode;
    public string StrCreator;
    public string StrVersion;
    public string StrSource;
    public List<string> AlTags = new List<string>();
    public int IntBeatmapID;
    public int IntBeatmapSetID;
}

public class DifficultyClass
{
    public float FloHPDrainRate;
    public float FloCircleSize;
    public float FloOverallDifficulty;
    public float FloApproachRate;
    public float FloSliderMultiplier;
    public float FloSliderTickRate;
}

public class EventsClass
{
    public List<string[]> AlEvents = new List<string[]>();
}

public class TimingPointsClass//整个的时间点列表类，保持和文件一致
{
    public List<TimingPointClass> AlPoints = new List<TimingPointClass>();
}

public class TimingPointClass//单个的一个时间点
{
    public int IntOffset;
    public float FloMillisecondsPerBeat;
    public int IntMeter;
    public int IntSampleSet;
    public int IntSampleIndex;
    public int IntVolume;
    public int IntInherited;
    public int IntKiaiMode;
}

public class ColoursClass//颜色列表
{
    public List<Color> LisCombos = new List<Color>();
}

public class HitObjectsClass
{
    public List<HitObjectClass> LisHitObjects = new List<HitObjectClass>();
}

public class HitObjectClass
{
    public Vector2 Vec2Position = new Vector2();
    public int IntTime;
    public int IntUnresolvedType;
    public int IntType;
    public int IntNewCombo;
    public int IntSkipColor;
    public int IntUnresolvedHitSound;
    public int IntHitSoundNormal;
    public int IntHitSoundWhistle;
    public int IntHitSoundFinish;
    public int IntHitSoundClap;
    public int IntSliderType;
    public List<Vector2> LisSliderPoints = new List<Vector2>();
    public int IntSliderRepeat;
    public float FloSliderPixelLength;
    public List<int> LisSliderEdgeHitSounds = new List<int>();
    public List<EdgeAddition> LisSliderEdgeAddition = new List<EdgeAddition>();
    public int IntEndtime;
    public int IntExtraSampleSet;
    public int IntExtraAdditionSet;
    public int IntExtraCustomIndex;
    public int IntExtraSampleVolume;
    public string StrExtraFilename;
}

public class EdgeAddition
{
    public int IntSampleSet;
    public int IntAdditionSet;
}