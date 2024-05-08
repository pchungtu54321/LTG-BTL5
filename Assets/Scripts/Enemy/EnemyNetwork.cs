using Player;
using Unity.Netcode;
using UnityEngine;

namespace Enemy
{
    public class EnemyNetwork : NetworkBehaviour
    {
        private GameObject[] _player;
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }
        private void Update()
        {
            _player = GameObject.FindGameObjectsWithTag("Player");
            if (_player.Length > 0)
            {
                var player = _player[0];
                // Rotate face to player
                var targetDirection = player.transform.position - transform.position;
                var rotation = Quaternion.LookRotation(targetDirection);
                RotateEnemyServerRpc(rotation);
                
                MoveEnemyServerRpc();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer) return;
            if (other.GetComponent<PlayerBullet>())
            {
                GetComponent<NetworkHealthState>().health.Value -= 30;
            }
            else if (other.GetComponent<PlayerMovement>())
            {
                Debug.Log("Enemy hit player!");
                other.GetComponent<NetworkHealthState>().health.Value -= 10;
            }
        }
        public void DestroyEnemy()
        {
            DestroyPlayerServerRpc();
        }
        [ServerRpc]
        private void DestroyPlayerServerRpc()
        {
            Destroy(gameObject);
        }
        [ServerRpc(RequireOwnership = false)]
        private void RotateEnemyServerRpc(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
        [ServerRpc(RequireOwnership = false)]
        private void MoveEnemyServerRpc()
        {
            transform.position += transform.forward * Time.deltaTime;
        }
    }
}