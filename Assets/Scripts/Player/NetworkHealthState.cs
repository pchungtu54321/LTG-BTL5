using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class NetworkHealthState : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkVariable<int> health = new NetworkVariable<int>();
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            health.Value = 100;
        }
    }
}