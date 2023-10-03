using Freya;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Hazard, IDamageable
{
    protected override DamageSource damageSource => DamageSource.Player;

    [SerializeField] private float _accelerationSpeed;
    [SerializeField] private float _defaultBrakeSpeed;
    [SerializeField] private float _activeBrakeSpeed;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _idlingGasLoss = 0.01f;

    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _rotateVelocityRatio;
    [SerializeField] private float minSpeedWithOpenWagons = 0.4f;
    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] GameObject explosionPrefab;
    public float rotationInput;

    public Vector2 input;
    private Vector2 previousInput;

    private FMOD.Studio.EventInstance movementSound;
    bool exploded;
    public float health = 100;
    float maxHealth = 100;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private PlayerGas gasTracker;
    [SerializeField] private Slider healthSlider;

    public bool isNearPlanet;
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

        inventoryManager.SetTrainIsMoving(velocity.magnitude > minSpeedWithOpenWagons);

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float rotationInput = input.x * velocity.magnitude * _rotateVelocityRatio;

        gasTracker.ReduceGas(Mathf.Clamp((_rigidbody.velocity.y < 0.1f ? 0 : Mathf.Abs(input.y)) + Mathf.Abs(rotationInput), _idlingGasLoss, 1));
        
        healthSlider.value = health / maxHealth;
    }

    public void IncreaseMaxMovementSpeed(float amount)
    {
        _maxMoveSpeed += amount;
    }

    public void IncreaseBreakSpeed(float amount)
    {
        _activeBrakeSpeed += amount;
    }
    public void IncreaseMaxFuel(float amount)
    {
        gasTracker.maxGas += amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // :')
        if(collision.name.Contains("Planet"))
        {
            isNearPlanet = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // :')
        if (collision.name.Contains("Planet"))
        {
            isNearPlanet = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _rigidbody.velocity;

        float accelerationInput = input.y;
        rotationInput = input.x * velocity.magnitude * _rotateVelocityRatio;
        float moveSpeed = velocity.magnitude;

        if (inventoryManager.TrainCanBeClosed())
        {
            if (accelerationInput > 0)
            {
                if (previousInput.y == 0)
                {
                    //SFX: Acceleration start (stop everything else)
                    movementSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
    }

    private void OnDestroy()
    {
        movementSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        movementSound.release();
    }

    internal void RepairFull()
    {
        health = maxHealth;
    }

    public void Damage(float amount, DamageSource source)
    {
        switch (source)
        {
            case DamageSource.None:
                break;
            case DamageSource.Asteroid:
                if (amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Asteroid Impact");
                    // SFX: Very Hard asteroid hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Asteroid Impact");
                    // SFX: Hard asteroid hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Asteroid Impact");
                    // SFX: Medium asteroid hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Asteroid Impact");
                    // SFX: Light asteroid hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Asteroid Impact");
                    // SFX: Very Light asteroid hit
                }
                break;
            case DamageSource.Pirate:
                if (amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Pirate Impact");
                    // SFX: Very Hard Pirate hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Pirate Impact");
                    // SFX: Hard Pirate hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Pirate Impact");
                    // SFX: Medium Pirate hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Pirate Impact");
                    // SFX: Light Pirate hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Pirate Impact");
                    // SFX: Very Light Pirate hit
                }
                break;
            case DamageSource.Tumbleweed:
                if (amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Tumbleweed Impact");
                    // SFX: Very Hard Tumbleweed hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Tumbleweed Impact");
                    // SFX: Hard Tumbleweed hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Tumbleweed Impact");
                    // SFX: Medium Tumbleweed hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Tumbleweed Impact");
                    // SFX: Light Tumbleweed hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Tumbleweed Impact");
                    // SFX: Very Light Tumbleweed hit
                }
                // SFX: Tumbleweed hit, can use amount for intensity or sth
                break;
            default:
                if (amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Carriage Impact");
                    // SFX: Very Hard Default hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Carriage Impact");
                    // SFX: Hard Default hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Carriage Impact");
                    // SFX: Medium Default hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Carriage Impact");
                    // SFX: Light Default hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Carriage Impact");
                    // SFX: Very Light Default hit
                }
                // SFX: Default hit, can use amount for intensity or sth
                break;
        }
        health -= amount;
        if(health < 0)
        {
            movementSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            GetComponent<PlayerMovement>().enabled = false;
            if (!exploded)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Explosion");
                exploded = true;
                Transform expl = Instantiate(explosionPrefab).transform;
                expl.position = transform.position;
                FindObjectOfType<MenuButtons>().OpenGameOverScreen();
            }
        }
    }
}
