using UnityEngine;
using UnityEditor;
using System;

namespace AIR.Fluxity.Editor
{
    internal class TwoPanelUtility
    {
        private Vector2 _leftPanelScrollViewPos;
        private Vector2 _rightPanelScrollViewPos;
        private Action _sidePanelDrawer;
        private Action _contentDrawers;
        private int _leftPanelRightWall;

        public TwoPanelUtility(
            int leftPanelRightWall,
            Action sidePanelDrawer,
            Action contentDrawers)
        {
            _leftPanelRightWall = leftPanelRightWall;
            _sidePanelDrawer = sidePanelDrawer;
            _contentDrawers = contentDrawers;
        }

        public void OnGui(Rect position)
        {
            DoSidePanel(position);
            //todo add movable divider
            DoContentPanel(position);
        }

        private void DoContentPanel(Rect position)
        {
            GUILayout.BeginArea(new Rect(_leftPanelRightWall, 0, position.width - _leftPanelRightWall, position.height));
            {
                _rightPanelScrollViewPos = EditorGUILayout.BeginScrollView(
                    _rightPanelScrollViewPos,
                    GUILayout.Height(position.height));
                {
                    _contentDrawers?.Invoke();
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private void DoSidePanel(Rect position)
        {
            GUILayout.BeginArea(new Rect(0, 0, _leftPanelRightWall, position.height));
            {
                _leftPanelScrollViewPos = EditorGUILayout.BeginScrollView(
                _leftPanelScrollViewPos,
                    GUILayout.Height(position.height),
                    GUILayout.Width(_leftPanelRightWall));
                {
                    _sidePanelDrawer?.Invoke();
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
    }
}