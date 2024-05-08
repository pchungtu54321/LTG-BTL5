using System;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerBullet : NetworkBehaviour {

        [SerializeField] private float speed = 1f;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        private void Update()
        {
            var tf = transform.position + transform.forward * speed;
            MoveBulletServerRpc(tf);
        }

        private void OnTriggerExit(Collider other)
        {
            DestroyBulletServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void DestroyBulletServerRpc()
        {
            Destroy(gameObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void MoveBulletServerRpc(Vector3 position)
        {
            // MoveBulletClientRpc(position);       
            transform.position = position;
        }
    }
}
