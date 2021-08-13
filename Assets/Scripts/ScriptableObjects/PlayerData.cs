using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private int money;

        public event Action OnMoneyChange; 

        public int Money
        {
            get => money;
            set
            {
                var val = Mathf.Max(value, 0);
                if (val != money)
                {
                    money = val;
                    OnMoneyChange?.Invoke();
                }
            }
        }

        public bool HasMoney(int amount) => money > amount;

        public void Init(int newMoney)
        {
            Money = newMoney;
        }
    }
}