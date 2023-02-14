using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.AdrienLemaire.Attributes
{
    public class ListToPopupAttribute : PropertyAttribute
    {
        public Type type;
        public string name;
        public List<string> list;

        public ListToPopupAttribute(Type type, string name)
        {
            this.type = type;
            this.name = name;
        }
    }
}
