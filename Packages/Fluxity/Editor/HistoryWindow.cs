using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SettingsManagement;
using System.IO;

namespace AIR.Fluxity.Editor
{
    internal class HistoryWindow : FluxityRuntimeEditorWindow
    {
        [UserSetting("General Settings", "Command History Length")]
        static UserSetting<int> CommandHistoryLength = new UserSetting<int>(FluxityEditorSettings.Instance, $"general.{nameof(CommandHistoryLength)}", 30, SettingsScope.User);
        [UserSetting("General Settings", "Log History")]
        static UserSetting<bool> CommandHistoryLogToFile = new UserSetting<bool>(FluxityEditorSettings.Instance, $"general.{nameof(CommandHistoryLogToFile)}", false, SettingsScope.User);

        private const string CommandDispatchedTimeFormat = "HH:mm:ss.fff";
        private const string SessionStartedTimeFormat = "yyyy-MM-dd_HH-mm-ss";
        private Queue<DispatchData> _recentDispatchHistory;
        private SearchBoxUtility _searchBox;
        private TwoPanelUtility _twoPanel;
        private string _commandHistoryFileName;

        [MenuItem("Window/Fluxity/Runtime History")]
        public static void ShowWindow()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<HistoryWindow>();
            window.titleContent = new GUIContent("Fluxity Runtime History");
            window.Show();
        }

        public void OnGUI()
        {
            if (_twoPanel == null)
            {
                _twoPanel = new TwoPanelUtility(
                    220,
                    DoCommandRecentHistory,
                    () => { });
            }

            _twoPanel.OnGui(position);
        }

        public override void OnEnable()
        {
            _recentDispatchHistory = new Queue<DispatchData>(CommandHistoryLength);
            base.OnEnable();
            TrySubscribe();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            TryUnsubscribe();
        }

        protected override void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            Refresh();
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                var fileChangeTime = DateTime.Now;
                _commandHistoryFileName = Application.dataPath + "/../Logs/fluxitycommandhistory_" + fileChangeTime.ToString(SessionStartedTimeFormat) + ".txt";
                TrySubscribe();
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                TryUnsubscribe();
            }

            Repaint();
        }

        private void TrySubscribe()
        {
            var dispatcher = GetDispatcher();
            if (dispatcher == null) return;
            dispatcher.OnDispatch += OnReceivedDispatch;
        }

        private void TryUnsubscribe()
        {
            var dispatcher = GetDispatcher();
            if (dispatcher == null) return;
            dispatcher.OnDispatch -= OnReceivedDispatch;
        }

        private void DoCommandRecentHistory()
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Flush History"))
                {
                    Flush();
                }

                if (_searchBox == null)
                    _searchBox = new SearchBoxUtility();

                _searchBox.OnGui();
            }
            GUILayout.EndHorizontal();

            DrawHistoryRows();
        }

        private void DrawHistoryRows()
        {
            var oldWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80;
            foreach (var dispatch in _recentDispatchHistory)
            {
                var dateStr = dispatch.TimeStamp.ToString(CommandDispatchedTimeFormat);
                var dispatchStr = dispatch.Dispatch;
                var filter = _searchBox?.CurrentSearchText ?? string.Empty;

                if (dateStr.Contains(filter, StringComparison.OrdinalIgnoreCase)
                    || dispatchStr.Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    EditorGUILayout.LabelField(dateStr, dispatchStr);
                }
            }
            EditorGUIUtility.labelWidth = oldWidth;
        }

        private void OnReceivedDispatch(ICommand dispatchedCommand)
        {
            var now = DateTime.Now;
            UpdateRecentDispatchHistory(dispatchedCommand.GetType().Name, now);
        }

        private void UpdateRecentDispatchHistory(string dispatchMessage, DateTime at)
        {
            if (CommandHistoryLogToFile)
                File.AppendAllTextAsync(_commandHistoryFileName, $"{at.ToString(CommandDispatchedTimeFormat)} - {dispatchMessage}\n");

            if (_recentDispatchHistory.Count >= CommandHistoryLength)
                _recentDispatchHistory.Dequeue();

            _recentDispatchHistory.Enqueue(new DispatchData { Dispatch = dispatchMessage, TimeStamp = at});
            Repaint();
        }

        private void Flush()
        {
            _recentDispatchHistory.Clear();
            Repaint();
        }

        private struct DispatchData
        {
            public string Dispatch;
            public DateTime TimeStamp;
        }
    }
}