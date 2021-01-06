using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowScoreScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _floatingTextPrefab;
    //private int PlayerIndex { get this.gameObject.; set; };

    public void ShowFloatingText(string textToBeShown) 
    {
        if(_floatingTextPrefab==null)
            _floatingTextPrefab = Resources.Load("PopUpTextPrefab", typeof(GameObject)) as GameObject;

        var floatingText= Instantiate(_floatingTextPrefab, this.transform.position, Quaternion.identity, transform);
        floatingText.GetComponent<TextMeshPro>().text = textToBeShown;
    }
}
