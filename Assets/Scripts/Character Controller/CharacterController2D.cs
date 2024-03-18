using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class CharacterController2D : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] Camera mainCam;
    private Vector3 m_PlayerVelocity;
    private Vector2 m_MoveInput;
    private Transform m_Transform;
    [SerializeField] private WFC_TerrainGeneration m_WFC;

    private void ResetVelocity ()
    {
        m_PlayerVelocity.x = 0f;
        m_PlayerVelocity.y = 0f;
        m_PlayerVelocity.z = 0f;
        m_MoveInput.x = 0f;
        m_MoveInput.y = 0f;
    }
    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire!");
        Debug.Log(context.action);
    }

    void Start()
    {
        m_Transform = this.GetComponent<Transform>();
        m_PlayerVelocity = new();
        m_MoveInput = new();
    }

    // Update is called once per frame
    void Update()
    {
        m_PlayerVelocity.x = m_MoveInput.x;
        m_PlayerVelocity.y = m_MoveInput.y;
        m_Transform.position += m_PlayerVelocity * Time.deltaTime;
    }

    public void MoveCallback (InputAction.CallbackContext context)
    {
        InputControl control = context.control;
        m_MoveInput = context.ReadValue<Vector2>();
        m_WFC.CollapseTile(m_Transform.position);
        if (context.canceled)
        {
            ResetVelocity();
        }
    }
}
