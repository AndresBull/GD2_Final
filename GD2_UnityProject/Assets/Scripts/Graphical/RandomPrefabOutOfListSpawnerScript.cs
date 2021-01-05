using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Graphical
{
    public class RandomPrefabOutOfListSpawnerScript : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> _prefabsToChooseFrom = new List<GameObject>();
        // Start is called before the first frame update
        private void Start()
        {
            int randomNumber = Random.Range(0, _prefabsToChooseFrom.Count);

            Instantiate(_prefabsToChooseFrom[randomNumber]);
        }

    }
}