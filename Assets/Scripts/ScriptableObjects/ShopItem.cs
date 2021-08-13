using System;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public class ShopItem
    {
        [Serializable]
        public enum Type
        {
            Image,
            String,
            ImageString,
            TwoImages,
            TwoStrings,
            Money
        }
    
        public Sprite[] Sprites = new Sprite[2];

        public string[] Strings = new string[2];

        public int Money;

        public int Price;
    
        public Type ItemType;

        ShopItem()
        {
            Sprites = new Sprite[2];
            Strings = new string[2];
        }

    }
}
