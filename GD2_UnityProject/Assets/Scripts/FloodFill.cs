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
        public delegate List<BlockPosition> NeighbourStrategy(BlockPosition from);
        private readonly NeighbourStrategy _neighbours;
        public FloodFill(NeighbourStrategy neighbours)
        {
            _neighbours = neighbours;
        }
        /// <summary>
        /// Gets all open positions into a list.
        /// open meaning from a startposition, everything until a block is hit
        /// </summary>
        /// <param name="startingPosition"></param>
        /// <returns></returns>
        public List<BlockPosition> Flood(BlockPosition startingPosition)
        {
            var nearbyPositions = new List<BlockPosition>();
            nearbyPositions.Add(startingPosition);

            var blocksToVisit = new Queue<BlockPosition>();
            blocksToVisit.Enqueue(startingPosition);

            while (blocksToVisit.Count > 0)
            {
                var currentBlock = blocksToVisit.Dequeue();

                var neighbours = _neighbours(currentBlock);
                foreach (var neighbour in neighbours)
                {
                    if (HasBlock(neighbour))
                    {
                        nearbyPositions.Add(neighbour);
                        blocksToVisit.Enqueue(neighbour);
                    }
                }
            }
            return nearbyPositions;
        }
        public bool HasBlock(BlockPosition neighbourPosition)
        {
            if (GameLoop.Instance.Array.BlockAt(neighbourPosition) != null)
                return true;
            return false;
        }
    }
}
