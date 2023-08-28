using System.Collections;
using HealthSystem;
using Unity.Netcode;
using UnityEngine;

namespace Gun
{
    public class Gun : NetworkBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _rateOfFire = 0.1f;
        [SerializeField] private float _recharge = 3;
        [SerializeField] private int _maxClip = 20;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private bool _isMachineGun;
        
        private InputSystem _inputSystem;
        private float _time;
        private int _currentClip;
        private bool _canShoot = true;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient && IsOwner) Init();
        }

        private void Init()
        {
            _inputSystem = new InputSystem();
            _inputSystem.Gun.Recharge.performed += _ => StartCoroutine(Recharge());
            _inputSystem.Gun.Enable();
            _currentClip = _maxClip;
            _time = _rateOfFire;
        }

        private void Update()
        {
            if (!IsOwner) return;
            if (!Application.isFocused) return;
            if (!_canShoot) return;
            
            _time += Time.deltaTime;
            if (_time < _rateOfFire) return;
            
            if (_isMachineGun)
            {
                if (_inputSystem.Gun.Shoot.IsPressed()) Shoot();
            }
            else
            {
                if (_inputSystem.Gun.Shoot.WasPerformedThisFrame()) Shoot();
            }
        }

        protected virtual void Shoot()
        {
            if (Physics.Raycast(_shootPoint.position, _shootPoint.forward, out RaycastHit hit) && hit.collider.gameObject.TryGetComponent(out IHealthSystem healthSystem))
                healthSystem.ApplyDamage(_damage);

            _currentClip--;
            if (_currentClip == 0) StartCoroutine(Recharge());
            _time = 0;
        }

        private IEnumerator Recharge()
        {
            WaitForSeconds recharge = new WaitForSeconds(_recharge);
            
            _canShoot = false;
            yield return recharge;
            _canShoot = true;
	        _currentClip = _maxClip;
            _time = _rateOfFire;
        }
    }
}