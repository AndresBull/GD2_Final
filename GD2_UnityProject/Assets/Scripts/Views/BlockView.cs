using BoardBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Views
{
    [SelectionBase]
    public class BlockView : MonoBehaviour, IPointerClickHandler
    {
        public BlockPosition BottomLeftBlockPosition;

        /// <summary>
        /// Temp function to check stuffz (test block positions)
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Block {gameObject.name} is located at {BottomLeftBlockPosition.X}, {BottomLeftBlockPosition.Y} in the blockarray");
        }
    }
}
