using Unity.Netcode;
using UnityEngine;

namespace HealthSystem
{
    public class Health : NetworkBehaviour, IHealthSystem
    {
        [SerializeField] private int _maxHealth = 3;
        private readonly NetworkVariable<int> _currentHealth = new NetworkVariable<int>();
        private PlayerManager _playerManager;
        private bool _isDied;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _playerManager = FindObjectOfType<PlayerManager>();
            Init();
        }

        private void Init()
        {
            ChangeHealthServerRpc(_maxHealth);
            _isDied = false;
        }

        public void ApplyDamage(int damage)
        {
            if (_isDied) return;
            
            ChangeHealthServerRpc(_currentHealth.Value - damage);
            
            if (_currentHealth.Value > 0) return;
            DieServerRpc();
            _isDied = true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void DieServerRpc()
        {
            NetworkObject.Despawn(false);
            gameObject.SetActive(false);
            _playerManager.SpawnPlayerWithDelay(this);
        }

        public void Recovery(int health) { if (!_isDied) ChangeHealthServerRpc(_currentHealth.Value + health); }

        public void Revival()
        {
            if (!_isDied) return;
            NetworkObject.Spawn();
            Init();
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeHealthServerRpc(int value)
        {
            _currentHealth.Value = value;
            Mathf.Clamp(_currentHealth.Value, 0, _maxHealth);
        }
    }
}