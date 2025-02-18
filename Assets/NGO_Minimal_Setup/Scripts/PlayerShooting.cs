using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetButtonDown("Fire1"))
        {
            ShootServerRpc(_firePoint.position, _firePoint.forward);
        }
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 position, Vector3 direction)
    {
        GameObject bullet = Instantiate(_bulletPrefab, position, Quaternion.LookRotation(direction));
        bullet.GetComponent<NetworkObject>().Spawn(true);
        bullet.GetComponent<Rigidbody>().linearVelocity = direction * 50f;
    }
}