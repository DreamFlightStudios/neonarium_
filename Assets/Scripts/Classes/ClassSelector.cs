using UnityEngine;

namespace Classes
{
    public class ClassSelector : MonoBehaviour
    {
        public Classes CurrentClass {get; private set; } 
        public static ClassSelector Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public void SelectClass(Classes selectedClass) => CurrentClass = selectedClass;
    }
}