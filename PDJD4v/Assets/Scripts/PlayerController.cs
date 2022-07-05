using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int coin = 0;
    public float moveSpeed;
    public float maxVelocity;
    public float jumpForce;
    public float rayDistance;

    public LayerMask groumdLayer;
    //public vector2 maxVelocity
    

    private GameControle _gameControle;
    private PlayerInput _playerInput;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;
    private Vector2 _moverInput;
    private bool isGround;

    private void OnEnable()
    {
        //inicializacao de variavel 
        _gameControle = new GameControle();

        //Referencias dos componentes no mesmo objeto da unity
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();

        //Referencias para a camera main guardada no classe camera
        _mainCamera = Camera.main;

        //atribuindo ao delegate do action triggerd no player input
        _playerInput.onActionTriggered += OnActionTriggerd;

    }

    private void OnDisable()
    {
        _playerInput.onActionTriggered -= OnActionTriggerd;
        
        
    }

    private void OnActionTriggerd(InputAction.CallbackContext obj)
    {
        if (obj.action.name.CompareTo(_gameControle.Gamepad.Moviment.name)== 0)
        {
            _moverInput = obj.ReadValue<Vector2>();
        }
        if (obj.action.name.CompareTo(_gameControle.Gamepad.Jump.name)== 0)
        {
            if (obj.performed) Jump();
        }
    }

    private void move()
    {
        Vector3 camFoward = _mainCamera.transform.forward;
        camFoward.y = 0;

        Vector3 moveVertical = camFoward * _moverInput.y;

        Vector3 camRight = _mainCamera.transform.right;

        camRight.y = 0;

        Vector3 moveHorizontal = camRight * _moverInput.x;
        
        // Adiciona a forca no objeto pelo rigidBody, com intensidade dada por moveSpeed
        _rigidbody.AddForce((moveVertical + moveHorizontal)* moveSpeed*Time.fixedDeltaTime);
    }

    private void LimeteVelocity()
    {
        //pegar a velocidade do player
        Vector3 velocity = _rigidbody.velocity;
        
        // checar se a velocidade esta dentro do limete em cada eixo 
        // limitando o eixo x usado ifs,Abs e Sin
        if (Mathf.Abs(velocity.x) > maxVelocity) velocity.x = Mathf.Sign(velocity.x) * maxVelocity;
        
        velocity.z = Mathf.Clamp(velocity.z, -maxVelocity, maxVelocity);

        _rigidbody.velocity = velocity;

    }

    private void Jump()
    {
        if(isGround) _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, rayDistance, groumdLayer);
    }

    private void Update()
    {
        CheckGround();
    }

    private void OnDrawGizmos()
    {
       Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.yellow); 
    }

    private void FixedUpdate()
    {
        move();
        LimeteVelocity();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Coin"))
        {
            coin++;
            Destroy(other.gameObject);
        }
    }
}
