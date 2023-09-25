using TMPro;
using UnityEngine;

namespace Jenga
{
    public class JengaTower : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        [SerializeField] private TextMeshProUGUI towerName;

        #endregion

        #region PRIVATE_VARIABLES

        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region UNITY_CALLS

        #endregion

        #region PRIVATE_METHODS

        #endregion

        #region PUBLIC_METHODS
        
        public void SetTowerName(string grade)
        {
            towerName.text = grade;
            towerName.gameObject.SetActive(!string.IsNullOrEmpty(grade));
        }

        #endregion

        
    }
}