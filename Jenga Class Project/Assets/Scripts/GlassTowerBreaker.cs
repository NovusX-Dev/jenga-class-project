using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jenga
{
    public class GlassTowerBreaker : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        
        private List<BlockInfo> _allBlocks = new List<BlockInfo>();

        #endregion

        #region UNITY_CALLS
        
        public void InitStart(List<BlockInfo> allBlocks)
        {
            _allBlocks = allBlocks;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BreakGlassTower();
            }
        }

        #endregion

        #region PUBLIC_METHODS
        
        public void BreakGlassTower()
        {
            foreach (var block in _allBlocks)
            {
                block.SetPhysics(true);
                if (block.BlockType == BlockType.Glass)
                {
                    block.Release();
                }
            }
        }

        #endregion

        
    }
}