using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Views
{
    [SelectionBase]
    public class CombinedBlockView : MonoBehaviour
    {
        private List<BlockView> _blockViews = new List<BlockView>();

        
        public List<BlockView> BlockViews => _blockViews;

        public void GetAllBlockViews()
        {
            _blockViews = GetComponentsInChildren<BlockView>().ToList();
        }
    }
}
