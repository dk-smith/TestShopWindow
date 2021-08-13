using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI numText;
        [SerializeField] private Image[] images;
        [SerializeField] private TextMeshProUGUI[] texts;
        [SerializeField] private Button buyButton;

        public ShopItem ShopItem { get; private set; }
        private Action<ShopItemView> _onBuy;

        public void Init(int index, ShopItem shopItem, Action<ShopItemView> onBuy = null)
        {
            ShopItem = shopItem;
            _onBuy = onBuy;
            Draw();
            UpdateNum(index);
        }

        private void Draw()
        {
            for (int i = 0; i < images.Length; i++)
            {
                if (ShopItem.Sprites.Length > i && ShopItem.Sprites[i] is Sprite sprite && sprite != null)
                {
                    images[i].gameObject.SetActive(true);
                    images[i].sprite = sprite;
                }
                else
                    images[i].gameObject.SetActive(false);
            }
        
            for (int i = 0; i < texts.Length; i++)
            {
                if (ShopItem.Strings.Length > i && ShopItem.Strings[i] is string str)
                {
                    texts[i].gameObject.SetActive(true);
                    texts[i].text = str;
                }
                else
                    texts[i].gameObject.SetActive(false);
            }

            if (ShopItem.ItemType == ShopItem.Type.Money)
            {
                texts[0].gameObject.SetActive(true);
                texts[0].text = $"{ShopItem.Money}$";
            }

            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = ShopItem.Price > 0 ? $"{ShopItem.Price}$" : "FREE";
        
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => _onBuy?.Invoke(this));
        }

        public void UpdateNum(int index)
        {
            numText.text = $"{index}";
        }
    
    }
}