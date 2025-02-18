using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<int> Health = new(100);

    [ServerRpc]
    public void TakeDamageServerRpc(int damage)
    {
        Health.Value -= damage;
        if (Health.Value <= 0);
    }

}