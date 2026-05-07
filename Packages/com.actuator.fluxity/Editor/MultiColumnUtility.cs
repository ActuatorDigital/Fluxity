using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.IMGUI.Controls;
using System.Linq;

namespace AIR.Fluxity.Editor
{
    //Invaluable help from https://gamedev.stackexchange.com/questions/188771/creating-a-custom-editor-window-using-a-multi-column-header
    internal class MultiColumnUtility
    {
        private readonly Color _lighterColor = Color.white * 0.3f;
        private readonly Color _darkerColor = Color.white * 0.1f;
        private MultiColumnHeaderState _multiColumnHeaderState;
        private MultiColumnHeader _multiColumnHeader;
        private Vector2 _scrollPosition;

        public void Initialise(
            string[] columnNames,
            Action<int, bool> sortDataSetHandler)
        {
            var columns = columnNames
                .Select(x => new MultiColumnHeaderState.Column { headerContent = new GUIContent(x) })
                .ToArray();
            _multiColumnHeaderState = new MultiColumnHeaderState(columns: columns);
            _multiColumnHeader = new MultiColumnHeader(state: _multiColumnHeaderState);
            _multiColumnHeader.visibleColumnsChanged += (multiColumnHeader) => multiColumnHeader.ResizeToFit();
            _multiColumnHeader.sortingChanged += (multiColumnHeader) =>
            {
                var columnIndex = multiColumnHeader.sortedColumnIndex;
                var isAscending = multiColumnHeader.IsSortedAscending(columnIndex);
                sortDataSetHandler(columnIndex, isAscending);
            };
            _multiColumnHeader.ResizeToFit();
        }

        public void OnGui(
            Rect windowRect,
            int itemCount,
            Func<int, bool> ShouldDrawItem,
            Func<int, int, string> StringForItem)
        {
            var columnHeight = EditorGUIUtility.singleLineHeight;

            var columnRectPrototype = new Rect(windowRect)
            {
                height = columnHeight,
            };

            var viewRect = new Rect(source: windowRect)
            {
                width = _multiColumnHeaderState.widthOfAllVisibleColumns,
                height = columnHeight * (itemCount + 1),
            };

            _scrollPosition = GUI.BeginScrollView(
                windowRect,
                scrollPosition: _scrollPosition,
                viewRect,
                true,
                true
            );
            {
                _multiColumnHeader.OnGUI(columnRectPrototype, 0.0f);
                var rowRect = new Rect(columnRectPrototype);
                int drawnCount = 0;

                for (int i = 0; i < itemCount; i++)
                {
                    if (!ShouldDrawItem(i))
                        continue;

                    rowRect.y += columnHeight;

                    EditorGUI.DrawRect(rowRect, drawnCount % 2 == 0 ? _darkerColor : _lighterColor);
                    drawnCount++;

                    for (int columnIndex = 0; columnIndex < _multiColumnHeaderState.columns.Length; columnIndex++)
                    {
                        if (!_multiColumnHeader.IsColumnVisible(columnIndex)) continue;

                        var visibleColumnIndex = _multiColumnHeader.GetVisibleColumnIndex(columnIndex);
                        var strToShow = StringForItem(i, columnIndex);

                        var columnRect = _multiColumnHeader.GetColumnRect(visibleColumnIndex);
                        columnRect.y = rowRect.y;
                        var cellRect = _multiColumnHeader.GetCellRect(visibleColumnIndex, columnRect);
                        EditorGUI.LabelField(cellRect, strToShow);
                    }
                }
            }
            GUI.EndScrollView(true);
        }

        internal bool IsColumnVisible(int columnIndex)
            => _multiColumnHeader.IsColumnVisible(columnIndex);
    }
}