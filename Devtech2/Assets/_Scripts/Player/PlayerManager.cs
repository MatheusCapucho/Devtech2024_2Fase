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
    private Vector3Int _targetPosition;
    private bool _isGrounded = false;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private LayerMask groundMask;

    private void Awake()
    {
        _input = new PlayerControls();
        _input.Enable();
        _inputTransform = transform.GetChild(0);
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

        if (!_isGrounded)
            return;

        Vector2 dir = ctx.ReadValue<Vector2>();
        if (dir == Vector2.zero || dir.y != 0f)
            return;

        _inputTransform.position = transform.position + (Vector3)dir;
        var positionToInt = transform.position + (Vector3)dir;
        _targetPosition = Vector3Int.FloorToInt(positionToInt);
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
        if (_isGrounded)
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);

    }

    private void PlaceBlock()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(0.4f, 0f, 0f), -transform.up, .55f, groundMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(0.4f, 0f, 0f), -transform.up, .55f, groundMask);

        if ((hitL.collider != null || hitR.collider != null))
        {
            _isGrounded = true;
            _targetPosition = Vector3Int.FloorToInt( transform.position);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(0.4f, 0f, 0f), -transform.up, .55f, groundMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(0.4f, 0f, 0f), -transform.up, .55f, groundMask);

        if ((hitL.collider == null || hitR.collider == null))
            _isGrounded = false;
    }
}
