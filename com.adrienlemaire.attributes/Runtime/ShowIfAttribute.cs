using UnityEngine;

namespace Com.AdrienLemaire.Attributes
{
	public class ShowIfAttribute : PropertyAttribute
    {
        public string fieldName;
        public object fieldValue;

        public ShowIfAttribute(string fieldName, object fieldValue)
        {
            this.fieldName = fieldName;
            this.fieldValue = fieldValue;
        }

        public ShowIfAttribute(string fieldName)
        {
            this.fieldName = fieldName;
            fieldValue = true;
        }
    }
}
