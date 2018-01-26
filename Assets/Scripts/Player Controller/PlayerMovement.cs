using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 10.0f;

    [SerializeField]
    private float _deadzone = 0.3f;

    [SerializeField]
    private Vector2 _startingDirection;

    private float _xInput;
    private float _yInput;
    private Vector2 _moveDirection;
    private Vector2 _lastMoveDirection;

    private Rigidbody2D _rb;
    private Animator _animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _lastMoveDirection = _startingDirection;
    }

    private void Update()
    {
        GetInput();
        SetMovementDirection();

        UpdateAnimation(_moveDirection);
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveDirection * _moveSpeed;
    }

    private void GetInput()
    {
        _xInput = Input.GetAxis("Horizontal");
        _yInput = Input.GetAxis("Vertical");
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
        if (moveDirection == Vector2.zero)
        {
            _animator.SetFloat("LastDirX", _lastMoveDirection.x);
            _animator.SetFloat("LastDirY", _lastMoveDirection.y);
            _animator.SetBool("Moving", false);
        } else
        {
            _lastMoveDirection = moveDirection;
            _animator.SetBool("Moving", true);
        }

        _animator.SetFloat("DirX", moveDirection.x);
        _animator.SetFloat("DirY", moveDirection.y);
    }
}
