using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Management
{
    public class RulesScreenUpdater : MonoBehaviour
    {
        [SerializeField] private float _timeBeforeNextScene = 10.0f;
        [SerializeField] private Image _loadBar;

        private float _width, _timer;
        private RectTransform _imageRectTrans;

        private void Start()
        {
            _imageRectTrans = _loadBar.transform as RectTransform;
            _width = _imageRectTrans.rect.width;
        }

        void Update()
        {
            _timer += Time.deltaTime;
            float width = _timer * (_width / _timeBeforeNextScene);
            _imageRectTrans.sizeDelta = new Vector2(width, _imageRectTrans.sizeDelta.y);

            if (_timer >= _timeBeforeNextScene)
            {
                GameLoop.Instance.StateMachine.MoveTo(GameStates.Play);
                Destroy(gameObject);
            }
        }
    }
}