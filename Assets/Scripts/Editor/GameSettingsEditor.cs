using System;
using ScriptableObjects;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameSettings))]
    public class GameSettingsEditor : UnityEditor.Editor
    {
        private static int SPACING = 4;
        private static int PADDING = 4;
    
        private ReorderableList _list;
        private float _listX = 0f;

        private void OnEnable()
        {
            _list = new ReorderableList(serializedObject, serializedObject.FindProperty("shopItems"),
                true, true, true, true)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawElement,
                onAddDropdownCallback = AddDropdown,
                elementHeightCallback = ElementHeight
            };
        }

        private void DrawHeader(Rect rect)
        {
            _listX = rect.x - 4;
            var third = rect.width / 3f;
            
            EditorGUI.LabelField(new Rect(rect.x, rect.y, third, rect.height), "Shop Items");
            EditorGUI.LabelField(new Rect(rect.x + third, rect.y, third, rect.height), "Size: ",
                new GUIStyle(){alignment = TextAnchor.MiddleRight});
            
            var newSize = EditorGUI.IntField(new Rect(rect.x + 2 * third, rect.y, third, rect.height), 
                _list.serializedProperty.arraySize);

            var diff = newSize - _list.serializedProperty.arraySize;
        
            if (diff == 0)
                return;
        
            if (diff > 0) for (int i = 0; i < diff; i++)
            {
                Add(0);
            }
            else
            {
                _list.serializedProperty.arraySize = newSize;
            }
        }

        private float ElementHeight(int index)
        {
            var element = _list.serializedProperty.GetArrayElementAtIndex(index);
            var type = element.FindPropertyRelative("ItemType");
            var typeValue = (ShopItem.Type) type.intValue;
            return GetItemHeight(typeValue);
        }
    
        private float GetItemHeight(ShopItem.Type typeValue)
        {
            var height = 0f;
        
            switch (typeValue)
            {
                case ShopItem.Type.Image: height = GetHeight(3);
                    break;
                case ShopItem.Type.String: height = GetHeight(3);
                    break;
                case ShopItem.Type.ImageString: height = GetHeight(4);
                    break;
                case ShopItem.Type.TwoImages: height = GetHeight(4);
                    break;
                case ShopItem.Type.TwoStrings: height = GetHeight(4);
                    break;
                case ShopItem.Type.Money: height = GetHeight(2);
                    break;
            }

            return height;

            float GetHeight(int lines)
            {
                return PADDING + lines * (EditorGUIUtility.singleLineHeight + SPACING);
            }
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        { 
            var row = 0;
            var element = _list.serializedProperty.GetArrayElementAtIndex(index);
            var type = element.FindPropertyRelative("ItemType");

            if (index % 2 != 0)
                EditorGUI.DrawRect(rect, new Color(0,0,0,.05f));
        
            float GetY()
            {
                return rect.y + PADDING + (row++) * (EditorGUIUtility.singleLineHeight + SPACING);
            }
        
            float GetX()
            {
                return rect.x + PADDING;
            }
        
            float GetWidth()
            {
                return rect.width - PADDING*2;
            }
        
            EditorGUI.LabelField(new Rect(_listX, rect.y + rect.height/2 - EditorGUIUtility.singleLineHeight/2,
                    rect.x - _listX, EditorGUIUtility.singleLineHeight), index.ToString());

            EditorGUI.PropertyField(new Rect(GetX(), GetY(), 
                GetWidth(), EditorGUIUtility.singleLineHeight), type, new GUIContent("Item Type"));

            var typeValue = (ShopItem.Type) type.intValue;
            switch (typeValue)
            {
                case ShopItem.Type.Image: DrawImage();
                    break;
                case ShopItem.Type.String: DrawString();
                    break;
                case ShopItem.Type.ImageString: DrawImage(); DrawString();
                    break;
                case ShopItem.Type.TwoImages: DrawImage(2);
                    break;
                case ShopItem.Type.TwoStrings: DrawString(2);
                    break;
            }

            var isMoney = typeValue == ShopItem.Type.Money;
            EditorGUI.PropertyField(new Rect(GetX(), GetY(), GetWidth(), 
                EditorGUIUtility.singleLineHeight), element.FindPropertyRelative(isMoney ? "Money" : "Price"), new GUIContent(isMoney ? "Money" : "Price"));
        
            EditorGUI.DrawRect(new Rect(rect.x, rect.y + GetItemHeight(typeValue), rect.width, 1), new Color(0,0,0,.5f));
        
            void DrawImage(int count = 1)
            {
                var sprites = element.FindPropertyRelative("Sprites");
                for (int i = 0; i < count && i < sprites.arraySize; i++)
                {
                    EditorGUI.PropertyField(new Rect(GetX(), GetY(), GetWidth(), EditorGUIUtility.singleLineHeight), 
                        sprites.GetArrayElementAtIndex(i), new GUIContent($"Image {i + 1}"));
                }
            }
        
            void DrawString(int count = 1)
            {
                var strings = element.FindPropertyRelative("Strings");
                for (int i = 0; i < count && i < strings.arraySize; i++)
                {
                    EditorGUI.PropertyField(new Rect(GetX(), GetY(), GetWidth(), EditorGUIUtility.singleLineHeight), 
                        strings.GetArrayElementAtIndex(i), new GUIContent($"String {i + 1}"));
                }
            }
        }

        private void AddDropdown(Rect rect, ReorderableList list)
        {
            var menu = new GenericMenu();
        
            string[] options = Enum.GetNames(typeof(ShopItem.Type));

            for (var i = 0; i < options.Length; i++)
            {
                var index = i;
                var option = options[index];
                menu.AddItem(new GUIContent(option), false, () => Add(index));
            }
            
            menu.ShowAsContext();
        }
    
        private void Add(int index)
        {
            var last = _list.serializedProperty.arraySize++;
            _list.index = last;
            var element = _list.serializedProperty.GetArrayElementAtIndex(last);
            element.FindPropertyRelative("ItemType").enumValueIndex = index;
            element.FindPropertyRelative("Sprites").arraySize = 2;
            element.FindPropertyRelative("Strings").arraySize = 2;
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            var settings = (GameSettings) target;

            settings.startMoney = EditorGUILayout.IntField("Start Money", settings.startMoney);
        
            EditorGUILayout.Separator();
        
            serializedObject.Update();
            _list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}