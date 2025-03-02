using GoogleMobileAds.Api;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AdManager : MonoBehaviour
{
    //public TextMeshProUGUI totalCoinsTxt;

    public string appId = "ca-app-pub-3940256099942544~3347511713";// "ca-app-pub-3940256099942544~3347511713";//Real app id

#if UNITY_ANDROID
    string bannerId = "ca-app-pub-3940256099942544/6300978111";//test banner ads id
    string interId = "ca-app-pub-3940256099942544/1033173712";//test interstitial id
    string rewardedId = "ca-app-pub-3940256099942544/5224354917";//test rewarded id
    //string nativeId = "ca-app-pub-3940256099942544/2247696110";

#elif UNITY_IPHONE
    string bannerId = "ca-app-pub-3940256099942544/2934735716";
    string interId = "ca-app-pub-3940256099942544/4411468910";
    string rewardedId = "ca-app-pub-3940256099942544/1712485313";
    string nativeId = "ca-app-pub-3940256099942544/3986624511";

#endif

    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;

    private PlayerController playerController;
    //NativeAd nativeAd;


    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => {

            print("Ads Initialised !!");

        });

        LoadInterstitialAd();
        LoadRewardedAd();
    }

    //#region Banner

    //public void LoadBannerAd()
    //{
    //    //create a banner
    //    CreateBannerView();

    //    //listen to banner events
    //    ListenToBannerEvents();

    //    //load the banner
    //    if (bannerView == null)
    //    {
    //        CreateBannerView();
    //    }

    //    var adRequest = new AdRequest();
    //    adRequest.Keywords.Add("unity-admob-sample");

    //    print("Loading banner Ad !!");
    //    bannerView.LoadAd(adRequest);//show the banner on the screen
    //}
    //void CreateBannerView()
    //{

    //    if (bannerView != null)
    //    {
    //        DestroyBannerAd();
    //    }
    //    bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);
    //}
    //void ListenToBannerEvents()
    //{
    //    bannerView.OnBannerAdLoaded += () =>
    //    {
    //        Debug.Log("Banner view loaded an ad with response : "
    //            + bannerView.GetResponseInfo());
    //    };
    //    // Raised when an ad fails to load into the banner view.
    //    bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
    //    {
    //        Debug.LogError("Banner view failed to load an ad with error : "
    //            + error);
    //    };
    //    // Raised when the ad is estimated to have earned money.
    //    bannerView.OnAdPaid += (AdValue adValue) =>
    //    {
    //        Debug.Log("Banner view paid {0} {1}." +
    //            adValue.Value +
    //            adValue.CurrencyCode);
    //    };
    //    // Raised when an impression is recorded for an ad.
    //    bannerView.OnAdImpressionRecorded += () =>
    //    {
    //        Debug.Log("Banner view recorded an impression.");
    //    };
    //    // Raised when a click is recorded for an ad.
    //    bannerView.OnAdClicked += () =>
    //    {
    //        Debug.Log("Banner view was clicked.");
    //    };
    //    // Raised when an ad opened full screen content.
    //    bannerView.OnAdFullScreenContentOpened += () =>
    //    {
    //        Debug.Log("Banner view full screen content opened.");
    //    };
    //    // Raised when the ad closed full screen content.
    //    bannerView.OnAdFullScreenContentClosed += () =>
    //    {
    //        Debug.Log("Banner view full screen content closed.");
    //    };
    //}
    //public void DestroyBannerAd()
    //{

    //    if (bannerView != null)
    //    {
    //        print("Destroying banner Ad");
    //        bannerView.Destroy();
    //        bannerView = null;
    //    }
    //}
    //#endregion

    #region Interstitial

    public void LoadInterstitialAd()
    {

        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(interId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Interstitial ad failed to load" + error);
                return;
            }

            print("Interstitial ad loaded !!" + ad.GetResponseInfo());

            interstitialAd = ad;
            InterstitialEvent(interstitialAd);
        });

    }
    public void ShowInterstitialAd()
    {

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            print("Intersititial ad not ready!!");
        }
    }
    public void InterstitialEvent(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Interstitial ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            interstitialAd.Destroy();
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }


    #endregion

    #region Rewarded

    public void LoadRewardedAd()
    {

        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Rewarded failed to load" + error);
                return;
            }

            print("Rewarded ad loaded !!");
            rewardedAd = ad;
            RewardedAdEvents(rewardedAd);
            RegisterReloadHandler(rewardedAd);
        });
    }
    public void ShowRewardedAd()
    {

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                print("Give reward to player !!");

                playerController.StartGame();

            });
        }
        else
        {
            //IF this is the case then Restart level.
            print("Rewarded ad not ready");
            RegisterReloadHandler(rewardedAd);
        }
    }
    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
            ad.Destroy();
            RegisterReloadHandler(ad);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            ad.Destroy();
            RegisterReloadHandler(ad);
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            ad.Destroy();
            RegisterReloadHandler(ad);
        };
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }

    #endregion

    /*
        #region Native

        public Image img;

        public void RequestNativeAd()
        {

            AdLoader adLoader = new AdLoader.Builder(nativeId).ForNativeAd().Build();

            adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
            adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;

            adLoader.LoadAd(new AdRequest.Builder().Build());
        }

        private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e)
        {
            print("Native ad loaded");
            this.nativeAd = e.nativeAd;

            Texture2D iconTexture = this.nativeAd.GetIconTexture();
            Sprite sprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.one * .5f);

            img.sprite = sprite;

        }

        private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            print("Native ad failed to load" + e.ToString());

        }
        #endregion
    */

    #region extra 

    //void GrantCoins(int coins)
    //{
    //    int crrCoins = PlayerPrefs.GetInt("totalCoins");
    //    crrCoins += coins;
    //    PlayerPrefs.SetInt("totalCoins", crrCoins);

    //    ShowCoins();
    //}
    //void ShowCoins()
    //{
    //    totalCoinsTxt.text = PlayerPrefs.GetInt("totalCoins").ToString();
    //}

    #endregion


}
