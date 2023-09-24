using UnityEngine;
using UnityEngine.Pool;

namespace Jenga
{
    public enum BlockType 
    {
        Glass,
        Wood,
        Stone
    }
    
    public class BlockInfo : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        [field: SerializeField] public BlockType blockType { get; private set; }

        #endregion

        #region PRIVATE_VARIABLES
        
        private ObjectPool<BlockInfo> _pool;
        
        #endregion

        #region PUBLIC_VARIABLES
        
        public StudentsGradesData studentsGradesData { get; private set; }

        #endregion

        #region UNITY_CALLS

        #endregion

        #region PRIVATE_METHODS

        #endregion

        #region PUBLIC_METHODS
        
        public void SetPool(ObjectPool<BlockInfo> pool)
        {
            _pool = pool;
        }
        
        public void SetData(StudentsGradesData data)
        {
            studentsGradesData = data;
        }

        #endregion

        
    }
}