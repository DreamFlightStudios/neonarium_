using Unity.Netcode;
using UnityEngine;

namespace HealthSystem
{
    public class Health : NetworkBehaviour, IHealthSystem
    {
        [SerializeField] private int _maxHealth = 3;
        private readonly NetworkVariable<int> _currentHealth = new NetworkVariable<int>();
        private bool _isDied;

        private void Start()
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

        [ServerRpc]
        private void DieServerRpc() => gameObject.SetActive(false);

        public void Recovery(int health)
        {
            if (_isDied) return;
            
            ChangeHealthServerRpc(_currentHealth.Value + health);
        }

        public void Revival() { if (_isDied) Start(); }

        [ServerRpc]
        private void ChangeHealthServerRpc(int value)
        {
            _currentHealth.Value = value;
            Mathf.Clamp(_currentHealth.Value, 0, _maxHealth);
        }
    }
}