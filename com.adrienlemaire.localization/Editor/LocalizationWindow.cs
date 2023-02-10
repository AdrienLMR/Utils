using Com.Adrienlemaire.EditorWindows;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Com.AdrienLemaire.Localization.Localization;

namespace Com.AdrienLemaire.Localization
{
    public class LocalizationWindow : EditorWindow
    {
        private static readonly Color backgroundColor = new Color(25 / 256f, 25 / 256f, 25 / 256f);
        private static readonly Color areasColor = new Color(63 / 256f, 63 / 256f, 63 / 256f);

        private static readonly Vector2Int WINDOW_MIN_SIZE = new Vector2Int(500, 250);
        private static readonly Vector2Int TEXT_FIELD_SIZE = new Vector2Int(100, 20);
        private static readonly Vector2Int SPACE = new Vector2Int(4, 4);

        private const int HEADER_HEIGHT = 50;
        private const int TEXT_AREA_WIDTH = 200;
        private const int LANGUAGE_MANAGER_WIDTH = 20;

        private const string windowName = "Language Editor";
        private const string removeElementWarning = "the sign \"-\" removes element\nfocused or last element in the list";

        private const string keyLabel = "keys";
        private const string languageLabel = "languages";
        private const string contentLabel = "contents";
        private const string plusLabel = "+";
        private const string minusLabel = "-";

        private const string clearLabel = "Clear All";
        private const string applyLabel = "Apply Changes";
        private const string clearWarningLabel = "Are you sure you want to clear the whole list ?";

        private static LocalizationWindow window = default;

        private static Localization localization = default;

        private List<string> languageList = default;
        private List<string> keyList = default;
        private List<ContentByRef> contentList = default;

        private LocalizationRefType currentReferenceType = LocalizationRefType.BUTTON;

        private Texture2D backgroundTexture = default;
        private Texture2D areasTexture = default;

        private Rect backgroundRect = default;
        private Rect headerRect = default;
        private Rect keysNameRect = default;
        private Rect languageManagerRect = default;
        private Rect keysManagerRect = default;
        private Rect languageRect = default;
        private Rect keysRect = default;
        private Rect contentRect = default;

        private GUIStyle textAreaStyle;
        private GUIStyle wordWrapStyle;
        private GUIStyle buttonBoldStyle;
        private GUIStyle keysLabelStyle;
        private GUIStyle languageLabelStyle;
        private GUIStyle yellowWarningStyle;

        private GUILayoutOption languageManagerButtonWidthOption;
        private GUILayoutOption headerButtonHeightOption;
        private GUILayoutOption textFieldHeightOption;
        private GUILayoutOption textFieldWidthOption;
        private GUILayoutOption textAreaWidthOption;
        private GUILayoutOption languageRectOption;
        private GUILayoutOption keyRectOption;
        private GUILayoutOption contentScrollWidthOption;
        private GUILayoutOption contentScrollHeightOption;

        private Vector2 scrollBarKeysValue;
        private Vector2 scrollBarLanguageValue;
        private Vector2 scrollBarValues;

        private string focusedControl = "";

        private Action DoLoop = Void;

        #region Unity Methods
        public void OnEnable()
        {
            if (!window) window = (LocalizationWindow)GetWindow(typeof(LocalizationWindow));

            InitTextures();

            InitStyles();

            SetRects();

            DoLoop = InitGUI;
            DoLoop += GameLoop;
        }

        public void OnGUI()
        {
            DoLoop();
        }

        private static void Void() { }

        private void InitGUI()
        {
            buttonBoldStyle = new GUIStyle(GUI.skin.button);
            buttonBoldStyle.fontStyle = FontStyle.Bold;

            textAreaStyle = new GUIStyle(GUI.skin.textArea);
            textAreaStyle.alignment = TextAnchor.MiddleCenter;

            wordWrapStyle = new GUIStyle(GUI.skin.textArea);
            wordWrapStyle.wordWrap = true;

            DoLoop -= InitGUI;
        }

        private void GameLoop()
        {
            string _focusedControl = GUI.GetNameOfFocusedControl();
            if (_focusedControl != "") focusedControl = _focusedControl;

            MoveFocus();

            if (new Rect(0, 0, window.position.width, window.position.height) != backgroundRect)
                SetRects();

            DrawLayouts();
            DrawHeader();
            DrawLanguageManager();
            DrawKeysManager();
            DrawKeysName();
            DrawLanguages();
            DrawKeys();
            DrawContents();

            if (focusedControl != "") GUI.FocusControl(focusedControl);
        }
        #endregion

