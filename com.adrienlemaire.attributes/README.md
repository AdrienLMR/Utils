# Attributes
The Unity package allows you to add already made custom attributes to your projects.

## Prerequisites
This package requires Unity 2021.3.

## Getting Started
### ListToPopup
##### What for
In the editor transform visually a static List like if it was a dynamic enum.
By example, with a list of string, if you want to choose any string inside of it, you will have to create a reference or type it again. With this attribute, the references are already made and you only have to make two clicks.

##### How to use it
Example :

    public static List<string> myList = new List<string>() { "a", "b", "c" };

    [ListToPopup(typeof(Test), "myList")]
    public string myString = "example";
    
In this example, there are :
- a static list ```myList``` which contain what you want **(complex objects doesn't work)**.
- a string ```myString``` which will be seen in the inspector as an enum
    The string is not made for being set by any other code than the attribute.

##### References
    URocks!
    https://www.youtube.com/watch?v=ThcSHbVh7xc

### ReadOnly
##### What for
Prohibit user to change a field in the editor. They will only be able to read it.

##### How to use it
Just put ```[ReadOnly]``` before your field

##### References
    Comp-3 Interactive
    https://www.youtube.com/watch?v=r3nwTGLHygI