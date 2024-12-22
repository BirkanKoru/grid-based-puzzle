using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTester : MonoBehaviour
{
    [SerializeField] private bool infoDrag = true;
    [SerializeField] private bool infoClick = true;
    [SerializeField] private bool infoPosAndDelta = true;

    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(infoDrag)
        {
            if(inputManager.IsDragBegin) Debug.Log("<color=blue> Drag Begin </color>");
            if(inputManager.IsDrag) Debug.Log("<color=blue> Drag </color>");
            if(inputManager.IsDragEnd) Debug.Log("<color=blue> Drag End </color>");
        }

        if(infoClick)
        {
            if(inputManager.IsPointerDown) Debug.Log("<color=yellow> Pointer Down </color>");
            if(inputManager.IsPointerUp) Debug.Log("<color=yellow> Pointer Up </color>");
            if(inputManager.IsPointerClick) Debug.Log("<color=yellow> Pointer Click </color>");
        }

        if(infoPosAndDelta)
        {
            if(inputManager.IsDragBegin || inputManager.IsDrag || inputManager.IsDragEnd
            || inputManager.IsPointerDown || inputManager.IsPointerUp || inputManager.IsPointerClick)
              {
                Debug.Log("<color=green> Pointer Pos: " + inputManager.PointerPos + "</color>");
                Debug.Log("<color=green> Delta Pos: " + inputManager.DeltaPos + "</color>");
              }
        }
    }
}
