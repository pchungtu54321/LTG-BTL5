using System;
using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace
{
    public class NetworkCommunicator : NetworkBehaviour
    {
        private CharacterController _controller;
        
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        [ServerRpc]
        public void MovePlayerServerRpc(Vector3 position)
        {
            _controller.Move(position);
        }

        [ServerRpc]
        public void RotatePlayerServerRpc(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        // [ServerRpc]
        // public void AnimatePlayerServerRpc(bool isAttacking)
        // {
        //     _animator.SetBool("isAttacking", isAttacking);
        // }
    }
}