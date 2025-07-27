using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public static class Common
{
    public static List<(Type, string)> GetDerivedTypes<T>() where T : ScriptableObject
    {
        return Assembly.GetAssembly(typeof(T))
            .GetTypes()
            .Where(t => !t.IsAbstract && t.IsClass && typeof(T).IsAssignableFrom(t))
            .Select(t => (t, RegexReplacePascal(t.Name)))
            .ToList();
    }
    
    public static string RegexReplacePascal(this string input)
    {
        return string.Join(" ", Regex.Split(input, @"(?<!^)(?=[A-Z])"));
    }
    
    public static void DrawItemList<T>(string listTitle, List<(T Item1, Editor Item2)> list, Action<int> drawItem, ref bool foldout)
        where T : ScriptableObject
    {

        int? removeIndex = null;
        foldout = EditorGUILayout.Foldout(foldout, listTitle);

        if(!foldout)
            return;
        
        for (var i = 0; i < list.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("X", GUILayout.Width(20))) removeIndex = i;
            EditorGUILayout.EndHorizontal();

            var editor = list[i].Item2;
            editor.OnInspectorGUI();

            drawItem?.Invoke(i);

            EditorGUILayout.EndVertical();
        }

        if (removeIndex.HasValue)
        {
            var toRemove = list[removeIndex.Value];
            if (toRemove.Item2 != null) UnityEngine.Object.DestroyImmediate(toRemove.Item2);
            list.RemoveAt(removeIndex.Value);
        }
    }


    public static void ShowScriptableObjectDropdown(string dropdownTitle, List<(Type, string)> types, Action<Type> onSelected)
    {
        var dropdown = new ScriptableObjectTypeDropdown(dropdownTitle, new AdvancedDropdownState(), types, onSelected);
        var rect = new Rect(Event.current.mousePosition, Vector2.zero);
        dropdown.Show(rect);
    }

    public static (SerializedProperty, Editor) AddElementAndCreateEditor<T>(this SerializedProperty property, T element) where T : UnityEngine.Object
    {
        property.arraySize++;
        var newElement = property.GetArrayElementAtIndex(property.arraySize - 1);
        newElement.objectReferenceValue = element;
        return (newElement, Editor.CreateEditor(element));
    }
}