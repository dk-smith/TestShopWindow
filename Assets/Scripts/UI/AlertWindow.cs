using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AlertWindow : GameWindow
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button okButton;
        [SerializeField] private Button confirmButton;
        
        public static AlertWindow Show(string text, string okLabel = "OK", Action onConfirm = null, string confirmLabel = "Confirm")
        {
            return Game.ShowWindow<AlertWindow>(w => w.Init(text, okLabel, onConfirm, confirmLabel));
        }

        public void Init(string msg, string okLabel = "OK", Action onConfirm = null, string confirmLabel = "Confirm")
        {
            text.text = msg;
            okButton.GetComponentInChildren<TextMeshProUGUI>().text = okLabel;
            okButton.onClick.AddListener(Close);

            if (onConfirm != null)
            {
                confirmButton.gameObject.SetActive(true);
                confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = confirmLabel;
                confirmButton.onClick.AddListener(() =>
                {
                    Close();
                    onConfirm?.Invoke();
                });
            }
        }
        
    }
}