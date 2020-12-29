using BoardSystem;
using GameSystem.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem.Views
{
    public class BlockFieldView : MonoBehaviour
    {
        [SerializeField]
        private PositionConverter _positionConverter = null;

        public PositionConverter PositionConverter => _positionConverter;

        private void Awake()
        {
            GameLoop.Instance.FieldView = this;
        }
    }
}