using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerManager : MonoBehaviour
{
    private PlayerControls _input;

    private Transform _inputTransform;
    private Vector3Int _targetPosition;
    private Vector2 _lastInputDirection;
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
        _lastInputDirection = dir;
        CheckAndMoveBoxes();
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
        CheckGround();
        CheckForMovement();
    }

    private void CheckGround()
    {
        RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(0.44f, 0f, 0f), -transform.up, .55f, groundMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(0.44f, 0f, 0f), -transform.up, .55f, groundMask);

        if ((hitL.collider != null || hitR.collider != null))
        {
            if (transform.position.x % 1 < 0.5f)
                _targetPosition = Vector3Int.CeilToInt(transform.position);
            else
                _targetPosition = Vector3Int.FloorToInt(transform.position);

            //_inputTransform.position = _targetPosition;
            _isGrounded = true;

        } else
        {
            if (transform.position.x % 1 > 0.5f)
                _targetPosition = Vector3Int.CeilToInt(transform.position + Vector3.down * .55f);
            else
                _targetPosition = Vector3Int.FloorToInt(transform.position + Vector3.down * .55f);

            _inputTransform.position = _targetPosition + Vector3.up;
            _isGrounded = false;
        }
    }

    private void CheckAndMoveBoxes()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * _lastInputDirection.x, .6f, boxMask);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<Box>().Move(_lastInputDirection.x);
        }
          
        
    }

    private void CheckForMovement()
    {

        if (PathIsBlocked() && _isGrounded)
            return;

        if (_isGrounded)
        {
            transform.position = Vector3.MoveTowards(transform.position, _inputTransform.position, Time.deltaTime * _speed);

        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down * Time.deltaTime * _speed, Time.deltaTime * _speed);
            _lastInputDirection = Vector2.down;

        }

        if (transform.position == _targetPosition)
            _isMoving = false;
        else
            _isMoving = true;
            return;
  

    }

    private bool PathIsBlocked()
    {
        RaycastHit2D hit = Physics2D.Raycast(_inputTransform.position, _lastInputDirection, .1f, groundMask);

        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    private void PlaceBlock()
    {
        if (_isMoving)
            return;
        if (!_isGrounded)
            return;
        Instantiate(_blockPrefab, transform.position + Vector3.right * _lastInputDirection.x, transform.rotation, null);
    }

   
}
