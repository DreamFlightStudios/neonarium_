using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    [SerializeField] private TMP_Text _fpsText;

    // Update is called once per frame
    void Update()
    {
       int FPS = (int)(1 / Time.deltaTime);
        _fpsText.text = "FPS: " + FPS.ToString();
    }
}
