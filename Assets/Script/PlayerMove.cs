using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Rigidbody rb;
    private float MoveSpeed = 5f;
    private float JumpForce = 5f; // ジャンプ力
    private bool isGrounded = true; // 地面にいるかどうか

    [Header("プレハブ生成設定")]
    public GameObject prefab; // 生成するプレハブ
    public Transform spawnPoint; // 生成する位置（指定の殻のオブジェクト）


    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 移動入力を取得
        var pos = _playerInput.actions["Move"].ReadValue<Vector2>();
        float runMultiplier = _playerInput.actions["Acceleration"].ReadValue<float>() > 0 ? 1.5f : 1f; // Runボタンを押している間は1.5倍
        Vector3 move = new Vector3(pos.x, 0, pos.y) * MoveSpeed * runMultiplier; // 速度を調整
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);

        // **プレイヤーの向きを移動方向に回転**
        if (move.magnitude > 0.01f) 
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.z));
        }

        // アニメーションの切り替え
        if (move.magnitude > 0.01f)
        {
            if (_playerInput.actions["Acceleration"].ReadValue<float>() > 0)
            {
                _animator.SetBool("Run", true);
                _animator.SetBool("Walk", false);
            }
            else
            {
                _animator.SetBool("Walk", true);
                _animator.SetBool("Run", false);
            }
        }
        else
        {
            _animator.SetBool("Walk", false);
            _animator.SetBool("Run", false);
        }

        // ジャンプ処理
        if (_playerInput.actions["Jump"].triggered && isGrounded)
        {
            float jumpMultiplier = _playerInput.actions["Acceleration"].ReadValue<float>() > 0 ? 1.5f : 1.2f;
            rb.AddForce(Vector3.up * JumpForce * jumpMultiplier, ForceMode.Impulse);
            isGrounded = false;
            _animator.SetBool("Jump", true);
        }

         // プレハブ生成処理
        if (_playerInput.actions["SpawnPrefab"].triggered)
        {
            SpawnPrefab();
        }
    }

     // プレハブを指定の位置に生成
    private void SpawnPrefab()
    {
        if (prefab != null && spawnPoint != null)
        {
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    // 地面に着いたかどうかの判定
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            _animator.SetBool("Jump", false);
        }
    }
}
