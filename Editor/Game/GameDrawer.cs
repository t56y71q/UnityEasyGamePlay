using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyGamePlay.Editor
{
    [CustomPropertyDrawer(typeof(GameAttribute))]
    class GameDrawer:PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GameAttribute gameAttribute = attribute as GameAttribute;

            GUI.Label(position, property.displayName);
            gameAttribute.index = EditorGUI.Popup(new Rect(position.x + 70f, position.y, position.width, position.height), gameAttribute.index, FrameWorkEditor.gameColloction.GetNames());
            Type type = FrameWorkEditor.gameColloction.GetGameType();
            property.stringValue = type.FullName + "," + type.Assembly.GetName().Name;
        }
    }
}
