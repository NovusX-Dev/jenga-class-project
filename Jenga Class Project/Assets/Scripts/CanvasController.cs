using System;
using TMPro;
using UnityEngine;

namespace Jenga
{
    public class CanvasController : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        [Header("Grade Info Panel")]
        [SerializeField] private GameObject gradeInfoPanel = null;
        [SerializeField] private TextMeshProUGUI gradeInfoText = null;
        [SerializeField] private TextMeshProUGUI clusterText = null;
        [SerializeField] private TextMeshProUGUI standardText = null;

        #endregion

        #region PRIVATE_VARIABLES
        

        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region UNITY_CALLS

        private void OnEnable()
        {
            BlockHighlight.OnBlockClicked += OnBlockClicked;
            BlockHighlight.OnBlockExited += DisableGradeInfoPanel;
        }

        private void OnDisable()
        {
            BlockHighlight.OnBlockClicked -= OnBlockClicked;
            BlockHighlight.OnBlockExited -= DisableGradeInfoPanel;
        }

        #endregion

        #region PRIVATE_METHODS
        
        private void DisableGradeInfoPanel()
        {
            gradeInfoPanel.SetActive(false);
        }
        private void OnBlockClicked(StudentsGradesData data)
        {
            gradeInfoText.text = $"<b>{data.grade}:</b> {data.domain}";
            clusterText.text = $"<b>Cluster:</b> {data.cluster}";
            standardText.text = $"<b>{data.standardid}:</b> {data.standarddescription}";
            gradeInfoPanel.SetActive(true);
        }

        #endregion

        #region PUBLIC_METHODS

        #endregion

        
    }
}