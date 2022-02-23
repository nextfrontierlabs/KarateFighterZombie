using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class IAP : MonoBehaviour, IStoreListener
{
	private static IAP instance;
	public static IAP Instance
	{
		get
		{
			return instance;
		}
	}

	void Awake()
	{
		instance = this;
	}




	public string [] gemsPrices;


	public string [] goldPrices;


	public string [] packs;
	public string starterPack;


	private static IStoreController m_StoreController; // Reference to the Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // Reference to store-specific Purchasing

	//private static string kItems = "items_all"; // General handle for the consumable product.

	//private static string kGooglePlayItems = "com.emaago.zombiemassacre.items_all"; // Google Play Store identifier for the consumable product.
	public MonoBehaviour _Main;

	[Serializable]
	public struct StoreItem
	{
		//public string productName ;
		public string productID ;
		public string iOSID ;
		public ProductType type ;
	}

	public StoreItem [] inAppProducts;


	// Just for starter Pack;
	string currentProductID;
	void Start()
	{
		
		//ZPlayerPrefs.Initialize("----------------", SystemInfo.deviceUniqueIdentifier);
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null)
		{
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}

//		if (Application.platform == RuntimePlatform.IPhonePlayer)
//			restoreButton.SetActive (true);
//		else
//			restoreButton.SetActive (false);
	}
	
	public void InitializePurchasing()
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

//		// Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
//		builder.AddProduct(kItems, ProductType.NonConsumable, new IDs(){ {kGooglePlayItems,  GooglePlay.Name} });// Continue adding the non-consumable product.
		foreach (StoreItem sI in inAppProducts) 
		{
			#if UNITY_IPHONE
				builder.AddProduct (sI.iOSID, sI.type);// Continue adding the non-consumable product.
			#else
				builder.AddProduct (sI.productID, sI.type);// Continue adding the non-consumable product.
			#endif
		}

		UnityPurchasing.Initialize(this, builder);
	}


	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;

	}

