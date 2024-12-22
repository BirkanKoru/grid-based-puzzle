using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public bool IsDragBegin     { get; set; }
    public bool IsDrag          { get; set; }
    public bool IsDragEnd       { get; set; }

    public bool IsPointerDown   { get; set; }
    public bool IsPointerUp     { get; set; }
    public bool IsPointerClick  { get; set; }

    public Vector3 PointerPos   { get; set; }
    public Vector3 DeltaPos     { get; set; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        PointerPos = eventData.position;
        DeltaPos = eventData.delta;

        IsDragBegin = true;        
    }

    public void OnDrag(PointerEventData eventData)
    {
        PointerPos = eventData.position;
        DeltaPos = eventData.delta;

        IsDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PointerPos = eventData.position;
        DeltaPos = eventData.delta;

        IsDrag = false;
        IsDragEnd = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerPos = eventData.position;
        DeltaPos = eventData.delta;

        IsPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerPos = eventData.position;
        DeltaPos = eventData.delta;

        IsPointerUp = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerPos = eventData.position;
        DeltaPos = eventData.delta;

        IsPointerClick = true;    
    }
    
    private void LateUpdate() 
    {
        if(IsDragBegin) IsDragBegin = false;
        if(IsDragEnd) IsDragEnd = false;

        if(IsPointerDown) IsPointerDown = false;
        if(IsPointerUp) IsPointerUp = false;
        if(IsPointerClick) IsPointerClick = false;
    }

    #region EXTRA FUNCTIONS
    
    public RaycastHit RaycastControl(LayerMask mask)
    {
        RaycastHit hit;

        Ray ray = Camera.ScreenPointToRay(PointerPos);
        Physics.Raycast(ray, out hit, Mathf.Infinity, mask);

        return hit;
    }

    public RaycastHit2D RaycastControl2D(LayerMask mask)
    {
        // Create a ray from the camera through the screen point defined by the input manager's pointer position.
        Ray ray = Camera.ScreenPointToRay(PointerPos);
        
        // Convert the ray to a 2D ray starting point and direction.
        Vector2 rayOrigin = ray.origin;
        Vector2 rayDirection = ray.direction;

        // Perform the raycast in 2D.
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, Mathf.Infinity, mask);

        // Return the RaycastHit2D object, which contains information about the hit (if any).
        return hit;
    }

    #endregion

    #region START

    //SINGLETON
    private static InputManager instance;
    public static InputManager Instance { get { return instance; }}

    private void Awake() 
    {
        instance = this;    
    }

    [SerializeField] private bool startWithMainCam = true;
    private Canvas canvas;
    public Camera Camera { get; set; }

    private void Start() 
    {
        if(startWithMainCam)
            SetCamera(Camera.main);
    }

    public void SetCamera(Camera cam)
    {
        if(canvas == null)
            canvas = GetComponent<Canvas>();

        if(canvas == null)
        {
            Debug.LogError("Not Found Canvas!");
            return;
        }

        if(cam == null)
        {
            Debug.LogError("Not Found Camera!");
            return;
        }

        Camera = cam;
        canvas.worldCamera = cam;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }

    #endregion
}
