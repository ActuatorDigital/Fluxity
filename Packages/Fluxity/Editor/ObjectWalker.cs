using UnityEditor;
using System.Collections;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

namespace AIR.Fluxity.Editor
{
    internal class ObjectWalker
    {
        private const int MaxLevel = 15;
        private const string CyclicReferenceName = "CYCLIC";
        private const int AutoFoldOutIndent = 2;
        private List<object> _cache = new List<object>();
        private Dictionary<object, bool> _foldoutStatus = new Dictionary<object, bool>();

        public void Prepare()
        {
            _cache.Clear();
        }

        public void DrawObject(object obj, int curLevel)
        {
            if (_cache.Contains(obj))
            {
                Print(CyclicReferenceName, obj, curLevel);
                return;
            }

            _cache.Add(obj);

            if (curLevel > MaxLevel)
            {
                Debug.Log($"{nameof(ObjectWalker)} reached '{MaxLevel}', when {nameof(DrawObject)} '{obj}'.");
                return;
            }

            var flags = BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public;
            //move to an object, track foldouts, determine if something can be folded out by checking for getenumerator
            var fields = obj.GetType().GetFields(flags);
            foreach (var field in fields)
            {
                var value = field.GetValue(obj);
                DoNameAndValue(field.Name, value, curLevel);
            }
            var props = obj.GetType().GetProperties(flags);
            foreach (var prop in props)
            {
                if (prop.GetIndexParameters().Length == 0)
                {
                    var value = prop.GetValue(obj);
                    DoNameAndValue(prop.Name, value, curLevel);
                }
            }
        }

        private void DoNameAndValue(string name, object value, int curLevel)
        {
            if (value == null)
            {
                Print(name, null, curLevel);
            }
            else if (IsLeafType(value))
            {
                Print(name, value, curLevel);
            }
            else if (value is IDictionary dictionary)
            {
                var currentlyFoldedOut = DealWithFoldout(name, curLevel, dictionary, dictionary.Count.ToString());

                if (currentlyFoldedOut)
                {
                    foreach (DictionaryEntry item in dictionary)
                    {
                        DoNameAndValue(item.Key.ToString(), item.Value, curLevel + 1);
                    }
                }
            }
            else if (value is ICollection collection)
            {
                var currentlyFoldedOut = DealWithFoldout(name, curLevel, collection, collection.Count.ToString());

                if (currentlyFoldedOut)
                {
                    var key = 0;
                    foreach (var item in collection)
                    {
                        DoNameAndValue(key.ToString(), item, curLevel + 1);
                        key++;
                    }
                }
            }
            else
            {
                var currentlyFoldedOut = DealWithFoldout(name, curLevel, value, value.GetType().Name);
                if (currentlyFoldedOut)
                {
                    DrawObject(value, curLevel + 1);
                }
            }
        }

        private bool DealWithFoldout(string name, int curLevel, object obj, string info)
        {
            var currentlyFoldedOut = curLevel < AutoFoldOutIndent;
            if (_foldoutStatus.TryGetValue(obj, out var val))
                currentlyFoldedOut = val;

            currentlyFoldedOut = PrintCollectionFoldout(name, info, curLevel, currentlyFoldedOut);
            _foldoutStatus[obj] = currentlyFoldedOut;
            return currentlyFoldedOut;
        }

        private bool PrintCollectionFoldout(string name, string info, int curLevel, bool currentlyFoldedOut)
        {
            EditorGUI.indentLevel = curLevel;
            return EditorGUILayout.Foldout(currentlyFoldedOut, $"{name}({info})", true);
        }

        private void Print(string prefix, object val, int curLevel)
        {
            EditorGUI.indentLevel = curLevel;
            if (val is UnityEngine.Object unityObject)
                EditorGUILayout.ObjectField(prefix, unityObject, unityObject.GetType(), true);
            else
                EditorGUILayout.LabelField(prefix, val?.ToString() ?? "null");
        }

        private bool IsLeafType(object obj)
        {
            if (obj.GetType().IsPrimitive
                || obj.GetType().IsEnum)
                return true;

            switch (obj)
            {
            case string:
            case decimal:
            case Vector2:
            case Vector3:
            case Vector4:
            case System.Guid:
            case System.DateTime:
            case System.TimeSpan:
            case UnityEngine.Object:
                return true;
            default:
                return false;
            }
        }
    }
}