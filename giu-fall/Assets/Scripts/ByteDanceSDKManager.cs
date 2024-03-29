﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarkSDKSpace;
using System;

public class ByteDanceSDKManager
{
    private static ByteDanceSDKManager instance;

    private float startTime;

    private float lastPlayTime;

    private bool initialized = false;

    public Action onShareResult;

    public static ByteDanceSDKManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ByteDanceSDKManager();
            return instance;
        }
    }
    public void Initialize()
    {
        if (initialized) return;
        if (Debug.isDebugBuild)
        {
            MockSetting.OpenAllMockModule();
        }
        startTime = Time.time;

        lastPlayTime = 0;

        StarkAdManager.IsShowLoadAdToast = false;

        initialized = true;
    }
    #region record&share
    public void StartRecord()
    {
        StarkSDK.API.GetStarkGameRecorder().StartRecord();
    }

    public void StopRecord()
    {
        StarkSDK.API.GetStarkGameRecorder().StopRecord();
        int duration = StarkSDK.API.GetStarkGameRecorder().GetRecordDuration() / 1000;
    }

    void ShareReward()
    {
        if (onShareResult != null)
        {
            onShareResult();
            onShareResult = null;
        }
    }

    public void Share()
    {
        int duration = StarkSDK.API.GetStarkGameRecorder().GetRecordDuration()/1000;
        if (duration > 3 && duration < 600 || true)
        {
            
            //share topic
            List<string> topics = new List<string>();
            topics.Add("永不言弃掉落");
            StarkSDK.API.GetStarkGameRecorder().ShareVideoWithTitleTopics(ShareReward, null, null, "", topics);
        }
    }

    #endregion

    #region ads
    public void ShowRewardVideo()
    {
        StarkSDKSpace.StarkSDK.API.GetStarkAdManager().ShowVideoAdWithId("312digmjeab8f5oe1o", VideoReward);
        return;
    }

    public void VideoReward(bool complete)
    {
        if (complete)
        {
            Debug.Log("完成");
            PluginMercury.Instance.AdShowSuccessCallBack("PlayComplete");
        }
    }

    public void ShowInterstitial()
    {
        float curTime = Time.time;
        if (curTime - startTime < 15 || (lastPlayTime != 0 && curTime - lastPlayTime < 30))
        {
            return;
        }
        StarkSDKSpace.StarkSDK.API.GetStarkAdManager().CreateInterstitialAd("88fadne60m4kkkkehr");
        lastPlayTime = curTime;
    }

    public void ShowBanner()
    {
        StarkAdManager.BannerStyle banner = new StarkAdManager.BannerStyle();
        banner.left = 0;
        banner.top = 300;
        banner.width = 500;
        StarkSDK.API.GetStarkAdManager().CreateBannerAd("5hk0bjlmg58917kh29", banner);
    }
    #endregion

    #region other functions
    public void SaveToDesktop()
    {
        StarkSDK.API.CreateShortcut(null);
    }

    public void FollowDouyin()
    {
        StarkSDK.API.FollowDouYinUserProfile(null,null);
    }
    #endregion
}
