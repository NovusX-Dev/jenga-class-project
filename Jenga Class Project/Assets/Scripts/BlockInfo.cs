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
        
        [SerializeField] private Rigidbody rigidbody;

        #endregion

        #region PRIVATE_VARIABLES
        
        private ObjectPool<BlockInfo> _pool;
        
        #endregion

        #region PUBLIC_VARIABLES
        
        public BlockType BlockType { get; set; }
        public StudentsGradesData StudentsGradesData { get; private set; }

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
            StudentsGradesData = data;
        }
        
        public void SetPhysics(bool activate)
        {
            rigidbody.isKinematic = !activate;
        }
        
        public void Release()
        {
            _pool.Release(this);
        }

        #endregion

        
    }
}