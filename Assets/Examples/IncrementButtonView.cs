using System;
using UnityEngine;
using UnityEngine.UI;

public class IncrementButtonView : MonoBehaviour
{
    private Action onClicked;

    [SerializeField] private Button _button;
    [SerializeField] private Text _text;

    public void SetOnClickedCallback(Action onClicked)
        => this.onClicked = onClicked;
    public void SetButtonText(string text)
        => _text.text = text;

    public void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        onClicked.Invoke();
    }
}
