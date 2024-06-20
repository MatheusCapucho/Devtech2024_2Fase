using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntivirusMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _movement;
    private bool _isGrounded = false;
    private bool _isMoving = false;
    private float _lastDirection;
    private Vector2 _dir;
    private Vector3Int _targetPosition;

    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask boxMask;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _dir = Vector2.right;
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (!_isGrounded)
            return;
        if (_dir == Vector2.zero || _dir.y != 0f)
            return;
        _lastDirection = Mathf.Round(_dir.x);
        CheckAndMoveBoxes();
        var positionToInt = transform.position + (Vector3)_dir;
        _targetPosition = Vector3Int.FloorToInt(positionToInt);

        CheckForMovement();

        //Debug.Log(_targetPosition);
    }

    private void CheckForMovement()
    {
        if (_isGrounded)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
        }
        if (transform.position == _targetPosition)
            _isMoving = false;
        else
            _isMoving = true;
        return;
    }

    private void CheckAndMoveBoxes()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * _lastDirection, .6f, boxMask);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<Box>().Move(new Vector2(_lastDirection, 0f));
            Flip();
        }
    }

    private void Flip()
    {
        Debug.Log("Flip");
        _dir.x *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision Enter");
        Debug.Log(_isGrounded);
        if (other.gameObject.CompareTag("Wall"))
        {
            Flip();
        }

        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Box"))
        {
            RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(0.45f, 0f, 0f), -transform.up, .55f, groundMask);
            RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(0.45f, 0f, 0f), -transform.up, .55f, groundMask);

            if ((hitL.collider != null || hitR.collider != null))
            {
                Debug.Log("Grounded");
                _isGrounded = true;
                if (transform.position.x % 1 < 0.5f)
                    _targetPosition = Vector3Int.CeilToInt(transform.position);
                else
                    _targetPosition = Vector3Int.FloorToInt(transform.position);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(0.5f, 0f, 0f), -transform.up, .55f, groundMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0f, 0f), -transform.up, .55f, groundMask);

        if ((hitL.collider == null || hitR.collider == null))
            _isGrounded = false;
    }
}