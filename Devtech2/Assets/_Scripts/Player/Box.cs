using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Box : MonoBehaviour
{
    private bool _isMoving = false;
    private bool _isMovingDown = false;
    private bool _isGrounded = true;
    private Vector3 _targetPosition;
    [SerializeField] LayerMask groundMask;

    private Tilemap tilemap;

    private Vector2 _direction = Vector2.zero;

    private void Awake()
    {
        _targetPosition = transform.position;
        _isMoving = false;
        _isGrounded = true;
    }
    public void Move(Vector2 dir)
    {
        _isMoving = true;
        if(_direction != Vector2.down)
            _direction = new Vector3(dir.x, dir.y, 0f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * .2f, _direction, 20f, groundMask);
        Vector3Int targetOffset = Vector3Int.CeilToInt(new Vector3(dir.x, dir.y, 0f));


        if(hit.collider != null)
        {
            tilemap = hit.collider.GetComponent<Tilemap>();
            var hitPoint = tilemap.WorldToCell(hit.point);
            if(hitPoint.x > transform.position.x)
            {
                targetOffset = Vector3Int.FloorToInt(new Vector3(hitPoint.x - transform.position.x, 0f, 0f));
            }
            else
            if(hitPoint.x < transform.position.x)
            {
                targetOffset = Vector3Int.FloorToInt(new Vector3(hitPoint.x - transform.position.x + 1f, 0f, 0f));
            }
        } else
        {
            targetOffset = Vector3Int.CeilToInt(_direction * 100f);
        }
        _targetPosition +=  targetOffset;
        _isMovingDown = true;
       
    }

    private void FixedUpdate()
    {
        CheckGround();


        CheckAndMove();


    }

    private void CheckAndMove()
    {
        if (_isMoving && _isGrounded)
        {
            if (transform.position == _targetPosition)
            {
                if (_direction != Vector2.zero && _isGrounded)
                    Move(_direction);
                else
                    _isMoving = false;
            }
            if(_targetPosition.y < transform.position.y)
                _targetPosition.y = transform.position.y;

            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.fixedDeltaTime * 15f);
        }
        else
        if (!_isGrounded)
        {

            if (_isMovingDown)
                transform.position += 15f * Time.fixedDeltaTime * Vector3.down;

            //transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * 15f);
        }
    }

    private void CheckGround()
    {
        RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(0.45f, 0f, 0f), -transform.up, .55f, groundMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(0.45f, 0f, 0f), -transform.up, .55f, groundMask);

        if ((hitL.collider != null || hitR.collider != null))
        {
            if(!_isGrounded)
            {
                if (transform.position.x % 1 < 0.5f)
                    _targetPosition = Vector3Int.FloorToInt(transform.position - new Vector3(.1f, .1f, 0f));
                else
                    _targetPosition = Vector3Int.CeilToInt(transform.position + new Vector3(.1f, .1f, 0f));
            }
            _isGrounded = true;
        } 
        else
        {
            _isGrounded = false;
        }
    }
}
