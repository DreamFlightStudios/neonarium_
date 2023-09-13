using Unity.Netcode;
using UnityEngine;

namespace HealthSystem
{
    public class Health : NetworkBehaviour, IHealthSystem
    {
        [SerializeField] private byte _maxHealth = 3;
        private readonly NetworkVariable<byte> _currentHealth = new();
        private bool _isDied;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Init();
        }

        private void Init()
        {
            ChangeHealthServerRpc(_maxHealth);
            _isDied = false;
        }

        public void ApplyDamage(byte damage)
        {
            if (_isDied) return;
            
            ChangeHealthServerRpc(_currentHealth.Value - damage);
            
            if (_currentHealth.Value > 0) return;
            DieServerRpc();
            _isDied = true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void DieServerRpc() => NetworkObject.Despawn();

        public void Recovery(byte health) { if (!_isDied) ChangeHealthServerRpc(_currentHealth.Value + health); }

        public void Revival()
        {
            if (!_isDied) return;
            NetworkObject.Spawn();
            Init();
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeHealthServerRpc(int value)
        {
            _currentHealth.Value = (byte)value;
            Mathf.Clamp(_currentHealth.Value, 0, _maxHealth);
        }
    }
}