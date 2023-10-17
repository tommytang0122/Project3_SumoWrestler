﻿/*--------------------------------------------------------------------------------*
  Copyright (C)Nintendo All rights reserved.

  These coded instructions, statements, and computer programs contain proprietary
  information of Nintendo and/or its licensed developers and are protected by
  national and international copyright laws. They may not be disclosed to third
  parties or copied or duplicated in any form, in whole or in part, without the
  prior written consent of Nintendo.

  The content herein is highly confidential and should be handled accordingly.
 *--------------------------------------------------------------------------------*/

using UnityEngine;
using nn.hid;

public class HidVibrationSimple : MonoBehaviour
{
    private UnityEngine.UI.Text textComponent;
    private System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

    private NpadId npadId = NpadId.Invalid;
    private NpadStyle npadStyle = NpadStyle.Invalid;
    private NpadState npadState = new NpadState();

    private const int vibrationDeviceCountMax = 2;
    private int vibrationDeviceCount = 0;
    private VibrationDeviceHandle[] vibrationDeviceHandles = new VibrationDeviceHandle[vibrationDeviceCountMax];
    private VibrationDeviceInfo[] vibrationDeviceInfos = new VibrationDeviceInfo[vibrationDeviceCountMax];
    private VibrationValue vibrationValue = VibrationValue.Make();

    private byte[] fileA;
    private byte[] fileB;
    private VibrationFileInfo fileInfoA = new VibrationFileInfo();
    private VibrationFileInfo fileInfoB = new VibrationFileInfo();
    private VibrationFileParserContext fileContextA = new VibrationFileParserContext();
    private VibrationFileParserContext fileContextB = new VibrationFileParserContext();
    private int sampleA;
    private int sampleB;

    void Start()
    {
        textComponent = GameObject.Find("/Canvas/Text").GetComponent<UnityEngine.UI.Text>();

        nn.Result result;
        Npad.Initialize();

        Npad.SetSupportedStyleSet(NpadStyle.Handheld | NpadStyle.JoyDual | NpadStyle.FullKey);
        NpadId[] npadIds = { NpadId.Handheld, NpadId.No1 };
        Npad.SetSupportedIdType(npadIds);

        fileA = Resources.Load<TextAsset>("SampleA.bnvib").bytes;
        result = VibrationFile.Parse(ref fileInfoA, ref fileContextA, fileA, fileA.LongLength);
        Debug.Assert(result.IsSuccess());
        Debug.Log("SampleA.bnvib: " + fileInfoA);

        fileB = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/SampleB.bnvib");

        result = VibrationFile.Parse(ref fileInfoB, ref fileContextB, fileB, fileB.LongLength);
        Debug.Assert(result.IsSuccess());
        Debug.Log("SampleB.bnvib: " + fileInfoB);
    }

    void Update()
    {
        stringBuilder.Length = 0;
        stringBuilder.Append("A: amplitudeLow random\nB: amplitudeHigh random\nX: Play Resources/SampleA.bnvib\nY: Play StreamingAssets/SampleB.bnvib\n\n");

        if (UpdatePadState())
        {
            stringBuilder.AppendFormat("{0} {1} {2}\n", npadId, npadStyle, npadState);

            vibrationValue.Clear();

            if ((npadState.buttons & NpadButton.A) != 0)
            {
                vibrationValue.amplitudeLow = Random.Range(0f, 1f);
                vibrationValue.amplitudeHigh = 0f;
            }

            if ((npadState.buttons & NpadButton.B) != 0)
            {
                vibrationValue.amplitudeLow = 0f;
                vibrationValue.amplitudeHigh = Random.Range(0f, 1f);
            }

            if ((npadState.buttons & NpadButton.X) != 0)
            {
                VibrationFile.RetrieveValue(ref vibrationValue, sampleA, ref fileContextA);
                if ((sampleA % 10) == 0) { sampleA++; } // 200 sample / sec
                sampleA += 3;
                if (sampleA >= fileInfoA.sampleLength) { sampleA = 0; }
            }
            else
            {
                sampleA = 0;
            }

            if ((npadState.buttons & NpadButton.Y) != 0)
            {
                VibrationFile.RetrieveValue(ref vibrationValue, sampleB, ref fileContextB);
                if ((sampleB % 10) == 0) { sampleB++; } // 200 sample / sec
                sampleB += 3;
                if (sampleB >= fileInfoB.sampleLength) { sampleB = 0; }
            }
            else
            {
                sampleB = 0;
            }

            for (int i = 0; i < vibrationDeviceCount; i++)
            {
                Vibration.SendValue(vibrationDeviceHandles[i], vibrationValue);
                stringBuilder.AppendFormat("VibrationDevice{0} {1}: {2}\n",
                    i, vibrationDeviceInfos[i], vibrationValue);
            }
        }

        textComponent.text = stringBuilder.ToString();
    }

    private bool UpdatePadState()
    {
        NpadStyle handheldStyle = Npad.GetStyleSet(NpadId.Handheld);
        NpadState handheldState = npadState;
        if (handheldStyle != NpadStyle.None)
        {
            Npad.GetState(ref handheldState, NpadId.Handheld, handheldStyle);
            if (handheldState.buttons != NpadButton.None)
            {
                if ((npadId != NpadId.Handheld) || (npadStyle != handheldStyle))
                {
                    this.GetVibrationDevice(NpadId.Handheld, handheldStyle);
                }
                npadId = NpadId.Handheld;
                npadStyle = handheldStyle;
                npadState = handheldState;
                return true;
            }
        }

        NpadStyle no1Style = Npad.GetStyleSet(NpadId.No1);
        NpadState no1State = npadState;
        if (no1Style != NpadStyle.None)
        {
            Npad.GetState(ref no1State, NpadId.No1, no1Style);
            if (no1State.buttons != NpadButton.None)
            {
                if ((npadId != NpadId.No1) || (npadStyle != no1Style))
                {
                    this.GetVibrationDevice(NpadId.No1, no1Style);
                }
                npadId = NpadId.No1;
                npadStyle = no1Style;
                npadState = no1State;
                return true;
            }
        }

        if ((npadId == NpadId.Handheld) && (handheldStyle != NpadStyle.None))
        {
            npadId = NpadId.Handheld;
            npadStyle = handheldStyle;
            npadState = handheldState;
        }
        else if ((npadId == NpadId.No1) && (no1Style != NpadStyle.None))
        {
            npadId = NpadId.No1;
            npadStyle = no1Style;
            npadState = no1State;
        }
        else
        {
            npadId = NpadId.Invalid;
            npadStyle = NpadStyle.Invalid;
            npadState.Clear();
            return false;
        }
        return true;
    }

    private void GetVibrationDevice(NpadId id, NpadStyle style)
    {
        vibrationValue.Clear();
        for (int i = 0; i < vibrationDeviceCount; i++)
        {
            Vibration.SendValue(vibrationDeviceHandles[i], vibrationValue);
        }

        vibrationDeviceCount = Vibration.GetDeviceHandles(
            vibrationDeviceHandles, vibrationDeviceCountMax, id, style);

        for (int i = 0; i < vibrationDeviceCount; i++)
        {
            Vibration.InitializeDevice(vibrationDeviceHandles[i]);
            Vibration.GetDeviceInfo(ref vibrationDeviceInfos[i], vibrationDeviceHandles[i]);
        }
    }
}