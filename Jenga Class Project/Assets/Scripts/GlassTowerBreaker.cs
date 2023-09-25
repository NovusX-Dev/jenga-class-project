using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jenga
{
    public class GlassTowerBreaker : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        private JengaPoolBuilder poolBuilder;

        #endregion

        #region PRIVATE_VARIABLES
        
        private List<BlockInfo> _allBlocks = new List<BlockInfo>();

        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region UNITY_CALLS
        
        public void InitStart(JengaPoolBuilder poolBuilder, List<BlockInfo> allBlocks)
        {
            this.poolBuilder = poolBuilder;
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

        #region PRIVATE_METHODS

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