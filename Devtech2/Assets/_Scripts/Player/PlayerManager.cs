using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    private PlayerControls _input;

    private Transform _inputTransform;
    private Vector3 _targetPosition;
    private bool _isGrounded = true;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private LayerMask groundMask;

    private void Awake()
    {
        _input = new PlayerControls();
        _input.Enable();
        _inputTransform = transform.GetChild(0);
        _targetPosition = transform.position;
    }
    private void OnEnable()
    {
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMoveCancelled;
    }

  
    private void OnDisable()
    {
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMoveCancelled;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {

        if (!IsGrounded())
            return;

        Vector2 dir = ctx.ReadValue<Vector2>();
        if (dir == Vector2.zero || dir.y != 0f)
            return;

        _inputTransform.position = transform.position + (Vector3)dir;
        _targetPosition = transform.position + (Vector3)dir;
    }


    private void OnMoveCancelled(InputAction.CallbackContext ctx)
    {
    }

    private void Update()
    {
        CheckForMovement();
    }

    private void CheckForMovement()
    {
        if (IsGrounded())
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _speed);
        else
            _targetPosition = transform.position;

    }

    private bool IsGrounded()
    {
        RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(0.6f, 0f, 0f), -transform.up, .55f, groundMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(0.6f, 0f, 0f), -transform.up, .55f, groundMask);

        if ((hitL.collider != null || hitR.collider != null))
            return true;

        return false;
    }

    private void PlaceBlock()
    {

    }
}
