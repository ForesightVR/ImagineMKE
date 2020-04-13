using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Foresight.Utilities;

public class ToolTipManager : MonoBehaviour
{
    [TextArea]
    public List<string> tips;
    public float changeTime = 8;

    TextMeshProUGUI tipText;
    int index;

    private void Start()
    {
        tipText = GetComponent<TextMeshProUGUI>();
        InvokeRepeating("ChangeToolTip", 0, changeTime);
    }

    void ChangeToolTip()
    {
        tipText.text = "Tip: " + tips[index];
        index = index.IncrementLoop(tips.Count - 1);
    }
}
