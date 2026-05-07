using System;
using UnityEditor.IMGUI.Controls;

namespace AIR.Fluxity.Editor
{
    internal class SearchBoxUtility
    {
        public string CurrentSearchText { get; private set; } = string.Empty;
        private readonly SearchField _searchField = new SearchField();

        public event Action<string> OnSearchTextChanged;

        public void OnGui()
        {
            var prevSearchText = CurrentSearchText;
            CurrentSearchText = _searchField.OnGUI(CurrentSearchText);
            if (CurrentSearchText != prevSearchText)
                OnSearchTextChanged?.Invoke(CurrentSearchText);
        }
    }
}