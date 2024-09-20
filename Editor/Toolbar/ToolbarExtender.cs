using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace yuxuetian
{
    public static class ToolbarExtender
    {
        static int m_toolCount;
        static GUIStyle m_commandStyle = null;
        
        public static readonly List<Action> LeftToolbarGUI = new List<Action>();
        // public static readonly List<Action> RightToolbarGUI = new List<Action>();

        public const float space = 8;
        
        public const float largeSpace = 20;
        public const float buttonWidth = 32;
        public const float dropdownWidth = 80;
        
        public const float playPauseStopWidth = 140;

        static ToolbarExtender()
        {
            Type toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");

            string fieldName = "k_ToolCount";
            FieldInfo toolIcons = toolbarType.GetField(fieldName,BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            m_toolCount = toolIcons != null ? ((int)toolIcons.GetValue(null)) : 8;
            
            ToolbarCallback.OnToolbarGUI = OnGUI;
            ToolbarCallback.OnToolbarGUILeft = GUILeft;
            // ToolbarCallback.OnToolbarGUIRight = GUIRight;
        }
        
        static void OnGUI()
        {
            if (m_commandStyle == null)
            {
                m_commandStyle = new GUIStyle("CommandLeft");
            }

            var screenWidth = EditorGUIUtility.currentViewWidth;
            
            float playButtonsPosition = Mathf.RoundToInt((screenWidth - playPauseStopWidth) / 2);
            
            Rect leftRect = new Rect(0, 0, screenWidth, Screen.height);
            leftRect.xMin += space; // Spacing left
            leftRect.xMin += buttonWidth * m_toolCount; // Tool buttons
            #if UNITY_2019_3_OR_NEWER
                leftRect.xMin += space; // Spacing between tools and pivot
            #else
			    leftRect.xMin += largeSpace; // Spacing between tools and pivot
            #endif
            leftRect.xMin += 64 * 2; // Pivot buttons
            leftRect.xMax = playButtonsPosition;

            Rect rightRect = new Rect(0, 0, screenWidth, Screen.height);
            rightRect.xMin = playButtonsPosition;
            rightRect.xMin += m_commandStyle.fixedWidth * 3; // Play buttons
            rightRect.xMax = screenWidth;
            rightRect.xMax -= space; // Spacing right
            rightRect.xMax -= dropdownWidth; // Layout
            rightRect.xMax -= space; // Spacing between layout and layers
            rightRect.xMax -= dropdownWidth; // Layers
            #if UNITY_2019_3_OR_NEWER
                rightRect.xMax -= space; // Spacing between layers and account
            #else
			    rightRect.xMax -= largeSpace; // Spacing between layers and account
            #endif
            rightRect.xMax -= dropdownWidth; // Account
            rightRect.xMax -= space; // Spacing between account and cloud
            rightRect.xMax -= buttonWidth; // Cloud
            rightRect.xMax -= space; // Spacing between cloud and collab
            rightRect.xMax -= 78; // Colab

            // Add spacing around existing controls
            leftRect.xMin += space;
            leftRect.xMax -= space;
            rightRect.xMin += space;
            rightRect.xMax -= space;

            // Add top and bottom margins
            #if UNITY_2019_3_OR_NEWER
                leftRect.y = 4;
                leftRect.height = 22;
                rightRect.y = 4;
                rightRect.height = 22;
            #else
			    leftRect.y = 5;
			    leftRect.height = 24;
			    rightRect.y = 5;
			    rightRect.height = 24;
            #endif

            if (leftRect.width > 0)
            {
                GUILayout.BeginArea(leftRect);
                GUILayout.BeginHorizontal();
                foreach (var handler in LeftToolbarGUI)
                {
                    handler();
                }

                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }

            // if (rightRect.width > 0)
            // {
            //     GUILayout.BeginArea(rightRect);
            //     GUILayout.BeginHorizontal();
            //     foreach (var handler in RightToolbarGUI)
            //     {
            //         handler();
            //     }
            //
            //     GUILayout.EndHorizontal();
            //     GUILayout.EndArea();
            // }
            
        }

        static void GUILeft()
        {
            GUILayout.BeginHorizontal();
            foreach (var handler in LeftToolbarGUI)
            {
                handler();
            }
            GUILayout.EndHorizontal();
        }

        // static void GUIRight()
        // {
        //     GUILayout.BeginHorizontal();
        //     foreach (var handler in RightToolbarGUI)
        //     {
        //         handler();
        //     }
        //     GUILayout.EndHorizontal();
        // }
    }
}
