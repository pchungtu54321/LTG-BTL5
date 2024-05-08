// using System;
// using System.Net.NetworkInformation;
// using UnityEngine;
// using UnityEngine.PlayerLoop;
//
// namespace Enemy
// {
//     public class EnemyBehaviour : MonoBehaviour
//     {
//         public int hp = 3;
//         public int damage = 1;
//         [SerializeField] private float speed = 1f;
//         [SerializeField] private float attackRange = 10f;
//         [SerializeField] private float triggerRange = 15f;
//         [SerializeField] private float attackCd = 1f;
//         private float _attackCdTimer = 0f;
//         
//         private Animator _animator;
//         private Transform _playerTransform;
//         private Rigidbody _rb;
//         private bool _isAttacking = false;
//         private bool _isTriggered = false;
//         private bool _isDead = false;
//         private void Start()
//         {
//             _animator = GetComponent<Animator>();
//             _rb = GetComponent<Rigidbody>();
//             _playerTransform = GameObject.FindWithTag("Player").transform;
//             _attackCdTimer = attackCd;
//         }
//
//         private void FixedUpdate()
//         {
//             var players = GameObject.FindGameObjectsWithTag("Player");
//             // Find nearest player
//             if (players.Length > 0 && _isTriggered == false)
//             {
//                 foreach (var player in players)
//                 {
//                     var distance = Vector3.Distance(transform.position, player.transform.position);
//                     if (distance <= triggerRange)
//                     {
//                         _isTriggered = true;
//                         _playerTransform = player.transform;
//                         break;
//                     }
//                     else
//                     {
//                         Debug.Log("No player in range");
//                         _isTriggered = false;
//                     }
//                 }
//             }
//
//             if (_isTriggered) TriggerEnemy();
//         }
//
//         private void TriggerEnemy()
//         {
//             var distance = Vector3.Distance(transform.position, _playerTransform.position);
//             if (distance <= attackRange)
//             {
//                 _isAttacking = true;
//                 _animator.SetTrigger("Attack");
//                 _rb.velocity = Vector3.zero;
//                 _rb.angularVelocity = Vector3.zero;
//                 _attackCdTimer -= Time.fixedDeltaTime;
//                 if (_attackCdTimer <= 0)
//                 {
//                     _attackCdTimer = attackCd;
//                     // _playerTransform.GetComponent<Player.PlayerBehaviour>().TakeDamage(damage);
//                 }
//             }
//             else
//             {
//                 _isAttacking = false;
//                 _animator.SetBool("isAttacking", false);
//                 _animator.SetBool("isRunning", true);
//                 // _rb.velocity = transform.forward * speed;
//                 transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, speed * Time.fixedDeltaTime);
//             }
//         }
//         public void TakeDamage(int damage)
//         {
//             hp -= damage;
//             if (hp <= 0)
//             {
//                 _isDead = true;
//                 _animator.SetBool("isDead", true);
//                 Destroy(gameObject, 1f);
//             }
//             else
//             {
//                 _animator.SetTrigger("Hit");
//             }
//         }
//     }
// }