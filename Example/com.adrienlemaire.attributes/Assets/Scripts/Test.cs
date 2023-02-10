using Com.AdrienLemaire.Attributes;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public static List<string> myList = new List<string>() { "a", "b", "c" };

    [ListToPopup(typeof(Test), "myList")]
    public string myString = "example";
}