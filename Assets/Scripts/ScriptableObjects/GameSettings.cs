using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        public int startMoney;
        public List<ShopItem> shopItems;
    }
}