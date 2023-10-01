using Freya;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _accelerationSpeed;
    [SerializeField] private float _defaultBrakeSpeed;
    [SerializeField] private float _activeBrakeSpeed;
    [SerializeField] private float _maxMoveSpeed;

    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _rotateVelocityRatio;

    [SerializeField] private Rigidbody2D _rigidbody;

    private float rotation;

    private Vector2 input;
    private Vector2 previousInput;

    [SerializeField] private PlayerGas gasTracker;
    // Start is called before the first frame update
    void Start()
    {
        rotation = _rigidbody.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        previousInput = input;
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        gasTracker.ReduceGas(Mathf.Clamp(Mathf.Abs(input.x) + Mathf.Abs(input.y), 0, 1));
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _rigidbody.velocity;

        float accelerationInput = input.y;
        float rotationInput = input.x * velocity.magnitude * _rotateVelocityRatio;
        float moveSpeed = velocity.magnitude;

        if (accelerationInput > 0)
        {
            if(previousInput.y == 0)
            {
                //SFX: Acceleration start (stop everything else)
            }
            // Player is accelerating
            moveSpeed += _accelerationSpeed * Time.deltaTime;
        }
        else if (accelerationInput < 0)
        {
            if (previousInput.y >= 0)
            {
                //SFX: Active brake start (stop everything else)
            }
            // Player is actively braking
            moveSpeed -= _activeBrakeSpeed * Time.deltaTime;
        }
        else
        {
            if (previousInput.y != 0)
            {
                //SFX: Passive brake start (stop everything else)
            }
            // Player is letting vehicle passively brake
            moveSpeed -= _defaultBrakeSpeed * Time.deltaTime;
        }
        
        _rigidbody.rotation -= rotationInput * _rotateSpeed * Time.deltaTime;

        moveSpeed = Mathf.Clamp(moveSpeed, 0, _maxMoveSpeed);

        _rigidbody.velocity = transform.up * moveSpeed;
    }
}
