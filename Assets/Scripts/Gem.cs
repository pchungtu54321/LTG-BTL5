using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Unity.Netcode;

public class Gem : NetworkBehaviour
{
    private GameObject[] _player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (!IsServer) return;
        if (other.GetComponent<PlayerMovement>())
        {
            Debug.Log("Enemy hit player!");
            other.GetComponent<PlayerMovement>().increaseSpeed();
            // Destroy(gameObject);
            DestroyEnemy();
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
}
