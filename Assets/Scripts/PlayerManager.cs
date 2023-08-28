using System.Threading.Tasks;
using HealthSystem;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public async void SpawnPlayerWithDelay(Health health)
    {
        await Task.Delay(3000);
        health.gameObject.SetActive(true);
        health.Revival();
    }
}