        #region Init
        [MenuItem("Window/Language Window")]
        public static void OpenWindow()
        {
            window = (LocalizationWindow)GetWindow(typeof(LocalizationWindow));
            window.minSize = WINDOW_MIN_SIZE;
            window.titleContent = new GUIContent(windowName);
            window.Show();

            Debug.Log("a");
            localization = (Localization)AssetDatabase.LoadAssetAtPath(TextLocalization.LOCALIZATION_PATH, typeof(Localization));
            Debug.Log(localization);
        }

        private void InitTextures()
        {
            backgroundTexture = new Texture2D(1, 1);
            backgroundTexture.SetPixel(0, 0, backgroundColor);
            backgroundTexture.Apply();

            areasTexture = new Texture2D(1, 1);
            areasTexture.SetPixel(0, 0, areasColor);
            areasTexture.Apply();
        }

        private void InitStyles()
        {
            yellowWarningStyle = new GUIStyle();
            yellowWarningStyle.alignment = TextAnchor.MiddleCenter;
            yellowWarningStyle.normal.textColor = Color.yellow;
            yellowWarningStyle.fontStyle = FontStyle.Bold;

            keysLabelStyle = new GUIStyle();
            keysLabelStyle.alignment = TextAnchor.MiddleCenter;
            keysLabelStyle.fontStyle = FontStyle.Bold;
            keysLabelStyle.normal.textColor = Color.gray;

            languageLabelStyle = new GUIStyle();
            languageLabelStyle.alignment = TextAnchor.MiddleCenter;
            languageLabelStyle.fontStyle = FontStyle.Bold;
            languageLabelStyle.normal.textColor = Color.gray;

            languageManagerButtonWidthOption = GUILayout.Width(LANGUAGE_MANAGER_WIDTH);
            headerButtonHeightOption = GUILayout.Height(HEADER_HEIGHT);
            textFieldHeightOption = GUILayout.Height(TEXT_FIELD_SIZE.y);
            textFieldWidthOption = GUILayout.Width(TEXT_FIELD_SIZE.x);
            textAreaWidthOption = GUILayout.Width(TEXT_AREA_WIDTH);
        }
        #endregion

        #region Draw
        private void SetRects()
        {
            backgroundRect = new Rect(0, 0, window.position.width, window.position.height);

            headerRect = new Rect(SPACE.x, SPACE.y, window.position.width - SPACE.x * 2, HEADER_HEIGHT + SPACE.y);

            languageManagerRect = new Rect(window.position.width - TEXT_FIELD_SIZE.y - SPACE.x, headerRect.height + SPACE.y * 2, TEXT_FIELD_SIZE.y, TEXT_FIELD_SIZE.y * 2 + SPACE.y);

            keysManagerRect = new Rect(SPACE.x, window.position.height - TEXT_FIELD_SIZE.y - SPACE.y, TEXT_FIELD_SIZE.x + SPACE.x * 2, TEXT_FIELD_SIZE.y);

            keysNameRect = new Rect(keysManagerRect.x, languageManagerRect.y, keysManagerRect.width, languageManagerRect.height + SPACE.y);

            keysRect = new Rect(
                keysManagerRect.x,
                languageManagerRect.y + keysNameRect.height,
                keysManagerRect.width,
                window.position.height - headerRect.height - keysManagerRect.height - keysNameRect.height - SPACE.y * 4);

            languageRect = new Rect(
                keysManagerRect.width + SPACE.x * 2,
                languageManagerRect.y,
                window.position.width - keysRect.width - languageManagerRect.width - SPACE.x * 4,
                languageManagerRect.height);

            contentRect = new Rect(
                keysManagerRect.width + SPACE.x * 2,
                languageManagerRect.y + languageManagerRect.height + SPACE.y,
                window.position.width - keysManagerRect.width - SPACE.x * 3,
                window.position.height - headerRect.height - languageManagerRect.height - SPACE.y * 4);

            languageRectOption = GUILayout.Height(languageRect.height);
            keyRectOption = GUILayout.Height(keysRect.height);
            contentScrollWidthOption = GUILayout.Width(contentRect.width - SPACE.x - languageManagerRect.width + 13);
            contentScrollHeightOption = GUILayout.Height(contentRect.height - SPACE.y - keysManagerRect.height + 13);
        }

