using TMPro;
using UnityEngine;

namespace Jenga
{
    public class JengaTower : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        [SerializeField] private TextMeshProUGUI towerName;

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