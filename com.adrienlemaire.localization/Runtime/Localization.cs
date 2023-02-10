using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.AdrienLemaire.Localization
{
    [CreateAssetMenu(menuName = "AdrienLemaire/Localization")]
    public class Localization : ScriptableObject
    {
        private const string BASE_LANGUAGE = "english";
        private const string REF_TEXT = "ref_ex";
        private const string CONTENT_TEXT = "";

        public List<string> languageList = new List<string>() { BASE_LANGUAGE };

        public KeysByRefType[] allKeys = new KeysByRefType[3] {
            new KeysByRefType(),
            new KeysByRefType(),
            new KeysByRefType()};

        public ContentByRefType[] contentList = new ContentByRefType[3] {
            new ContentByRefType(),
            new ContentByRefType(),
            new ContentByRefType()};

        public int currentLanguage = 0;

        public void ChangeCurrentLanguage(int language)
        {
            currentLanguage = language;

            TextLocalization[] texts = (TextLocalization[])Resources.FindObjectsOfTypeAll(typeof(TextLocalization));

            for (int i = texts.Length - 1; i >= 0; i--)
            {
                texts[i].SetText(currentLanguage);
            }
        }

        public enum LocalizationRefType
        {
            BUTTON = 0,
            UI_TEXT = 1,
            INGAME_TEXT = 2
        }

        [Serializable]
        public class KeysByRefType
        {
            public List<string> list = new List<string>() { REF_TEXT };
        }

        [Serializable]
        public class ContentByRefType
        {
            public List<ContentByRef> list = new List<ContentByRef>() { new ContentByRef() };
        }

        [Serializable]
        public class ContentByRef
        {
            public List<string> list = new List<string>() { CONTENT_TEXT };
            public int biggestSizeElement = 15;
        }
    }
}
