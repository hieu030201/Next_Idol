using System;
using System.Collections.Generic;
using Advertising;
using Gley.EasyIAP;
using UnityEngine;

namespace Adverstising_Integration.Scripts
{
    public class IAPManager : Singleton<IAPManager>
    {
        [SerializeField] private bool isRemoveAds;

        private ShopProductNames _restoreProductName;
        private event Action OnProductPurchasedEvent;
        private event Action OnRestorePurchasesEvent;

        #region Unity Methods

        private void Start()
        {
            // Debug.Log("super yun iap start: " + IAPManager.Instance);
            
            API.Initialize(OnInitializationComplete);
        }

        #endregion

        #region Public Methods

        public void BuyProduct(ShopProductNames productName, Action onPurchaseCompleted)
        {
            OnProductPurchasedEvent = onPurchaseCompleted;
            if (isRemoveAds)
            {
                OnProductPurchasedEvent?.Invoke();
                return;
            }

            API.BuyProduct(productName, ProductBought);
        }

#if UNITY_IOS
        public void RestorePurchases(ShopProductNames productName, Action onRestored = null)
        {
            _restoreProductName = productName;
            OnRestorePurchasesEvent = onRestored;
            API.RestorePurchases(ProductRestored);
        }
#endif

        public int GetPrice(ShopProductNames productName) => API.GetValue(productName);

        public string GetLocalizedPrice(ShopProductNames productName) => API.GetLocalizedPriceString(productName);

        public string GetCurrencyCode(ShopProductNames productName)
        {
            return API.GetIsoCurrencyCode(productName);
        }

        #endregion

        #region Private Methods

        private void OnInitializationComplete(IAPOperationStatus status, string message,
            List<StoreProduct> shopProducts)
        {
            if (status == IAPOperationStatus.Success)
            {
                // Debug.Log("super yun OnInitializationComplete: " + IAPManager.IsInstanceExisted());
                Debug.Log("IAP is initialized!");
            }
            else
            {
                Debug.Log("Error occurred: " + message);
                // Debug.Log("super yun iap init fail");
            }
        }

        private void ProductBought(IAPOperationStatus status, string message, StoreProduct product)
        {
            if (status == IAPOperationStatus.Success)
            {
                OnProductPurchasedEvent?.Invoke();
            }
            else
            {
                Debug.Log("Error occurred: " + message);
            }
        }

        // For IOS only
        private void ProductRestored(IAPOperationStatus status, string message, StoreProduct product)
        {
            if (status == IAPOperationStatus.Success)
            {
                if (product.productName == _restoreProductName.ToString())
                {
                    OnRestorePurchasesEvent?.Invoke();
                }
            }
            else
            {
                Debug.Log("Error occurred: " + message);
            }
        }

        #endregion
    }
}