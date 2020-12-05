using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    [SelectionBase]
    public class BlockView : MonoBehaviour, IPointerClickHandler
    {
        private MeshRenderer _meshRenderer;
        private Block _model;

        public Block Model
        {
            get => _model;
            set
            {
                _model = value;
            }
        }

        private void Start()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
        private void OnDestroy()
        {
            Model = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"This block is located at {Model.BlockPosition} in the blockarray");
        }
    }
}
