using Unity.Netcode;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject _camera;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            _camera.SetActive(false);
            Destroy(GetComponent<PlayerInput>());
        }
    }
}