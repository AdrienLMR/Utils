using Com.AdrienLemaire.Attributes;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public static List<string> myList = new List<string>() { "a", "b", "c" };

    [ListToPopup(typeof(Test), "myList")]
    public string myString = "example";

    [Space(10)]
    [ReadOnly]
    public string readOnlyString = "example";

    [Space(10)]
    public bool showValues = false;

    [ShowIf("showValues", false)]
    public string value1 = string.Empty;
}