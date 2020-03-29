using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckName : MonoBehaviour
{
    public GameObject nameWarning;

    public void Enter(TMP_InputField inputField)
    {
        if (inputField.text == "")
            nameWarning.SetActive(true);
        else
            NetworkConnectionManager.Instance.ConnectToMaster();
    }
}
