using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener 
{
    private const string REMOVE_ADS = "remove_ads";
    private const string OPEN_CITY = "open_city";
    private const string OPEN_MEGAPOLIS = "open_megapolis";

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public static IAPManager Instance;

    public void InitializePurchasing() 
    {
        if (IsInitialized()) return;
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(REMOVE_ADS, ProductType.Consumable);
        builder.AddProduct(OPEN_CITY, ProductType.Consumable);
        builder.AddProduct(OPEN_MEGAPOLIS, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized() 
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyNoAds()
    {
        BuyProductID(REMOVE_ADS);
    }

    public void BuyCityMap()
    {
        BuyProductID(OPEN_CITY);
    }

    public void BuyMegapolisMap()
    {
        BuyProductID(OPEN_MEGAPOLIS);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
    {
        if (String.Equals(args.purchasedProduct.definition.id, REMOVE_ADS, StringComparison.Ordinal)) {
            // Товар был куплен успешно, можно его отдать пользователю
            PlayerPrefs.SetString("NoAds", "Yes");
            Destroy(GameObject.Find("ADS Manager(Clone)"));
            Destroy(GameObject.Find("NoADS"));
        }
        else if(String.Equals(args.purchasedProduct.definition.id, OPEN_CITY, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1000);
            GameObject shopController = GameObject.Find("ShopController");
            shopController.GetComponent<BuyMapForCoins>().BuyNewMap(needCoins: 1000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, OPEN_MEGAPOLIS, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 5000);
            GameObject shopController = GameObject.Find("ShopController");
            shopController.GetComponent<BuyMapForCoins>().BuyNewMap(needCoins: 5000);
        }
        else 
        {
            // Неуспешный платеж
        }
        return PurchaseProcessingResult.Complete;
    }


    private void Awake() 
    {
        TestSingleton();
    }

    void Start() 
    {
        if (m_StoreController == null) InitializePurchasing();
    }

    private void TestSingleton() 
    {
        if (Instance != null) 
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId) 
    {
        if (IsInitialized()) 
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase) {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
        }
        else 
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases() 
    {
        if (!IsInitialized()) 
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) 
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else 
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) 
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error) 
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) 
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}
