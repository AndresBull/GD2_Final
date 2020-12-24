using BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem.Views
{
    public class BlockView : MonoBehaviour, IPointerClickHandler
    {
        public BlockPosition Position;

        /// <summary>
        /// Temp function to check stuffz (test block positions)
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Block {gameObject.name} is located at {Position.X}, {Position.Y} in the blockarray");
        }
    }
}
