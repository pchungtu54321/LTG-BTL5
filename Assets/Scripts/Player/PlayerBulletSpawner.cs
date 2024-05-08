using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerBulletSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;

        [SerializeField] private GameObject spawnPoint;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsOwner)
            {
                SpawnBulletServerRpc(spawnPoint.transform.position, spawnPoint.transform.rotation);
            }   
        }

        [ServerRpc]
        private void SpawnBulletServerRpc(Vector3 position, Quaternion rotation)
        {
            var bullet = Instantiate(bulletPrefab, position, rotation);
            bullet.GetComponent<NetworkObject>().Spawn();
        }
    }
}