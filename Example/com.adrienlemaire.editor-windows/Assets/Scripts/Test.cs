using Com.Adrienlemaire.EditorWindows;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        YesNoWindow.DrawWindow("Test", "Is this as Test ?");
        YesNoWindow.OnAnswerYes += YesNoWindow_OnAnswerYes;
        YesNoWindow.OnAnswerNo += YesNoWindow_OnAnswerNo;
    }

    private void YesNoWindow_OnAnswerYes(YesNoWindow sender)
    {
        Debug.Log("It is a test.");
    }

    private void YesNoWindow_OnAnswerNo(YesNoWindow sender)
    {
        Debug.Log("It is not a test.");
    }
}