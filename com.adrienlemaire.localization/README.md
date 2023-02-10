# Unity UI
The Unity UI package allows you to set and change your localization in the easiest way.

## Prerequisites
### Unity 2021.3
This package requires Unity 2021.3.
This package requires TextMeshPro.
This package requires com.adrienlemaire.editor-windows : git+https://github.com/AdrienLMR/Utils.git?path=/com.adrienlemaire.editor-windows/.
This package requires com.adrienlemaire.attributes : git+https://github.com/AdrienLMR/Utils.git?path=/com.adrienlemaire.attributes/.

## How to Use It
### Component Text Localization
Add the component ``Text Localization`` next to your TextMeshPro and set its fields.
- ``textReference`` is the reference that you will use to recognize which text you want.
- ``Text Type`` is the type of this text. It is used to tidy your localization and find it more easily.

### Localization Window
Open the ``Localization Window`` by going to : **Window > Language Window**.
Here you can put all the references, the languages and the texts of your entire project.

To **add or remove** a language just click on the **plus or minus** button at the right of the label ``languages``. Then you just have to fill the name of your language in the text field that appeared.
Same for the ``keys``. It is where you put your reference of the text. As written earlier, the enumerator (currently ``BUTTONS``) let you choose the type of your text.

The biggest area is for your texts, you can put them according to their column(language) and lines(references).

There are two main buttons :
- ``Apply Changes`` - apply your change to every component ``Text Localization`` that has a textReference.
- ``CLear All`` - entirely clear your localization. Doesn't remove the components though.

When you remove a reference or a language, please note that it will remove the column/line of the **one that is being selected**. If nothing is selected it will remove the **last one**. 

#### Inputs
You can travel faster between text fields by pressing the ``Alt`` button and clicking on the ``Arrows``.

### Change Text
You can change the language of your game during playtime by simply putting those two lines of code :
``Localization localization = TextLocalization.GetLocalization;``
``localization.ChangeCurrentLanguage(0);``

GetLocalization property is a little heavy so it should be best to use it the least possible.

## Warning /!\
This window can be slow at times, Be a little patient or you might break the code and destroy all your previous work. If it still happens you can just click on ``Clear All`` and refill everything.