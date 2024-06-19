using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private bool _isMoving = false;
    private bool _isGrounded = true;
    private Vector3 _targetPosition;
    [SerializeField] LayerMask groundMask;

    private void Awake()
    {
        _targetPosition = transform.position;
        _isMoving = false;
        _isGrounded = true;
    }
    public void Move(float dir)
    {
        _isMoving = true;
        _targetPosition = transform.position + new Vector3(dir, 0f, 0f);
       
    }

    private void Update()
    {
        CheckGround();


        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * 15f);
            if (transform.position == _targetPosition)
            {
                _isMoving = false;
            }
        }
        else
        if (!_isGrounded && !_isMoving)
        {
            transform.position += Vector3.down * Time.deltaTime * 15f;
        }


    }

    private void CheckGround()
    {
        RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(0.45f, 0f, 0f), -transform.up, .55f, groundMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(0.45f, 0f, 0f), -transform.up, .55f, groundMask);

        if ((hitL.collider != null || hitR.collider != null))
        {
            _isGrounded = true;
        } 
        else
        {
            _isGrounded = false;
        }
    }
}
