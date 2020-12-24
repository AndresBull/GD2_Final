using BoardSystem;
using GameSystem.Management;
using System.Collections.Generic;

namespace GameSystem
{
    public class FloodFill
    {
        public List<BlockPosition> _floodedPositions = new List<BlockPosition>();
        
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
            var nearbyPositions = new List<BlockPosition>{startingPosition};

            var blocksToVisit = new Queue<BlockPosition>();
            blocksToVisit.Enqueue(startingPosition);

            while (blocksToVisit.Count > 0)
            {
                var currentBlock = blocksToVisit.Dequeue();

                var neighbours = _neighbours(currentBlock);
                foreach (var neighbour in neighbours)
                {
                    if (nearbyPositions.Contains(neighbour))
                        continue;

                    if (!HasBlock(neighbour))
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
            if (GameLoop.Instance.FieldView.BlockAt(neighbourPosition) != null)
                return true;
            return false;
        }
    }
}
