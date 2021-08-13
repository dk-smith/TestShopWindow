using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyDisplay;
        [SerializeField] private PlayerData playerData;

        public void Start()
        {
            if (playerData == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            playerData.OnMoneyChange += OnMoneyChange;
            OnMoneyChange();
        }

        private void OnMoneyChange()
        {
            moneyDisplay.text = $"{playerData.Money}$";
        }

        private void OnDestroy()
        {
            if (playerData != null)
                playerData.OnMoneyChange -= OnMoneyChange;
        }
    }
}