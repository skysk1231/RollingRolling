using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{
    private static AdManager _instance;  // �̱��� �ν��Ͻ�
    public static AdManager Instance { get { return _instance; } }

    private InterstitialAd interstitialAd;  // ���� ���� ��ü
    private BannerView bannerView;  // ��� ���� ��ü
    private bool isInterstitialAdReady = false;  // ���� ���� �غ� ����

    private int adDisplayCount = 0;  // ���� ǥ�� Ƚ��
    private const int maxAdsBeforeDisplay = 10;  // ������ ǥ�õ� �ִ� Ƚ��

    void Awake()
    {
        // �̱��� ���� ����
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);  // �� ��ȯ �ÿ��� ����
        }
        else
        {
            Destroy(gameObject);  // �̹� �ν��Ͻ��� �����ϸ� ��ü�� �ı�
        }
    }

    void Start()
    {
        // Google Mobile Ads SDK �ʱ�ȭ
        MobileAds.Initialize(OnMobileAdsInitialized);

        // �� �ε� �� ���� ��û ó��
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnMobileAdsInitialized(InitializationStatus status)
    {
        // ���� ������ ���� ���� �̸� �ε�
        LoadInterstitialAd();
        // ��� ���� ��û
        RequestBannerAd();
    }

    // ��� ������ ��û�ϴ� �Լ�
    private void RequestBannerAd()
    {
        string adUnitId = "ca-app-pub-3706732444529605/6654901048";  // ��� ���� ID

        // 올바른 AdSize 객체 생성
        AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        // 여기서 AdSize → adSize 로 수정!
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest adRequest = new AdRequest();
        bannerView.LoadAd(adRequest);
    }

    // ��� ���� ǥ�� �Լ�
    public void ShowBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Show();  // ��� ���� ǥ��
            Debug.Log("Banner ad is showing.");
        }
    }

    // ���� ������ ��û�ϴ� �Լ�
    private void LoadInterstitialAd()
    {
        string adUnitId = "ca-app-pub-3706732444529605/8619306137";  // ���� ���� ID
        AdRequest adRequest = new AdRequest();  // ���� ��û ��ü ����

        // ���� ���� �ε� ��û
        InterstitialAd.Load(adUnitId, adRequest, HandleInterstitialAdLoaded);
    }

    // ���� ������ �ε�Ǿ��� �� ȣ��Ǵ� �ݹ� �Լ�
    private void HandleInterstitialAdLoaded(InterstitialAd ad, LoadAdError error)
    {
        if (error != null || ad == null)
        {
            Debug.LogError("Interstitial ad failed to load: " + error?.GetMessage());
            return;
        }

        // ���� ��ü �Ҵ�
        interstitialAd = ad;
        isInterstitialAdReady = true;  // ������ �غ�� ���·� ǥ�õ� �� ����

        // ������ ������ �� ȣ��� �ݹ� �Լ� ����
        interstitialAd.OnAdFullScreenContentClosed += HandleAdClosed;
    }

    // ���� ���� ǥ��
    public void ShowInterstitialAd()
    {
        // ������ �غ�Ǿ��� ���� ǥ��
        if (isInterstitialAdReady && adDisplayCount >= maxAdsBeforeDisplay)
        {
            interstitialAd.Show();
            adDisplayCount = 0;  // ���� ǥ�� �� ī��Ʈ ����
            Debug.Log("Interstitial ad is showing.");
        }
        else
        {
            // ���� ǥ�� Ƚ���� ������ �� �̸��� �� ���� �ε� �� ī��Ʈ ����
            adDisplayCount++;
            Debug.Log("Interstitial ad is not shown yet. Count: " + adDisplayCount);
        }
    }

    // ���� ������ ������ �� ȣ��Ǵ� �Լ�
    private void HandleAdClosed()
    {
        // ���� ���� ��ü �޸� ����
        interstitialAd.Destroy();
        interstitialAd = null;
        isInterstitialAdReady = false;  // ������ ������ �غ� ���� �ʱ�ȭ

        // ���ο� ���� ���� �ε�
        LoadInterstitialAd();
    }

    // �� �ε� �� ������ ǥ���ϰų� �ε��ϴ� �Լ�
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� ���� ���� �ε�Ǿ��� �� ���� ���� ǥ��
        if (scene.name == "GameOver" && isInterstitialAdReady)
        {
            ShowInterstitialAd();  // ������ ��� ǥ��
        }
    }

    // Unity ������Ʈ�� �ı��� �� ��� �� ���� ���� ��ü ����
    private void OnDestroy()
    {
        // ��� ������ ���� ���� ��ü �޸� ����
        bannerView?.Destroy();
        interstitialAd?.Destroy();
    }
}
