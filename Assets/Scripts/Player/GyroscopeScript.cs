using UnityEngine;

public class GyroscopeScript : MonoBehaviour
{
    [SerializeField] private Transform Player;

    void Start()
    {
        Input.gyro.enabled = true;
    }
    void Update()
    {
        transform.localRotation = Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
        Player.transform.rotation = Quaternion.Euler(0, Input.gyro.attitude.y, 0);
    }
}