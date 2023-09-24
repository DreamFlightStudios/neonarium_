using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Classes
{
    public class ClassSelector : NetworkBehaviour
    {
        [SerializeField] private Button _medicButton, _stormtrooperButton, _supporterButton;
        [SerializeField] private NetworkObject _medic, _stormtrooper, _supporter;
        private readonly NetworkVariable<NetworkObject[]> _classes = new();

        private void Start()
        {
            _classes.Value = new[] {_medic, _stormtrooper, _supporter};
            
            _medicButton.onClick.AddListener(() => SelectClassServerRpc(1));
            _stormtrooperButton.onClick.AddListener(() => SelectClassServerRpc(2));
            _supporterButton.onClick.AddListener(() => SelectClassServerRpc(3));
        }

        [ServerRpc(RequireOwnership = false)]
        private void SelectClassServerRpc(byte selectedClass, ServerRpcParams serverRpcParams = default) => 
            Instantiate(_classes.Value[selectedClass]).SpawnAsPlayerObject(serverRpcParams.Receive.SenderClientId, true);
    }
}