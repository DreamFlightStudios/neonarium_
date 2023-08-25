using Unity.Netcode;
using UnityEngine;

namespace HealthSystem
{
    public class Health : MonoBehaviour, IHealthSystem
    {
        [SerializeField] private int _maxHealth = 3;
        private int _currentHealth;
        private bool _isDied;

        private void Start()
        {
            _currentHealth = _maxHealth;
            _isDied = false;
        }

        public void ApplyDamage(int damage)
        {
            if (_isDied) return;
            
            _currentHealth -= damage;
            
            if (_currentHealth > 0) return;
            DieServerRpc();
            _isDied = true;
        }

        [ServerRpc]
        private void DieServerRpc() => gameObject.SetActive(false);

        public void Recovery(int health)
        {
            if (_isDied) return;
            
            _currentHealth += health;
            Mathf.Clamp(_currentHealth, 0, _maxHealth);
        }

        public void Revival() { if (_isDied) Start(); }
    }
}