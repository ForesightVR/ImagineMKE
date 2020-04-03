using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This takes in a reference to a variable name in the class. That variable should be an array. This attribute will convert that array into a drop down list of options for the variable this attrbiute is above.
/// </summary>
public class StringSelectorAttribute : PropertyAttribute
{
    public string stringDropDownName;

    /// <summary>
    /// Creating a string selector attribute
    /// </summary>
    /// <param name="dropDown">
    /// The name of the variable that holds what you want the options to be.
    /// </param>
    public StringSelectorAttribute(string dropDown)
    {
        stringDropDownName = dropDown;
    }

}
