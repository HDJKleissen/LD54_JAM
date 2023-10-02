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

    public float rotationInput;

    public Vector2 input;
    private Vector2 previousInput;

    private FMOD.Studio.EventInstance movementSound;

    [SerializeField] private PlayerGas gasTracker;
    // Start is called before the first frame update
    void Start()
    {
        movementSound = FMODUnity.RuntimeManager.CreateInstance("event:/Engine");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Honk"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Truck Horn");
        }
        if (Input.GetButtonUp("Honk"))
        {
            // Stop Horn Sound?
        }

        Vector2 velocity = _rigidbody.velocity;
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float rotationInput = input.x * velocity.magnitude * _rotateVelocityRatio;
        gasTracker.ReduceGas(Mathf.Clamp(Mathf.Abs(input.y) + Mathf.Abs(rotationInput), 0.001f, 1));
    }

    public void IncreaseMaxMovementSpeed(float amount)
    {
        _maxMoveSpeed += amount;
    }

    public void IncreaseBreakSpeed(float amount)
    {
        _activeBrakeSpeed += amount;
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _rigidbody.velocity;

        float accelerationInput = input.y;
        rotationInput = input.x * velocity.magnitude * _rotateVelocityRatio;
        float moveSpeed = velocity.magnitude;

        if (accelerationInput > 0)
        {
            if(previousInput.y == 0)
            {
                //SFX: Acceleration start (stop everything else)
                movementSound.start();
            }
            // Player is accelerating
            moveSpeed += _accelerationSpeed * Time.deltaTime;
        }
        else if (accelerationInput < 0)
        {
            if (previousInput.y >= 0)
            {
                //SFX: Active brake start (stop everything else)
                movementSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                if (velocity.magnitude >= 0.1f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Brake");
                }
            }
            // Player is actively braking
            moveSpeed -= _activeBrakeSpeed * Time.deltaTime;
        }
        else
        {
            if (previousInput.y != 0)
            {
                //SFX: Passive brake start (stop everything else)
                movementSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            // Player is letting vehicle passively brake
            moveSpeed -= _defaultBrakeSpeed * Time.deltaTime;
        }
        
        _rigidbody.rotation -= rotationInput * _rotateSpeed * Time.deltaTime;

        moveSpeed = Mathf.Clamp(moveSpeed, 0, _maxMoveSpeed);

        _rigidbody.velocity = transform.up * moveSpeed;
        previousInput = input;
    }

    private void OnDestroy()
    {
        movementSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        movementSound.release();
    }
}
