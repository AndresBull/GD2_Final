using BoardBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Views;

namespace Assets.Scripts
{
    public class FloodFill
    { 
    /// <summary>
    /// Recreating chess part 27, around 14 minutes
    /// in the process of making the list of all connected blocks via neighbourstrategy
    /// if player is not in connectedblock, they are trapped.
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
        
        public delegate List<BlockPosition> NeighbourStrategy(BlockPosition from);
        private readonly NeighbourStrategy _neighbours;
        public FloodFill(NeighbourStrategy neighbours)
        {
            _neighbours = neighbours;
        }
        public List<BlockPosition> Flood(BlockPosition startingPosition)
        {
            var nearbyPosition = new List<BlockPosition>();

            var blocksToVisit = new Queue<BlockPosition>();
            blocksToVisit.Enqueue(startingPosition);

            while (blocksToVisit.Count > 0)
            {
                var currentBlock = blocksToVisit.Dequeue();

                var neighbours = _neighbours(currentBlock);
                foreach (var neighbour in neighbours)
                {
                    return null;
                }
                return null;
            }
            return null;
        }
    }
}