//	public void BuyNonConsumable(string kItems)
//	{
//		// Buy the non-consumable product using its general identifier. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
//		BuyProductID(kItems);
//	}


	public void BuyProductID(string productId)
	{
		currentProductID = productId;
		Debug.Log (productId);
		#if UNITY_IPHONE
		productId = productId.Replace ("thegreatarmy","thegloriousresolve");
		#endif
		// If the stores throw an unexpected exception, use try..catch to protect my logic here.
		try
		{
			// If Purchasing has been initialized ...
			if (IsInitialized())
			{
				// ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
				Product product = m_StoreController.products.WithID(productId);

				// If the look up found a product for this device's store and that product is ready to be sold ...
				if (product != null && product.availableToPurchase)
				{
					//if(!currentProductID.Equals("com.rockville.games.starterpack.kokf")|| !currentProductID.Equals("com.rockville.games.unlock.all") || !currentProductID.Equals("com.rockville.games.unlock.shots.moves") || !currentProductID.Equals("com.rockville.games.unlock.fighters") || !currentProductID.Equals("com.rockville.games.remove.ads"))
					//InAppManager.GetInstance().loading.SetActive (true);
					Debug.Log (string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
					m_StoreController.InitiatePurchase(product);
				}
				// Otherwise ...
				else
				{
					// ... report the product look-up failure situation

					Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
					FailPurchase ("Product Unavailable!");


				}


			}
			// Otherwise ...
			else
			{
				// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
				Debug.Log("BuyProductID FAIL. Not initialized.");
				FailPurchase ("Initialization Error!");

			}
		}
		// Complete the unexpected exception handling ...
		catch (Exception e)
		{
			// ... by reporting any unexpected exception for later diagnosis.
			Debug.Log ("BuyProductID: FAIL. Exception during purchase. " + e);
			FailPurchase ("Purchase Failed!");

		}
	}


	// Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
	public void RestorePurchases()
	{
		// If Purchasing has not yet been set up ...
		if (!IsInitialized())
		{
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ...
		if (Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			// ... begin restoring purchases
			Debug.Log("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		// Otherwise ...
		else
		{
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}


	//
	// --- IStoreListener
	//

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;


		#if UNITY_IPHONE

		gemsPrices[0] = m_StoreController.products.WithID ("com.rockville.games.gems.small.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.gems.small.kokf").metadata.isoCurrencyCode;
		gemsPrices[1] = m_StoreController.products.WithID ("com.rockville.games.gems.medium.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.gems.medium.kokf").metadata.isoCurrencyCode;
		gemsPrices[2] = m_StoreController.products.WithID ("com.rockville.games.gems.large.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.gems.large.kokf").metadata.isoCurrencyCode;
		gemsPrices[3] = m_StoreController.products.WithID ("com.rockville.games.gems.xl.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.gems.xl.kokf").metadata.isoCurrencyCode;
		gemsPrices[4] = m_StoreController.products.WithID ("com.rockville.games.gems.xxl.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.gems.xxl.kokf").metadata.isoCurrencyCode;

		goldPrices[0] = m_StoreController.products.WithID ("com.rockville.games.gold.small.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.gold.small.kokf").metadata.isoCurrencyCode;
		goldPrices[1] = m_StoreController.products.WithID ("com.rockville.games.gold.medium.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.gold.medium.kokf").metadata.isoCurrencyCode;
		goldPrices[2] = m_StoreController.products.WithID ("com.rockville.games.gold.large.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.gold.large.kokf").metadata.isoCurrencyCode;
		goldPrices[3] = m_StoreController.products.WithID ("com.rockville.games.gold.xl.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.gold.xl.kokf").metadata.isoCurrencyCode;
		goldPrices[4] = m_StoreController.products.WithID ("com.rockville.games.gold.xxl.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.gold.xxl.kokf").metadata.isoCurrencyCode;

		packs[0] = m_StoreController.products.WithID ("com.rockville.games.package.one.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.package.one.kokf").metadata.isoCurrencyCode;
		packs[1] = m_StoreController.products.WithID ("com.rockville.games.package.two.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.package.two.kokf").metadata.isoCurrencyCode;
		packs[2] = m_StoreController.products.WithID ("com.rockville.games.package.three.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.package.three.kokf").metadata.isoCurrencyCode;


		starterPack = m_StoreController.products.WithID ("com.rockville.games.starterpack.kokf").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.rockville.games.starterpack.kokf").metadata.isoCurrencyCode;
	#else
		gemsPrices[0] = m_StoreController.products.WithID ("com.onestep.games.gems.small.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.gems.small.loz").metadata.isoCurrencyCode;
		gemsPrices[1] = m_StoreController.products.WithID ("com.onestep.games.gems.medium.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.gems.medium.loz").metadata.isoCurrencyCode;
		gemsPrices[2] = m_StoreController.products.WithID ("com.onestep.games.gems.large.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.gems.large.loz").metadata.isoCurrencyCode;
		gemsPrices[3] = m_StoreController.products.WithID ("com.onestep.games.gems.xl.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.gems.xl.loz").metadata.isoCurrencyCode;
		gemsPrices[4] = m_StoreController.products.WithID ("com.onestep.games.gems.xxl.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.gems.xxl.loz").metadata.isoCurrencyCode;

		goldPrices[0] = m_StoreController.products.WithID ("com.onestep.games.gold.small.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.gold.small.loz").metadata.isoCurrencyCode;
		goldPrices[1] = m_StoreController.products.WithID ("com.onestep.games.gold.medium.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.gold.medium.loz").metadata.isoCurrencyCode;
		goldPrices[2] = m_StoreController.products.WithID ("com.onestep.games.gold.large.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.gold.large.loz").metadata.isoCurrencyCode;
		goldPrices[3] = m_StoreController.products.WithID ("com.onestep.games.gold.xl.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.gold.xl.loz").metadata.isoCurrencyCode;
		goldPrices[4] = m_StoreController.products.WithID ("com.onestep.games.gold.xxl.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.gold.xxl.loz").metadata.isoCurrencyCode;

		packs[0] = m_StoreController.products.WithID ("com.onestep.games.package.one.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.package.one.loz").metadata.isoCurrencyCode;
		packs[1] = m_StoreController.products.WithID ("com.onestep.games.package.two.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.package.two.loz").metadata.isoCurrencyCode;
		packs[2] = m_StoreController.products.WithID ("com.onestep.games.package.three.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.package.three.loz").metadata.isoCurrencyCode;


		starterPack = m_StoreController.products.WithID ("com.onestep.games.starterpack.loz").metadata.localizedPrice.ToString()+"  "+m_StoreController.products.WithID ("com.onestep.games.starterpack.loz").metadata.isoCurrencyCode;

		#endif

	}
	void UpdateSuccess()
	{
		if (InAppManager.GetInstance().currencyItem == InAppManager.CurrencyToBuy.gems) {
			int gems = EconomyController.GetInstance ().GetGems ()+ InAppManager.GetInstance().currentRewardGem;
			EconomyController.GetInstance ().SetGems (gems);
			CurrencyPopUpManager.GetInstance ().ShowPop (InAppManager.GetInstance().currentRewardGem,0);

		} else if (InAppManager.GetInstance().currencyItem == InAppManager.CurrencyToBuy.gold) {
			int gold = EconomyController.GetInstance ().GetGold ()+ InAppManager.GetInstance().currentRewardGold;
			EconomyController.GetInstance ().SetGold (gold);
			CurrencyPopUpManager.GetInstance ().ShowPop (0,InAppManager.GetInstance().currentRewardGold);
		}
		else if (InAppManager.GetInstance().currencyItem == InAppManager.CurrencyToBuy.package) {
			int gold = EconomyController.GetInstance ().GetGold ()+ InAppManager.GetInstance().currentRewardGold;
			EconomyController.GetInstance ().SetGold (gold);
			int gems = EconomyController.GetInstance ().GetGems ()+ InAppManager.GetInstance().currentRewardGem;
			EconomyController.GetInstance ().SetGems (gems);
			CurrencyPopUpManager.GetInstance ().ShowPop (InAppManager.GetInstance().currentRewardGem,InAppManager.GetInstance().currentRewardGold);
		}
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{

		bool validPurchase = true; // Presume valid for platforms with no R.V.

		// Unity IAP's validation logic is only included on these platforms.
		#if UNITY_ANDROID || UNITY_STANDALONE_OSX
		// Prepare the validator with the secrets we prepared in the Editor
		// obfuscation window.
//		var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),AppleTangle.Data(), Application.identifier);
//
//		try 
//		{
//			// On Google Play, result has a single product ID.
//			// On Apple stores, receipts contain multiple products.
//			var result = validator.Validate(args.purchasedProduct.receipt);
//			// For informational purposes, we list the receipt(s)
//			Debug.Log("Receipt is valid. Contents:");
//			foreach (IPurchaseReceipt productReceipt in result) 
//			{
//				Debug.Log(productReceipt.productID);
//				Debug.Log(productReceipt.purchaseDate);
//				Debug.Log(productReceipt.transactionID);
//			}
//		} catch (IAPSecurityException) 
//		{
//			Debug.Log("Invalid receipt, not unlocking content");
//			validPurchase = false;
//		}
		#endif
         
		#if UNITY_EDITOR || UNITY_IOS
		validPurchase = true;
		#endif

		if (validPurchase) 
		{
			// Unlock the appropriate content here.
			Debug.Log ("PURCHASE VALID");
			if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.gems.small.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
	else if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.gems.medium.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
			else if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.gems.large.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
			else if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.gems.xl.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
			else if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.gems.xxl.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
			if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.gold.small.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
			else if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.gold.medium.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
			else if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.gold.large.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
			else if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.gold.xl.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
			else if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.gold.xxl.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
			else if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.package.one.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();

				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
			else if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.package.two.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);
			}
			else if (
				String.Equals (args.purchasedProduct.definition.id, "com.onestep.games.package.three.loz", StringComparison.Ordinal)

			) 
			{
				UpdateSuccess ();


				PlayerPrefs.Save ();
				InAppManager.GetInstance().loading.SetActive (false);

			}
			else if (String.Equals(args.purchasedProduct.definition.id, "com.onestep.games.starterpack.loz", StringComparison.Ordinal)
				
			)
            {
				DefaultMainMenuScreen.GetInstance ().LoaderPanel.SetActive (false);

				DefaultMainMenuScreen.GetInstance().starterPackBtn.SetActive(false);
				DefaultMainMenuScreen.GetInstance().starterPackPanel.SetActive(false);

				int gold = EconomyController.GetInstance ().GetGold ()+ 10000;
				EconomyController.GetInstance ().SetGold (gold);
				int gems = EconomyController.GetInstance ().GetGems ()+ 100;
				EconomyController.GetInstance ().SetGems (gems);
				CurrencyPopUpManager.GetInstance ().ShowPop (100,10000);
               
     
                PlayerPrefs.Save();

            }
			else if (String.Equals(args.purchasedProduct.definition.id, "com.onestep.games.remove.ads", StringComparison.Ordinal)

			)
			{
				DefaultMainMenuScreen.GetInstance().LoaderPanel.SetActive(false);

				PlayerPrefs.SetInt("RemoveAds",1);
				CurrencyPopUpManager.GetInstance().ShowPop("Ads Removed Successfully");

				PlayerPrefs.Save();
				InAppRandomOffersController.DoOnClosePanels();
				ShopManager.DoOnRemoveAdsBought();
			}
			else if (String.Equals(args.purchasedProduct.definition.id, "com.onestep.games.unlock.all", StringComparison.Ordinal)

			)
			{
				DefaultMainMenuScreen.GetInstance().LoaderPanel.SetActive(false);

				PlayerPrefs.SetInt("com.onestep.games.unlock.all", 1);
				CurrencyPopUpManager.GetInstance().ShowPop("Purchased Successfully");
				SetAllPlayersBought();
				SetAllPowersBought();
				PlayerPrefs.SetInt("RemoveAds", 1);
				PlayerPrefs.Save();
				InAppRandomOffersController.DoOnClosePanels();
				ShopManager.DoOnRemoveAdsBought();
			}
			
			else {
                starterPackDialg.SetActive(false);
				FailPurchase ("Incorrect Purchase!");
				Debug.Log (string.Format ("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));

			}
		}
		else{
			FailPurchase ("Validation Error!");
			Debug.Log ("Receipt not validated from server");

		}


		// Return a flag indicating wither this product has completely been received, or if the application needs to be reminded of this purchase at next app launch. Is useful when saving purchased products to the cloud, and when that save is delayed.
		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",product.definition.storeSpecificId, failureReason));
		Debug.Log (product.definition.id);

		FailPurchase ();

	}


	public void PurchaseItemButton (string productID)
	{

			BuyProductID (productID);

	}

	public GameObject IAPMenuObj;
	public GameObject loadingObj;
	public GameObject restoreButton;

    public GameObject starterPackDialg, starterPackBtn;
	public GameObject purchaseSuccessfulObj;
	public GameObject purchaseFailedObj;

	public Text purchaseFailedText;

	public Text cashLabel,medalLabel, grenadeLabel, armorLabel;

	public GameObject adsPurchased;
	public GameObject missionsPurchased;

	void FailPurchase(string msg = "Purchase Failed!")
	{
		InAppRandomOffersController.DoOnClosePanels();
		if (currentProductID.Equals ("com.rockville.games.starterpack.kokf") || currentProductID.Equals("com.rockville.games.remove.ads") || currentProductID.Equals("com.rockville.games.unlock.all") || currentProductID.Equals("com.rockville.games.unlock.shots.moves") || currentProductID.Equals("com.rockville.games.unlock.fighters")) {
			DefaultMainMenuScreen.GetInstance ().FailedPanel.SetActive (true);
			DefaultMainMenuScreen.GetInstance ().FailMessage.text = msg;
			DefaultMainMenuScreen.GetInstance ().LoaderPanel.SetActive (false);
 
		} else {
			InAppManager.GetInstance ().failedPanel.SetActive (true);

			InAppManager.GetInstance ().failedMessage.text = msg;
			InAppManager.GetInstance ().loading.SetActive (false);

		}
	}
	private void SetAllPlayersBought()
    {
        for (int i = 0; i < ShopManager.GetInstance().characterNames.Length; i++)
        {
			PlayerPrefs.SetInt("Characters Bought" + (i + 1), 1);
		}
    }
	private void SetAllPowersBought()
	{
		for (int i = 0; i < ShopManager.GetInstance().powerBtns	.Length; i++)
		{
			PlayerPrefs.SetInt("Power Move" + (i + 1), 1);
		}
		for (int i = 0; i < ShopManager.GetInstance().specialMovesBtns.Length; i++)
		{
			PlayerPrefs.SetInt("Special Move Bought" + (i + 1), 1);
		}
	}

	public void ShowIAPMenu()
	{
//				#if UNITY_IPHONE || UNITY_ANDROID
//				AdsManagerRG.Instance.AdmobShowBanner ();
//				#endif
		IAPMenuObj.SetActive (true);


		InAppManager.GetInstance().loading.SetActive (false);
		InAppManager.GetInstance ().failedPanel.SetActive (false);
		purchaseSuccessfulObj.SetActive (false);

		if(PlayerPrefs.GetInt ("MissionsCompletedLegendary",0)>6 )
		{
			missionsPurchased.SetActive (true);
		}
	else 
			missionsPurchased.SetActive (false);
		
		if(PlayerPrefs.GetInt ("NoAds",0) == 1)
		{
			adsPurchased.SetActive (true);
		}
		else 
			adsPurchased.SetActive (false);
		


	}

	public void HideIAPMenu()
	{
		#if UNITY_IPHONE || UNITY_ANDROID
   
		#endif
		IAPMenuObj.SetActive (false);

	}


}