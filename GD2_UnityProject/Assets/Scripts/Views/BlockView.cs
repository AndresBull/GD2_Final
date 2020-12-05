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
        public List<BlockPosition> BlockPositions = new List<BlockPosition>();

        
        public void GetAllBlockPositions()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"This block is located at {BottomLeftBlockPosition.X}, {BottomLeftBlockPosition.Y} in the blockarray");
        }
    }
}
