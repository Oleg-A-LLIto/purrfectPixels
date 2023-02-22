using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldManager : MonoBehaviour
{
    [SerializeField] private InputField uriField;
    [SerializeField] private ImageFeedBehaviour feed;
    
    // Start is called before the first frame update
    public void SubmitImage()
    {
        feed.AddNewImage(uriField.text);
        uriField.text = "";
    }
}
