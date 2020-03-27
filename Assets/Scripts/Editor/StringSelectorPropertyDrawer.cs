using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(StringSelectorAttribute))]
public class StringSelectorPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String) /// if the variable you're setting isn't a string, this doesn't work
        {
            EditorGUI.BeginProperty(position, label, property);

            var attrib = this.attribute as StringSelectorAttribute;

            string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
            string conditionPath = propertyPath.Replace(property.name, attrib.stringDropDownName); //changes the path to the conditionalsource property path
            SerializedProperty sp = property.serializedObject.FindProperty(conditionPath); // this returns the serialzed property that matches the one in the attribute

            List<string> stringList = new List<string>();

            if (sp.isArray)
            {
                int arrayLength = 0;

                sp.Next(true); // skip generic field
                sp.Next(true); // advance to array size field

                // Get the array size
                arrayLength = sp.intValue;

                sp.Next(true); // advance to first array index

                int lastIndex = arrayLength - 1;
                for (int i = 0; i < arrayLength; i++)
                {
                    stringList.Add(sp.stringValue); // copy the value to the list
                    if (i < lastIndex) sp.Next(false); // advance without drilling into children
                }
            }            

            string propertyString = property.stringValue;
            int index = -1;
           
            //check if there is an entry that matches the entry and get the index
            for (int i = 0; i < stringList.Count; i++)
            {
                if (stringList[i] == propertyString)
                {
                    index = i;
                    break;
                }
            }
            

            //Draw the popup box with the current selected index
            index = EditorGUI.Popup(position, label.text, index, stringList.ToArray());

            //Adjust the actual string value of the property based on the selection
            if (index == 0)
            {
                property.stringValue = stringList[index];
            }
            else if (index >= 1)
            {
                property.stringValue = stringList[index];
            }
            else
            {
                property.stringValue = "";
            }
        }

        EditorGUI.EndProperty();
        
       
    }
}
