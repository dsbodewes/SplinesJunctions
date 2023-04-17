using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VertexMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool buttonPressed;
    public bool hasChanged;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(buttonPressed)
        {
            transform.position = Input.mousePosition;
            hasChanged = true;
        }
    }
 
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
