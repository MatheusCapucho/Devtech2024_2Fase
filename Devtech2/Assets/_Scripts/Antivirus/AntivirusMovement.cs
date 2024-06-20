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

    // similar ao _inputTransform que tem no player
    private Transform _movementTransform;

    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask boxMask;
    [SerializeField] private LayerMask wallMask;

    [Header("Apenas -1 ou 1")]
    [SerializeField] private int _startingDirection = 1;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _dir = Vector2.right * _startingDirection;
        _movementTransform = transform.GetChild(0);
        _movementTransform.parent = null;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckGround();
        CheckForMovement();
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
        CheckWalls();
        _movementTransform.position = transform.position + (Vector3)_dir;
        _movementTransform.position = Vector3Int.FloorToInt(_movementTransform.position);
        var positionToInt = transform.position + (Vector3)_dir;
        _targetPosition = Vector3Int.FloorToInt(positionToInt);

        CheckForMovement();

        //Debug.Log(_targetPosition);
    }

    private void CheckForMovement()
    {
        if (_isGrounded)
        {
            Debug.Log("MEXEU");
            transform.position = Vector3.MoveTowards(transform.position, _movementTransform.position, Time.deltaTime * _speed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down * Time.deltaTime * _speed, Time.deltaTime * _speed);
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

    private void CheckWalls()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * _lastDirection, .6f, wallMask);

        if (hit.collider != null)
        {
            Flip();
        }
    }

    private void CheckGround()
    {
        RaycastHit2D hitL = Physics2D.Raycast(transform.position - new Vector3(0.44f, 0f, 0f), -transform.up, .55f, groundMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position + new Vector3(0.44f, 0f, 0f), -transform.up, .55f, groundMask);

        if ((hitL.collider != null || hitR.collider != null))
        {
            if (transform.position.x % 1 <= 0.5f)
                _targetPosition = Vector3Int.FloorToInt(transform.position);
            else
                _targetPosition = Vector3Int.CeilToInt(transform.position);

            //_inputTransform.position = _targetPosition;
            _isGrounded = true;
        }
        else
        {
            if (transform.position.x % 1 > 0.5f)
                _targetPosition = Vector3Int.CeilToInt(transform.position + Vector3.down * .55f);
            else
                _targetPosition = Vector3Int.FloorToInt(transform.position + Vector3.down * .55f);

            _movementTransform.position = _targetPosition + Vector3.up;
            _isGrounded = false;
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