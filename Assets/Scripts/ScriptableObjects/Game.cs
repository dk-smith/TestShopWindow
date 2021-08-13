using System;
using System.Collections.Generic;
using UI;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class Game : ScriptableObject
    {
        private static Game _instance;

        [SerializeField] private GameSettings settings;
        [SerializeField] private Canvas canvasPrefab;
        [SerializeField] private List<GameWindow> windows;
        [SerializeField] private Shop shop;
        [SerializeField] private PlayerData playerData;

        private Canvas _mainCanvas;
        
        public void Init()
        {
            _instance = this;
        
            if (!settings)
            {
                settings = CreateInstance<GameSettings>();
            };
        
            if (_mainCanvas == null)
            {
                LoadCanvas();
            }

            shop.Init(settings.shopItems);
            playerData.Init(settings.startMoney);

            ShopWindow.Show();
        }
        
        private void LoadCanvas()
        {
            _mainCanvas = FindObjectOfType<Canvas>();
                
            if (!_mainCanvas && canvasPrefab)
                _mainCanvas = Instantiate(canvasPrefab);
        }
        
        public static T ShowWindow<T>(Action<T> onShow = null) where T : GameWindow
        {
            if (!_instance) return null;
            
            var type = _instance.windows.Find(w => w is T);
        
            if (type == null) return null;
        
            var win =  Instantiate(type, _instance._mainCanvas.transform).GetComponent<T>();
            onShow?.Invoke(win);
        
            return win;
        }

    }
}
