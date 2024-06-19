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
    private float _lastInputDirection;
    private bool _isGrounded = false;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private LayerMask boxMask;
    [SerializeField]
    private GameObject _blockPrefab;
    private bool _isMoving = false;

    private void Awake()
    {
        _input = new PlayerControls();
        _input.Enable();
        _inputTransform = transform.GetChild(0);
        _inputTransform.parent = null;
    }
    private void OnEnable()
    {
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMoveCancelled;
        _input.Player.PlaceBlock.performed += OnPlaceBlock;
    }

  
    private void OnDisable()
    {
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMoveCancelled;
        _input.Player.PlaceBlock.performed -= OnPlaceBlock;
    }


    private void OnMove(InputAction.CallbackContext ctx)
    {

        if (!_isGrounded)
            return;

        Vector2 dir = ctx.ReadValue<Vector2>();
        if (dir == Vector2.zero || dir.y != 0f)
            return;
        _lastInputDirection = Mathf.Round(dir.x);
        _inputTransform.position = transform.position + (Vector3)dir;
        _inputTransform.position = Vector3Int.FloorToInt(_inputTransform.position);
        var positionToInt = transform.position + (Vector3)dir;
        _targetPosition = Vector3Int.FloorToInt(positionToInt);
    }


    private void OnMoveCancelled(InputAction.CallbackContext ctx)
    {
    }
    private void OnPlaceBlock(InputAction.CallbackContext ctx)
    {
        PlaceBlock();
    }

    private void Update()
    {
        CheckForMovement();
        CheckAndMoveBoxes();
    }

    private void CheckAndMoveBoxes()
    {
        RaycastHit2D hitL = Physics2D.Raycast(transform.position, -transform.right, .6f, boxMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position, transform.right, .6f, boxMask);

        if ((hitL.collider != null || hitR.collider != null))
        {
            if (hitL.collider != null)
            {
                hitL.collider.gameObject.GetComponent<Box>().Move(-1);
            }
            else
            {
                hitR.collider.gameObject.GetComponent<Box>().Move(1);
            }
        }
    }

    private void CheckForMovement()
    {
        if (_isGrounded)
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);

        if (transform.position == _targetPosition)
            _isMoving = false;
        else
            _isMoving = true;
            return;
  

    }

    private void PlaceBlock()
    {
        if (_isMoving)
            return;
        if (!_isGrounded)
            return;
        Instantiate(_blockPrefab, transform.position + Vector3.right * _lastInputDirection, transform.rotation, null);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Ground") || other.collider.CompareTag("Box"))
        {
            RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(0.5f, 0f, 0f), -transform.up, .55f, groundMask);
            RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0f, 0f), -transform.up, .55f, groundMask);

            if ((hitL.collider != null || hitR.collider != null))
            {
                _isGrounded = true;
                if(transform.position.x % 1 < 0.5f)
                    _targetPosition = Vector3Int.CeilToInt(transform.position);
                else
                    _targetPosition = Vector3Int.FloorToInt(transform.position);
                _inputTransform.position = _targetPosition;
            }
        }
      
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground") || other.collider.CompareTag("Box"))
        {
            RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(0.5f, 0f, 0f), -transform.up, .55f, groundMask);
            RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0f, 0f), -transform.up, .55f, groundMask);

            if ((hitL.collider == null || hitR.collider == null))
                _isGrounded = false;
        }
        
    }
}
