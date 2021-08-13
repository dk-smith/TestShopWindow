using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class ShopWindow : GameWindow
    {
        [SerializeField] private RectTransform table;
        [SerializeField] private ShopItemView rowPrefab;
        [SerializeField] private Shop shop;

        private List<ShopItemView> _views = new List<ShopItemView>(); 
        
        public static ShopWindow Show()
        {
            return Game.ShowWindow<ShopWindow>(w => w.Init());
        }

        public void Init()
        {
            Draw();
        }

        private void Draw()
        {
            var items = shop.Items;
            
            if (items == null) return;

            for (int i = 0; i < items.Count; i++)
            {
                var newRow = Instantiate(rowPrefab, table);
                newRow.Init(i + 1, items[i], OnBuy);
                _views.Add(newRow);
            }
        }

        private void UpdateViews()
        {
            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].UpdateNum(i + 1);
            }
        }

        private void OnBuy(ShopItemView itemView)
        {
            shop.BuyItem(itemView.ShopItem, OnComplete);

            void OnComplete()
            {
                _views.Remove(itemView);
                Destroy(itemView.gameObject);
                UpdateViews();
            }
        }
    }
}