        private void DrawLayouts()
        {
            GUI.DrawTexture(backgroundRect, backgroundTexture);
            GUI.DrawTexture(headerRect, areasTexture);
            GUI.DrawTexture(languageManagerRect, backgroundTexture);
            GUI.DrawTexture(keysManagerRect, backgroundTexture);
            GUI.DrawTexture(keysNameRect, areasTexture);
            GUI.DrawTexture(languageRect, areasTexture);
            GUI.DrawTexture(keysRect, areasTexture);
            GUI.DrawTexture(contentRect, areasTexture);
        }

        private void DrawHeader()
        {
            GUILayout.BeginArea(headerRect);
            {
                GUILayout.BeginHorizontal();
                {
                    Color oldColor = GUI.backgroundColor;

                    GUILayout.Space(5);

                    Apply();

                    GUILayout.Space(5);

                    CallClear();

                    GUI.backgroundColor = oldColor;

                    GUILayout.Label(removeElementWarning, yellowWarningStyle, headerButtonHeightOption);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private void DrawLanguageManager()
        {
            GUILayout.BeginArea(languageManagerRect);
            {
                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button(plusLabel, languageManagerButtonWidthOption, textFieldHeightOption))
                        AddLanguage();

                    GUILayout.Space(2);

                    if (GUILayout.Button(minusLabel, languageManagerButtonWidthOption, textFieldHeightOption))
                        RemoveLanguage();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        private void DrawKeysManager()
        {
            GUILayout.BeginArea(keysManagerRect);
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(plusLabel, textFieldHeightOption))
                        AddKey();
                    else if (GUILayout.Button(minusLabel, textFieldHeightOption))
                        RemoveKey();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private void DrawKeysName()
        {
            GUILayout.BeginArea(keysNameRect);
            {
                GUILayout.Label(keyLabel, keysLabelStyle, textFieldHeightOption);

                currentReferenceType = (LocalizationRefType)EditorGUILayout.EnumPopup(currentReferenceType, textFieldHeightOption);

                keyList = localization.allKeys[(int)currentReferenceType].list;
                contentList = localization.contentList[(int)currentReferenceType].list;
                languageList = localization.languageList;
            }
            GUILayout.EndArea();
        }

        private void DrawLanguages()
        {
            GUILayout.BeginArea(languageRect);
            {
                GUILayout.Label(languageLabel, languageLabelStyle, textFieldHeightOption);

                scrollBarLanguageValue = GUILayout.BeginScrollView(new Vector2(scrollBarValues.x, 0), GUIStyle.none, GUIStyle.none, languageRectOption);
                {
                    scrollBarValues.x = scrollBarLanguageValue.x;

                    GUILayout.BeginHorizontal();
                    {
                        for (int i = 0; i < languageList.Count; i++)
                        {
                            GUI.SetNextControlName($"{languageLabel}{i}");
                            languageList[i] = EditorGUILayout.TextField(languageList[i], textAreaStyle, textAreaWidthOption, textFieldHeightOption);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private void DrawKeys()
        {
            GUILayout.BeginArea(keysRect);
            {
                scrollBarKeysValue = GUILayout.BeginScrollView(new Vector2(0, scrollBarValues.y), GUIStyle.none, GUIStyle.none, keyRectOption);
                {
                    scrollBarValues.y = scrollBarKeysValue.y;

                    GUILayout.BeginVertical();
                    {
                        int size;

                        for (int i = 0; i < keyList.Count; i++)
                        {
                            size = contentList[i].biggestSizeElement;

                            if (size < TEXT_FIELD_SIZE.y - 5) size = TEXT_FIELD_SIZE.y - 5;

                            GUI.SetNextControlName($"{keyLabel}{i}");
                            keyList[i] = EditorGUILayout.TextField(keyList[i], textAreaStyle, textFieldWidthOption, GUILayout.Height(size));
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private void DrawContents()
        {
            GUILayout.BeginArea(contentRect);
            {
                using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollBarValues, contentScrollWidthOption, contentScrollHeightOption))
                {
                    scrollBarValues = scrollView.scrollPosition;

                    GUILayout.BeginVertical();
                    {
                        List<string> stringList;

                        int biggestSize;
                        int size;

                        for (int i = 0; i < contentList.Count; i++)
                        {
                            biggestSize = 0;

                            GUILayout.BeginHorizontal();
                            {
                                stringList = contentList[i].list;

                                for (int j = 0; j < stringList.Count; j++)
                                {
                                    GUI.SetNextControlName($"{contentLabel}{j}-{i}");
                                    contentList[i].list[j] = EditorGUILayout.TextArea(stringList[j], wordWrapStyle, textAreaWidthOption);

                                    size = (int)GUILayoutUtility.GetLastRect().height;

                                    if (biggestSize < size)
                                        biggestSize = size;
                                }

                                contentList[i].biggestSizeElement = biggestSize;
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndArea();
        }
        #endregion

        #region Params
        private void AddKey()
        {
            keyList.Add("");

            ContentByRef localization = new ContentByRef();

            for (int i = 0; i < languageList.Count - 1; i++)
            {
                localization.list.Add("");
            }

            contentList.Add(localization);
        }

        private void RemoveKey()
        {
            if (keyList.Count > 1)
            {
                int index = keyList.Count - 1;

                if (focusedControl != "" && focusedControl.Contains(keyLabel))
                    index = int.Parse(focusedControl.Substring(keyLabel.Length));

                keyList.RemoveAt(index);
                contentList.RemoveAt(index);
            }
        }

        private void AddLanguage()
        {
            languageList.Add("");

            for (int i = localization.contentList.Length - 1; i >= 0; i--)
            {
                for (int j = localization.contentList[i].list.Count - 1; j >= 0; j--)
                {
                    localization.contentList[i].list[j].list.Add("");
                }
            }
        }

        private void RemoveLanguage()
        {
            if (languageList.Count > 1)
            {
                int index = languageList.Count - 1;

                if (focusedControl != "" && focusedControl.Contains(languageLabel))
                    index = int.Parse(focusedControl.Substring(languageLabel.Length));

                languageList.RemoveAt(index);

                for (int i = 0; i < contentList.Count; i++)
                {
                    contentList[i].list.RemoveAt(index);
                }
            }
        }
        #endregion

        #region Buttons
        private void Apply()
        {
            GUI.backgroundColor = Color.green;

            if (GUILayout.Button(applyLabel, buttonBoldStyle, textFieldWidthOption, headerButtonHeightOption))
            {
                int currentLanguage = localization.currentLanguage;

                TextLocalization[] texts = (TextLocalization[])Resources.FindObjectsOfTypeAll(typeof(TextLocalization));

                foreach (var item in texts)
                {
                    item.SetText(currentLanguage);
                }
            }

            EditorUtility.SetDirty(localization);
        }

        private void CallClear()
        {
            GUI.backgroundColor = Color.red;

            if (GUILayout.Button(clearLabel, buttonBoldStyle, textFieldWidthOption, headerButtonHeightOption))
            {
                YesNoWindow.DrawWindow("Clear", clearWarningLabel);

                YesNoWindow.OnAnswerYes += LastCautionWindow_OnAnswerYes;
                YesNoWindow.OnAnswerNo += LastCautionWindow_OnAnswerNo;
            }
        }

        private void Clear()
        {
            localization.languageList.Clear();
            localization.languageList = new List<string>() { "english" };

            localization.allKeys = new KeysByRefType[3] {
                            new KeysByRefType(),
                            new KeysByRefType(),
                            new KeysByRefType()};

            localization.contentList = new ContentByRefType[3] {
                            new ContentByRefType(),
                            new ContentByRefType(),
                            new ContentByRefType()};

            TextLocalization[] texts = (TextLocalization[])Resources.FindObjectsOfTypeAll(typeof(TextLocalization));

            foreach (var item in texts)
            {
                item.textType = LocalizationRefType.BUTTON;
            }

            EditorUtility.SetDirty(localization);
        }
        #endregion

        #region Events
        private void LastCautionWindow_OnAnswerYes(YesNoWindow sender)
        {
            Clear();

            YesNoWindow.OnAnswerYes -= LastCautionWindow_OnAnswerYes;
            YesNoWindow.OnAnswerNo -= LastCautionWindow_OnAnswerNo;
        }

        private void LastCautionWindow_OnAnswerNo(YesNoWindow sender)
        {
            YesNoWindow.OnAnswerYes -= LastCautionWindow_OnAnswerYes;
            YesNoWindow.OnAnswerNo -= LastCautionWindow_OnAnswerNo;
        }
        #endregion

        #region Move Focus
        private void MoveFocus()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                focusedControl = "";
                return;
            }

            if (!Event.current.alt || Event.current.type != EventType.KeyDown) return;

            if (Event.current.keyCode == KeyCode.UpArrow) MoveUp();
            else if (Event.current.keyCode == KeyCode.DownArrow) MoveDown();
            else if (Event.current.keyCode == KeyCode.LeftArrow) MoveLeft();
            else if (Event.current.keyCode == KeyCode.RightArrow) MoveRight();
        }

        private void MoveUp()
        {
            if (focusedControl.Contains(keyLabel))
            {
                FocusListPrevious(keyLabel);
            }
            else if (focusedControl.Contains(languageLabel))
            {
                int index = int.Parse(focusedControl.Substring(languageLabel.Length));

                focusedControl = $"{contentLabel}{index}-{contentList.Count - 1}";
            }
            else if (focusedControl.Contains(contentLabel))
            {
                int indexX = int.Parse(focusedControl.Substring(contentLabel.Length, 1));
                int indexY = int.Parse(focusedControl.Substring(focusedControl.Length - 1)) - 1;

                if (indexY < 0) focusedControl = $"{languageLabel}{indexX}";
                else focusedControl = $"{contentLabel}{indexX}-{indexY}";
            }
        }

        private void MoveDown()
        {
            if (focusedControl.Contains(keyLabel))
            {
                FocusListNext(keyLabel, keyList);
            }
            else if (focusedControl.Contains(languageLabel))
            {
                int index = int.Parse(focusedControl.Substring(languageLabel.Length));

                focusedControl = $"{contentLabel}{index}-0";
            }
            else if (focusedControl.Contains(contentLabel))
            {
                int indexX = int.Parse(focusedControl.Substring(contentLabel.Length, 1));
                int indexY = int.Parse(focusedControl.Substring(focusedControl.Length - 1)) + 1;

                if (indexY >= contentList.Count) focusedControl = $"{languageLabel}{indexX}";
                else focusedControl = $"{contentLabel}{indexX}-{indexY}";
            }
        }

        private void MoveLeft()
        {
            if (focusedControl.Contains(keyLabel))
            {
                int index = int.Parse(focusedControl.Substring(keyLabel.Length));

                focusedControl = $"{contentLabel}{contentList[0].list.Count - 1}-{index}";
            }
            else if (focusedControl.Contains(languageLabel))
            {
                FocusListPrevious(languageLabel);
            }
            else if (focusedControl.Contains(contentLabel))
            {
                int indexX = int.Parse(focusedControl.Substring(contentLabel.Length, 1)) - 1;
                int indexY = int.Parse(focusedControl.Substring(focusedControl.Length - 1));

                if (indexX < 0) focusedControl = $"{keyLabel}{indexY}";
                else focusedControl = $"{contentLabel}{indexX}-{indexY}";
            }
        }

        private void MoveRight()
        {
            if (focusedControl.Contains(keyLabel))
            {
                int index = int.Parse(focusedControl.Substring(keyLabel.Length));

                focusedControl = $"{contentLabel}0-{index}";
            }
            else if (focusedControl.Contains(languageLabel))
            {
                FocusListNext(languageLabel, languageList);
            }
            else if (focusedControl.Contains(contentLabel))
            {
                int indexX = int.Parse(focusedControl.Substring(contentLabel.Length, 1)) + 1;
                int indexY = int.Parse(focusedControl.Substring(focusedControl.Length - 1));

                if (indexX >= contentList[indexY].list.Count) focusedControl = $"{keyLabel}{indexY}";
                else focusedControl = $"{contentLabel}{indexX}-{indexY}";
            }
        }

        private void FocusListPrevious(string label)
        {
            int index = int.Parse(focusedControl.Substring(label.Length)) - 1;
            if (index == -1) index = keyList.Count - 1;

            focusedControl = $"{label}{index}";
        }

        private void FocusListNext(string label, List<string> labelList)
        {
            int index = int.Parse(focusedControl.Substring(label.Length)) + 1;
            if (index >= labelList.Count) index = 0;

            focusedControl = $"{label}{index}";
        }
        #endregion
    }
}
