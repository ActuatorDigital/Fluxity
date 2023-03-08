using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AIR.Fluxity.Editor
{
    class HistoryWindow : FluxityRuntimeEditorWindow
    {
        private const int RECENT_CAPACITY = 30;
        private const string TIME_FORMAT = "HH:mm:ss.fff";
        private readonly Queue<DispatchData> _recentDispatchHistory = new Queue<DispatchData>(RECENT_CAPACITY);
        private Vector2 _panelScrollViewPos;
        private bool _subscribed = false;
        private IDispatcher _dispatcher;

        [MenuItem("Window/Fluxity/Runtime History")]
        public static void ShowWindow()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<HistoryWindow>();
            window.titleContent = new GUIContent("Fluxity Runtime History");
            window.Show();
        }

        public void Update()
        {
            if (!EditorApplication.isPlaying && _subscribed)
            {
                _subscribed = false;
            }

            if (EditorApplication.isPlaying && !_subscribed)
            {
                _dispatcher = GetDispatcher();
                if (_dispatcher == null) return;
                _dispatcher.OnDispatch += OnReceivedDispatch;
                _subscribed = true;
            }
        }

        public void OnGUI()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                GUILayout.Label("Not in play mode.");
            }
            else
            {
                if (_subscribed)
                    DoCommandRecentHistory();
            }
        }

        private void DoCommandRecentHistory()
        {
            GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));
            {
                if (GUILayout.Button("Flush History"))
                {
                    Flush();
                }

                _panelScrollViewPos = EditorGUILayout.BeginScrollView(
                    _panelScrollViewPos,
                    GUILayout.Height(position.height),
                    GUILayout.Width(position.width));
                {
                    foreach (var dispatch in _recentDispatchHistory)
                    {
                        DoPaintDispatchEntry(dispatch);
                    }
                }

                EditorGUILayout.EndScrollView();
            }

            GUILayout.EndArea();
        }

        private void DoPaintDispatchEntry(DispatchData dispatch)
        {
            EditorGUILayout.LabelField(dispatch.TimeStamp.ToString(TIME_FORMAT), dispatch.Dispatch);
        }

        private void OnReceivedDispatch(ICommand dispatchedCommand)
        {
            UpdateRecentDispatchHistory(dispatchedCommand.GetType().Name);
        }

        private void UpdateRecentDispatchHistory(string dispatchMessage)
        {
            if (_recentDispatchHistory.Count >= RECENT_CAPACITY)
                _recentDispatchHistory.Dequeue();

            _recentDispatchHistory.Enqueue(new DispatchData { Dispatch = dispatchMessage, TimeStamp = DateTime.Now });
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