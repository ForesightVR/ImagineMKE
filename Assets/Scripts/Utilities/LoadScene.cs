using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void ChangeScene(int scene)
    {
        GameManager.Instance.ChangeScene(scene);
    }

    public void ChangeScene(string scene)
    {
        GameManager.Instance.ChangeScene(scene);
    }
}
