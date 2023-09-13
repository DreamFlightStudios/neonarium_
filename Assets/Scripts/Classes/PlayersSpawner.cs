using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Classes
{
    public class PlayersSpawner : NetworkBehaviour
    {
        [SerializeField] private NetworkObject _playerPrefab;

        public override void OnNetworkSpawn() { if (IsServer) NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += StartGame; }
    
        private void StartGame(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
        {
            foreach (ulong id in NetworkManager.Singleton.ConnectedClientsIds) Instantiate(_playerPrefab).SpawnAsPlayerObject(id);
        }
    }
}