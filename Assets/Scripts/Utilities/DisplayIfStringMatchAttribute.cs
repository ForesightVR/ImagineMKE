using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An attribute that checks if the provided string value matches the named variable. If it is true, the variable this attribute is over will appear in the Unity Editor.
/// </summary>
public class DisplayIfStringMatchAttribute : PropertyAttribute
{
    public string stringVariableName;
    public string stringValue;

    /// <summary>
    /// A constructor that sets the variable you are checking and the value you want it to be.
    /// </summary>
    /// <param name="variableName">
    /// The name of the variable you are checking for its value.
    /// </param>
    /// <param name="stringValue">
    /// If the variable in the first parameter matches this value, the variable that this attribute is on will appear.
    /// </param>
    public DisplayIfStringMatchAttribute(string variableName, string stringValue)
    {
        this.stringVariableName = variableName;
        this.stringValue = stringValue;
    }
}
