using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterController2D : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera mainCam;
    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire!");
        Debug.Log(context.action);
    }

    public void Collapse(InputAction.CallbackContext context) 
    {
        Debug.Log(context.ReadValue<Vector2>());
        Vector2 screenPos = context.ReadValue<Vector2>();
        Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y));

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
