# EditorWindows
The Unity package allows you to add prefabricated sample windows to your project without having to make them yourself.

## Prerequisites
This package requires Unity 2021.3.

## Getting Started
### YesNoWindow
##### What for
In editor mode, if you want to ask any question that can be answered by yes or no, you can use the YesNoWindow. For example if the user gets ready to delete any data, you can ask them if they are sure to do so.

##### How to use it
You can call the function ```DrawWindow``` and put the name of your window and your question.

    YesNoWindow.DrawWindow(windowName, question);

Then bind ```OnAnswerYes``` and ```OnAnswerNo``` to an event.

    YesNoWindow.OnAnswerYes += LastCautionWindow_OnAnswerYes;
    YesNoWindow.OnAnswerNo += LastCautionWindow_OnAnswerNo;
    
When the user will click on yes or no, the window will disappear and one of the event will be called according to the answer.