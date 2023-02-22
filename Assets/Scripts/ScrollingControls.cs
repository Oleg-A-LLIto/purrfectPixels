using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IScrollable))] //[SerializeReference] still does not work with interfaces in unity, so we'll have to make do with just this
public class ScrollingControls : MonoBehaviour
{
    [SerializeField] private bool useScrolling;
    [SerializeField] private float scrollingSensitivity;
    [SerializeField] private bool useGrabbing;
    private IScrollable scrollTarget;
    private Vector3 lastMousePosition;

    void Start()
    {
        scrollTarget = GetComponent<IScrollable>();
    }

    void ProcessGrabbingControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            scrollTarget.Scroll((Input.mousePosition - lastMousePosition).y);
            lastMousePosition = Input.mousePosition;
        }
        
    }

    void ProcessScrollingControls()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            scrollTarget.Scroll(Input.mouseScrollDelta.y * scrollingSensitivity);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if(useGrabbing)
            ProcessGrabbingControls();
        if (useScrolling)
            ProcessScrollingControls();
    }
}
