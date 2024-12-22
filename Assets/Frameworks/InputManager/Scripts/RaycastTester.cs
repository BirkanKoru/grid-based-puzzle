using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTester : MonoBehaviour
{
    [SerializeField] private Transform pointer;
    [SerializeField] private LayerMask mask;

    private InputManager inputManager;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;

        pointer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(inputManager.IsPointerDown)
        {
            hit = inputManager.RaycastControl(mask);

            if(hit.collider != null)
            {
                pointer.gameObject.SetActive(true);
                pointer.transform.position = hit.point;

                Debug.Log("Hit: " + hit.transform.name);
            }
                
        }

        if(inputManager.IsDrag)
        {
            hit = inputManager.RaycastControl(mask);

            if(hit.collider != null) 
            {
                pointer.transform.position = hit.point;

                Debug.Log("Hit: " + hit.transform.name);
            }
                
        }

        if(inputManager.IsPointerUp)
        {
            pointer.gameObject.SetActive(false);

            hit = inputManager.RaycastControl(mask);

            if(hit.collider != null) 
            {
                pointer.transform.position = hit.point;
                
                Debug.Log("Hit: " + hit.transform.name);
            }
        }
    }
}
