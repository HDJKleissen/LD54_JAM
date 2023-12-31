using Freya;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PirateMovement : Hazard, IDamageable
{
    protected override DamageSource damageSource => DamageSource.Pirate;

    [SerializeField] private float _accelerationSpeed;
    [SerializeField] private float _defaultBrakeSpeed;
    [SerializeField] private float _activeBrakeSpeed;
    [SerializeField] private float _maxMoveSpeed;

    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _rotateVelocityRatio;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public float rotationInput;

    private FMOD.Studio.EventInstance movementSound;

    public Vector2 input;
    private Vector2 previousInput;

    public float health = 15;
    [SerializeField] float chaseTime = 0;
    [SerializeField] float destroyTime = 0;
    [SerializeField] float timer = 0;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] private PlayerMovement playerMovement;

    bool exploded;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        movementSound = FMODUnity.RuntimeManager.CreateInstance("event:/PirateEngine");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(movementSound, transform, GetComponent<Rigidbody2D>());

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float dotProd = Vector2.Dot((playerMovement.transform.position - transform.position).normalized, transform.right); 
        if (timer > chaseTime || playerMovement.isNearPlanet)
        {
            dotProd = -dotProd;

        }
        if ((timer > destroyTime || playerMovement.isNearPlanet) && !_spriteRenderer.isVisible)
        {
            movementSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            Destroy(gameObject);
        }

        input = new Vector2(dotProd, 1);
    }


    private void FixedUpdate()
    {
        if (health > 0)
        {
            Vector2 velocity = _rigidbody.velocity;

            float accelerationInput = input.y;
            rotationInput = input.x * velocity.magnitude * _rotateVelocityRatio;
            float moveSpeed = velocity.magnitude;

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


    public void Damage(float amount, DamageSource source)
    {
        switch (source)
        {
            case DamageSource.None:
                break;
            case DamageSource.Asteroid:
                if (amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Asteroid Impact", gameObject);
                    // SFX: Very Hard asteroid hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Asteroid Impact", gameObject);
                    // SFX: Hard asteroid hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Asteroid Impact", gameObject);
                    // SFX: Medium asteroid hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Asteroid Impact", gameObject);
                    // SFX: Light asteroid hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Asteroid Impact", gameObject);
                    // SFX: Very Light asteroid hit
                }
                break;
            case DamageSource.Pirate:
                if (amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pirate Impact", gameObject);
                    // SFX: Very Hard Pirate hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pirate Impact", gameObject);
                    // SFX: Hard Pirate hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pirate Impact", gameObject);
                    // SFX: Medium Pirate hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pirate Impact", gameObject);
                    // SFX: Light Pirate hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pirate Impact", gameObject);
                    // SFX: Very Light Pirate hit
                }
                break;
            case DamageSource.Tumbleweed:
                if (amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Tumbleweed Impact", gameObject);
                    // SFX: Very Hard Tumbleweed hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Tumbleweed Impact", gameObject);
                    // SFX: Hard Tumbleweed hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Tumbleweed Impact", gameObject);
                    // SFX: Medium Tumbleweed hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Tumbleweed Impact", gameObject);
                    // SFX: Light Tumbleweed hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Tumbleweed Impact", gameObject);
                    // SFX: Very Light Tumbleweed hit
                }
                // SFX: Tumbleweed hit, can use amount for intensity or sth
                break;
            default:
                if (amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Carriage Impact", gameObject);
                    // SFX: Very Hard Default hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Carriage Impact", gameObject);
                    // SFX: Hard Default hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Carriage Impact", gameObject);
                    // SFX: Medium Default hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Carriage Impact", gameObject);
                    // SFX: Light Default hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Carriage Impact", gameObject);
                    // SFX: Very Light Default hit
                }
                // SFX: Default hit, can use amount for intensity or sth
                break;
        }
        health -= amount;
        if(health < 0)
        {
            movementSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            if (!exploded)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Explosion", gameObject);
                exploded = true;
                Transform expl = Instantiate(explosionPrefab).transform;
                expl.position = transform.position;
            }
            _spriteRenderer.color = new Color(.5f, .5f, .5f);
        }
    }
}
