using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class Shop : ScriptableObject
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private List<ShopItem> shopItems = new List<ShopItem>();

        public List<ShopItem> Items => shopItems;

        public void Init(List<ShopItem> items)
        {
            if (items != null)
                shopItems = items.ToList();
        }
        
        public void BuyItem(ShopItem item, Action onComplete = null)
        {
            if (playerData.HasMoney(item.Price))
            {
                if (item.Price > 0)
                {
                    AlertWindow.Show($"Do you really want buy this items for {item.Price}$?", "Cancel", OnOk, "Buy");
                } 
                else OnOk();
            }
            else
            {
                AlertWindow.Show($"You don't have enough money to buy this item for {item.Price}$");
            }

            void OnOk()
            {
                CompleteBuy(item);
                onComplete?.Invoke();
            }
        }

        private void CompleteBuy(ShopItem shopItem)
        {
            if (shopItems.Contains(shopItem) && playerData.HasMoney(shopItem.Price))
            {
                playerData.Money -= shopItem.Price;
                
                if (shopItem.Money > 0)
                {
                    playerData.Money += shopItem.Money;
                }
                
                shopItems.Remove(shopItem);
            };
        }
    }
}