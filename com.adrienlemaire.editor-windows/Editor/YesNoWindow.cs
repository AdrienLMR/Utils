using UnityEditor;
using UnityEngine;

namespace Com.Adrienlemaire.EditorWindows {

    public delegate void LastCautionWindowEventHandler(YesNoWindow sender);
    public class YesNoWindow : EditorWindow
    {
        private static readonly Vector2Int WINDOW_MIN_SIZE = new Vector2Int(300, 100);
        private const int BUTTON_OFFSET_Y = 30;
        private const string YES = "Yes";
        private const string NO = "No";

        private static YesNoWindow window = default;

        private static string question;

        public static event LastCautionWindowEventHandler OnAnswerYes;
        public static event LastCautionWindowEventHandler OnAnswerNo;

        public static void DrawWindow(string windowName, string question)
        {
            YesNoWindow.question = question;

            if (!window) window = (YesNoWindow)GetWindow(typeof(YesNoWindow));
            window.titleContent = new GUIContent(windowName);
            window.minSize = WINDOW_MIN_SIZE;
            window.Show();
        }

        private void OnGUI()
        {
            Rect windowRect = new Rect(0, 0, window.position.width, window.position.height);

            GUILayout.BeginArea(windowRect);
            {
                GUILayout.BeginVertical();
                {
                    GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                    labelStyle.alignment = TextAnchor.MiddleCenter;

                    GUILayout.Label(question, labelStyle);

                    GUILayout.Space(windowRect.height - labelStyle.CalcSize(new GUIContent(question)).y - BUTTON_OFFSET_Y);

                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button(YES))
                        {
                            OnAnswerYes?.Invoke(this);
                            Close();
                        }
                        else if (GUILayout.Button(NO))
                        {
                            OnAnswerNo?.Invoke(this);
                            Close();
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }
    }
}
