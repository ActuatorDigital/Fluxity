﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AIR.Fluxity.Editor
{
    internal class HistoryWindow : FluxityRuntimeEditorWindow
    {
        private const int RECENT_CAPACITY = 30;
        private const string TIME_FORMAT = "HH:mm:ss.fff";
        private readonly Queue<DispatchData> _recentDispatchHistory = new Queue<DispatchData>(RECENT_CAPACITY);
        private Vector2 _panelScrollViewPos;

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
            DoCommandRecentHistory();
        }

        public override void OnEnable()
        {
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
            GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));
            {
                _panelScrollViewPos = EditorGUILayout.BeginScrollView(
                    _panelScrollViewPos,
                    GUILayout.Height(position.height),
                    GUILayout.Width(position.width));
                {
                    if (GUILayout.Button("Flush History"))
                    {
                        Flush();
                    }

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