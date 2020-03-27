using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(DisplayIfStringMatchAttribute))]
public class DisplayIfStringMatchPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {       
        var attrib = this.attribute as DisplayIfStringMatchAttribute;
        bool enabled = CheckIfStringMatches(attrib, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        GUI.enabled = wasEnabled;        
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        DisplayIfStringMatchAttribute condHAtt = (DisplayIfStringMatchAttribute)attribute;
        bool enabled = CheckIfStringMatches(condHAtt, property);

        if (enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            //The property is not being drawn
            //We want to undo the spacing added before and after the property
            return -EditorGUIUtility.standardVerticalSpacing;
            //return 0.0f;
        }


        /*
        //Get the base height when not expanded
        var height = base.GetPropertyHeight(property, label);

        // if the property is expanded go through all its children and get their height
        if (property.isExpanded)
        {
            var propEnum = property.GetEnumerator();
            while (propEnum.MoveNext())
                height += EditorGUI.GetPropertyHeight((SerializedProperty)propEnum.Current, GUIContent.none, true);
        }
        return height;*/
    }

    bool CheckIfStringMatches(DisplayIfStringMatchAttribute att, SerializedProperty property)
    {
        string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath = propertyPath.Replace(property.name, att.stringVariableName); //changes the path to the conditionalsource property path
        SerializedProperty sp = property.serializedObject.FindProperty(conditionPath); // this returns the serialzed property that matches the one in the attribute

        return sp.stringValue == att.stringValue;
    }
}
