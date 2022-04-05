using System;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.Shared
{
    public class ButtonView : MonoBehaviour
    {
        private Action _onClicked;

        [SerializeField]
        private Button uButton;

        [SerializeField]
        private Text uText;

        public void SetOnClickedCallback(Action onClicked)
            => _onClicked = onClicked;
        public void SetButtonText(string text)
            => uText.text = text;

        public void Start()
        {
            uButton.onClick.AddListener(OnClick);
        }

        public void OnDestroy()
        {
            uButton.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _onClicked?.Invoke();
        }
    }
}