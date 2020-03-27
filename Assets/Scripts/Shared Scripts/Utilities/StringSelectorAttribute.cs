using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StringSelectorAttribute : PropertyAttribute
{
    public string stringDropDownName;

    public StringSelectorAttribute(string dropDown)
    {
        stringDropDownName = dropDown;
    }

}
