using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
        
    private GameControle _gameControle;
    private PlayerInput _playerInput;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;

    private Vector2 moverInput;

    private void OnEnable()
    {
        //inicializacao de variavel 
        _gameControle = new GameControle();

        //Referencias dos componentes no mesmo objeto da unity
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();

        //Referencias para a camera main guardada no classe camera
        _mainCamera = Camera.main;

        //articulando ao delegate do action triggerd no player input
        _playerInput.onActionTriggered += OnActionTiggered;

    }

    private void OnDisable()
    {
        _playerInput.onActionTriggered -= OnActionTiggered;
        
    }

    private void OnActionTiggered(InputAction.CallbackContext obj)
    {
        //comecando o nome do action que este chegado com o nome do action de moviment
        if (obj.action.name.CompareTo(_gameControle.Gamepad.Moviment.name) == 0)
        {
            
            //atribuir ao moveinput o valor proveniente ao input do jogador com um vector2
            moverInput = obj.ReadValue<Vector2>();


        }
    }

    private void move()
    {
        Vector3 camFoward = _mainCamera.transform.forward;
        camFoward.y = 0;

        Vector3 moveVertical = camFoward * _moveInput.y;

        Vector3 camRight = _mainCamera.transform.right;

        camRight.y = 0;

        Vector3 moveHorizontal = camRight * _moveInput.x;
        
        // Adiciona a forca no objeto pelo rigidBody, com intensidade dada por moveSpeed
        _rigidbody.AddForce((moveVertical + moveHorizontal)* moveSpeed*Time.fixedDeltaTime);
    }

    private void LimeteVelocity()
    {
        //pegar a velocidade do player
        Vector3 velocity = _rigidbody.velocity;
        
        // checar se a velocidade esta dentro do limete em cada eixo 
        // limitando o eixo x usado ifs,Abs e Sin
        if (Mathf.Abs(velocity.x))
        {
            
        }
        move();
    }
}
