using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5.0f;

    [SerializeField]
    private float _dashSpeed = 8.0f;

    [SerializeField]
    private float _deadzone = 0.3f;

    [SerializeField]
    private Vector2 _startingDirection;

    private float _xInput;
    private float _yInput;
    private bool _dashInput;
    private bool _dashing = false;
    private float _dashTime;
    [SerializeField]
    private float DASH_TIME;
    private Vector2 _moveDirection;
    private Vector2 _lastMoveDirection;

    private Rigidbody2D _rb;
    private Animator _animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _lastMoveDirection = _startingDirection;
        _animator.SetFloat("LastDirX", _lastMoveDirection.x);
        _animator.SetFloat("LastDirY", _lastMoveDirection.y);
    }

    private void Update()
    {
        GetInput();
        SetMovementDirection();

        UpdateAnimation(_moveDirection);

        if (_dashing && _dashTime > 0f)
        {
            _dashTime -= Time.deltaTime;
        } else
        {
            _dashing = false;
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveDirection * _moveSpeed;

        if (_dashing)
        {
            _rb.velocity = _lastMoveDirection * _dashSpeed;
        }
    }

    private void GetInput()
    {
        _dashInput = Input.GetButtonDown("Dash");

        if(!_dashInput && !_dashing)
        {
            _xInput = Input.GetAxis("Horizontal");
            _yInput = Input.GetAxis("Vertical");
        } else if (!_dashing)
        {
            _dashing = true;
            _dashTime = DASH_TIME;
        }
    }

    private void SetMovementDirection()
    {
        _moveDirection = new Vector2(_xInput, _yInput);

        CleanMovementDirectionVector();
    }

    private void CleanMovementDirectionVector()
    {
        if (_moveDirection.magnitude > 1)
        {
            _moveDirection.Normalize();
        } else if (_moveDirection.magnitude < _deadzone)
        {
            _moveDirection = Vector2.zero;
        }
    }

    private void UpdateAnimation(Vector2 moveDirection)
    {

        if (_dashing)
        {
            _animator.SetBool("Dashing", true);
        }
        else
        {
            _animator.SetBool("Dashing", false);
        }

        if (moveDirection == Vector2.zero)
        {
            _animator.SetBool("Moving", false);
        } else
        {
            _lastMoveDirection = moveDirection;
            _animator.SetFloat("LastDirX", _lastMoveDirection.x);
            _animator.SetFloat("LastDirY", _lastMoveDirection.y);
            _animator.SetBool("Moving", true);
        }

        _animator.SetFloat("DirX", moveDirection.x);
        _animator.SetFloat("DirY", moveDirection.y);
    }
}
