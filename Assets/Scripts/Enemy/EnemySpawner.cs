using System;
using Unity.Netcode;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject normalEnemyPrefab;
        [SerializeField] private GameObject bossPrefab;
        
        private NetworkVariable<int> _enemyCount = new NetworkVariable<int>();
        private float _spawnTime = 5f;
        private float _spawnTimer = 0f;
        private float _spawnRadius = 5f;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }
            base.OnNetworkSpawn();
            _enemyCount.Value = 3;
        }
        private void Update()
        {
            if (_spawnTimer <= 0)
            {
                SpawnEnemyServerRpc();
                _spawnTimer = _spawnTime;
            }
            else
            {
                _spawnTimer -= Time.deltaTime;
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SpawnEnemyServerRpc()
        {
            // if (Input.GetKeyDown(KeyCode.Alpha1)) {
            //     var position = transform.position + UnityEngine.Random.insideUnitSphere * _spawnRadius;
            //     position.y = 0;
            //     var enemy = Instantiate(normalEnemyPrefab, position, Quaternion.identity);
            //     enemy.GetComponent<NetworkObject>().Spawn();
            // }
            if (_enemyCount.Value >= 0)
            {
                var position = transform.position + UnityEngine.Random.insideUnitSphere * _spawnRadius;
                position.y = 0;
                var enemy = Instantiate(normalEnemyPrefab, position, Quaternion.identity);
                enemy.GetComponent<NetworkObject>().Spawn();
                _enemyCount.Value--;
            }
            else
            {
                Debug.Log("Spawn boss!");
            }
        }
    }
}