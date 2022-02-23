using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class AdmobNativeAD : MonoBehaviour
{
    public UnifiedNativeAd adNative;
    public bool nativeLoaded = false;
    [SerializeField] GameObject adNativePanel;
	[SerializeField] RawImage adIcon;
	[SerializeField] RawImage adChoices;
	[SerializeField] Text adHeadline;
	[SerializeField] Text adCallToAction;
	[SerializeField] Text adAdvertiser;
    public RawImage BodyImage;
    private static AdmobNativeAD instance;

    

    public static AdmobNativeAD Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake ()
	{
        instance = this;
		adNativePanel.SetActive (false); //hide ad panel
        MobileAds.Initialize("ca-app-pub-1579727795693040~3659700951");
    }

	void Start ()
	{

        RequestNativeAd();
    }
    private void RequestNativeAd()
    {
        AdLoader adLoader = new AdLoader.Builder("ca-app-pub-1579727795693040/7330835730").ForUnifiedNativeAd().Build();
        adLoader.OnUnifiedNativeAdLoaded += this.HandleOnUnifiedNativeAdLoaded;
        adLoader.OnAdFailedToLoad += Adloader_OnAdFailedToLoad;
        adLoader.LoadAd(AdRequestBuild());
    }
    AdRequest AdRequestBuild()
    {
        return new AdRequest.Builder().Build();
    }
    //events

    void Update ()
	{
		if (nativeLoaded) {
            nativeLoaded = false;

            Texture2D iconTexture = this.adNative.GetIconTexture();
            Texture2D iconAdChoices = this.adNative.GetAdChoicesLogoTexture();
            string headline = this.adNative.GetHeadlineText();
            string cta = this.adNative.GetCallToActionText();
            string advertiser = this.adNative.GetAdvertiserText();

            if (this.adNative.GetImageTextures().Count > 0)
            {
                List<Texture2D> goList = this.adNative.GetImageTextures();
                BodyImage.texture = goList[0];
                List<GameObject> list = new List<GameObject>();
                list.Add(BodyImage.gameObject);
                this.adNative.RegisterImageGameObjects(list);
            }



            adIcon.texture = iconTexture;
            adChoices.texture = iconAdChoices;

            adHeadline.text = headline;
            adCallToAction.text = cta;
            adAdvertiser.text = advertiser;


            adNative.RegisterIconImageGameObject(adIcon.gameObject);
            adNative.RegisterAdChoicesLogoGameObject(adChoices.gameObject);
            adNative.RegisterHeadlineTextGameObject(adHeadline.gameObject);
            adNative.RegisterCallToActionGameObject(adCallToAction.gameObject);
            adNative.RegisterAdvertiserTextGameObject(adAdvertiser.gameObject);

            adNativePanel.SetActive (true); //show ad panel
		}
	}


    private void HandleOnUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs args)
    {
        AdmobNativeAD.Instance.adNative = args.nativeAd;
        AdmobNativeAD.Instance.nativeLoaded = true;

    }
    private void Adloader_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.Log("Native ad failed to load" + e.Message);
    }

}

