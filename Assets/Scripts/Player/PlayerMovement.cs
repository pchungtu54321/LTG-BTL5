using Cinemachine;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        public CharacterController controller;
        [SerializeField] private CinemachineFreeLook freeLookCamera;
        public GameObject bossPrefab;
        
        public float speed = 3f;
        public float turnSmoothTime = 0.1f;
        public float turnSmoothVelocity;
        
        private Transform _mainCamera;
        private Animator _animator;
        private NetworkCommunicator _network;

        private void Start()
        {
            _mainCamera = GameObject.FindWithTag("MainCamera").transform;
            _animator = GetComponent<Animator>();
            _network = GetComponent<NetworkCommunicator>();
        }
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                controller.enabled = false;
                transform.position = new Vector3(0, 0, 0);
                controller.enabled = true;
            }
            else
            {
                controller.enabled = false;
            }
            if (!IsOwner)
            {
                enabled = false;
                freeLookCamera.Priority = 0;
            }
            else
            {
                freeLookCamera.Priority = 10;
                // freeLookCamera.Follow = transform;
            }
        }
        public void DestroyPlayer()
        {
            DestroyPlayerServerRpc();
        }

        [ServerRpc]
        public void DestroyPlayerServerRpc()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            var direction = new Vector3(horizontal, 0f, vertical).normalized;
            
            if(direction.magnitude >= 0.1f)
            {
                // Rotate the player to face the direction of movement.
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
                // var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                // transform.rotation = Quaternion.Euler(0f, angle, 0f);
                _network.RotatePlayerServerRpc(Quaternion.Euler(0f, targetAngle, 0f));
                
                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                // controller.Move(moveDir.normalized * (speed * Time.deltaTime));
                _network.MovePlayerServerRpc(moveDir.normalized * (speed * Time.deltaTime));
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SpawnEnemyServerRpc();
            }   

        }
        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer) return;
            if (other.GetComponent<PlayerBullet>())
            {
                GetComponent<NetworkHealthState>().health.Value -= 10;
            }
        }
        public void increaseSpeed() {
            speed *= 3;
        }

        [ServerRpc]
        private void SpawnEnemyServerRpc()
        {
            var enemy = Instantiate(bossPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn();
        }
    }
}