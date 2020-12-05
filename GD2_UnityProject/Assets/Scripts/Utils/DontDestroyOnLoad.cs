using UnityEngine;

namespace Assets.Scripts.Utils
{
    class DontDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
