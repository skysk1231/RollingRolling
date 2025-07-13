using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewardAdController : MonoBehaviour
{
    private RewardedAd rewardedAd;
    private int pendingUnlockSkinIndex = -1;

    void Start()
    {
        MobileAds.Initialize(initStatus => {
            Debug.Log("AdMob initialized.");
            LoadAd();
        });
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ���
    }
    public void LoadAd()
    {
        string adUnitId = "ca-app-pub-3940256099942544/5224354917"; // �׽�Ʈ�� ������ ���� ID

        AdRequest adRequest = new AdRequest(); // SDK 7.x ���: Builder ����

        RewardedAd.Load(adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load: " + error?.GetMessage());
                return;
            }

            rewardedAd = ad;
            Debug.Log("Rewarded ad loaded successfully.");

            // ���� ��üȭ�� ���� ����
            rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
            {
                Debug.LogError("Ad failed to show full screen content: " + adError?.GetMessage());
            };

            // ���� ������ �ٽ� �ε�
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad closed. Reloading...");
                LoadAd();

                SetImmersiveMode();
            };
        });
    }

    public void ShowAd(int skinIndexToUnlock)
    {
        if (rewardedAd == null)
        {
            Debug.LogWarning("rewardedAd is null.");
        }
        else if (!rewardedAd.CanShowAd())
        {
            Debug.LogWarning("rewardedAd cannot show ad.");
        }
        else
        {
            Debug.Log("Showing ad...");
            // ���� ��û ����
        }
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            pendingUnlockSkinIndex = skinIndexToUnlock;


            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"Reward received: {reward.Type}, amount: {reward.Amount}");

                if (pendingUnlockSkinIndex != -1)
                {
                    PlayerPrefs.SetInt($"SkinUnlocked_{pendingUnlockSkinIndex}", 1);
                    PlayerPrefs.Save();
                    Debug.Log($"Skin {pendingUnlockSkinIndex} unlocked!");

                    
                    if (SkinManager.Instance != null)
                    {
                        SkinManager.Instance.RefreshSkinUIState(); 
                    }
                    
                    pendingUnlockSkinIndex = -1;
                }
            });


        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
        }
    }
    private void SetImmersiveMode()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow"))
        using (AndroidJavaObject decorView = window.Call<AndroidJavaObject>("getDecorView"))
        {
            int flags =
                0x00000400 | // View.SYSTEM_UI_FLAG_HIDE_NAVIGATION
                0x00000004 | // View.SYSTEM_UI_FLAG_FULLSCREEN
                0x00001000;  // View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY

            decorView.Call("setSystemUiVisibility", flags);
        }
#endif
    }
}
