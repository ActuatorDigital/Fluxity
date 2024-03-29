﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SettingsManagement;

namespace AIR.Fluxity.Editor
{
    internal sealed class HistoryWindow : FluxityRuntimeEditorWindow
    {
        [UserSetting("General Settings", "Command History Length")]
        static UserSetting<int> CommandHistoryLength = new UserSetting<int>(FluxityEditorSettings.Instance, $"general.{nameof(CommandHistoryLength)}", 30, SettingsScope.User);
        [UserSetting("General Settings", "Log History")]
        static UserSetting<bool> CommandHistoryLogToFile = new UserSetting<bool>(FluxityEditorSettings.Instance, $"general.{nameof(CommandHistoryLogToFile)}", false, SettingsScope.User);

        private const string CommandDispatchedTimeFormat = "HH:mm:ss.fff";
        private const string SessionStartedTimeFormat = "yyyy-MM-dd_HH-mm-ss";
        private const int LeftPanelRightWall = 280;
        
        private Queue<DispatchData> _recentDispatchHistory;
        private SearchBoxUtility _searchBox;
        private TwoPanelUtility _twoPanel;
        private CommandDispatchedLogger _commandDispatchedLogger;
        private DispatchData _selectedElement;
        private CommandDrawer _selectedDrawer;

        private string LogsPath => Application.dataPath + "/../Logs/";

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
                    LeftPanelRightWall,
                    DoCommandRecentHistory,
                    DoShowStore);
            }

            _twoPanel.OnGui(position);
        }

        public override void AddItemsToMenu(GenericMenu menu)
        {
            base.AddItemsToMenu(menu);
            GUIContent content = new GUIContent("Open Logs folder");
            menu.AddItem(content, false, OpenLogFolder);
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
                if (_commandDispatchedLogger == null)
                    CreateCommandLogger();
                
                TrySubscribe();
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                TryUnsubscribe();
                _commandDispatchedLogger?.Dispose();
                _commandDispatchedLogger = null;
            }

            Repaint();
        }

        private void CreateCommandLogger()
        {
            var time = DateTime.Now;
            _commandDispatchedLogger = new CommandDispatchedLogger( LogsPath + "fluxitycommandhistory_" + time.ToString(SessionStartedTimeFormat) + ".txt");
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
            foreach (var dispatchData in _recentDispatchHistory)
            {
                var dateStr = dispatchData.TimeStamp.ToString(CommandDispatchedTimeFormat);
                var dispatchStr = dispatchData.DispatchName;
                var filter = _searchBox?.CurrentSearchText ?? string.Empty;

                if (dateStr.Contains(filter, StringComparison.OrdinalIgnoreCase)
                    || dispatchStr.Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    var style = dispatchData == _selectedElement
                            ? EditorStyles.selectionRect
                            : EditorStyles.label;

                    EditorGUILayout.LabelField(
                        dateStr,
                        dispatchStr,
                        style,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    
                    UpdateSelection(dispatchData);
                }
            }
            EditorGUIUtility.labelWidth = oldWidth;
        }

        private void UpdateSelection(DispatchData dispatchData)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                var lastRect = GUILayoutUtility.GetLastRect();
                var max = lastRect.max;
                max.x = LeftPanelRightWall;
                lastRect.max = max;

                if (lastRect.Contains(Event.current.mousePosition))
                {
                    _selectedElement = dispatchData;
                    _selectedDrawer = new CommandDrawer(_selectedElement.DispatchCommand);
                    
                    Repaint();
                }
            }
        }

        private void OnReceivedDispatch(ICommand dispatchedCommand)
        {
            var dat = new DispatchData
            {
                DispatchName = dispatchedCommand.GetType().Name,
                TimeStamp = DateTime.Now,
                DispatchCommand = dispatchedCommand,
            };
            UpdateRecentDispatchHistory(dat);
        }

        private void UpdateRecentDispatchHistory(DispatchData dat)
        {
            if (CommandHistoryLogToFile)
            {
                if (_commandDispatchedLogger == null)
                    CreateCommandLogger();

                _commandDispatchedLogger.Log(dat);
            }

            if (_recentDispatchHistory.Count >= CommandHistoryLength)
            {
                _recentDispatchHistory.Dequeue();
            }

            _recentDispatchHistory.Enqueue(dat);
            Repaint();
        }

        private void DoShowStore()
        {
            if (_selectedDrawer == null)
            {
                EditorGUILayout.LabelField("No selection");
            }
            else
            {
                EditorGUILayout.LabelField($"Values of {_selectedElement.DispatchName} @ {_selectedElement.TimeStamp.ToString(CommandDispatchedTimeFormat)}", EditorStyles.boldLabel);
                EditorWindowUtil.DrawHorLine(Color.grey);

                _selectedDrawer.Draw();
            }
        }

        private void Flush()
        {
            _recentDispatchHistory.Clear();
            Repaint();
        }

        private void OpenLogFolder()
        {
            Application.OpenURL(LogsPath);
        }
    }

    internal class DispatchData
    {
        public string DispatchName;
        public DateTime TimeStamp;
        public ICommand DispatchCommand;
    }
}