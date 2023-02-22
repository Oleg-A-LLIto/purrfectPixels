using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorManager : MonoBehaviour
{
    [SerializeField] private Text Message;
    [SerializeField] private GameObject ErrorWindow;
    [SerializeField] private bool logErrors;

    public void ShowErrorMessage(string _msg)
    {
        ErrorWindow.SetActive(true);
        Message.text = _msg;
        if (logErrors)
            Debug.LogError(_msg);
    }
}
