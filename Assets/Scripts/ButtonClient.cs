using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ButtonClient : MonoBehaviour
    {
        [SerializeField] private Button _buttonClient;
        [SerializeField] private Button _buttonHost;

        private void Start()
        {
            _buttonClient.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
            _buttonHost.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        }
    }
}