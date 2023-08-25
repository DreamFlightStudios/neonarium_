using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ButtonClient : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Start()
        {
            _button.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        }
    }
}