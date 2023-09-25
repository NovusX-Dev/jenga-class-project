using System;
using System.Collections;
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
        [SerializeField] private float timeToDisableGradeInfoPanel = 2f;

        #endregion

        #region PRIV

        private WaitForSeconds _waitTime;
        private Coroutine _disableGradeInfoPanelCoroutine;

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

        private void Awake()
        {
            _waitTime = new WaitForSeconds(timeToDisableGradeInfoPanel);
        }

        #endregion

        #region PRIVATE_METHODS
        
        private void DisableGradeInfoPanel()
        {
            _disableGradeInfoPanelCoroutine = StartCoroutine(WaitAndTurnOffInfo());
            
            IEnumerator WaitAndTurnOffInfo()
            {
                yield return _waitTime;
                gradeInfoPanel.SetActive(false);
                _disableGradeInfoPanelCoroutine = null;
            }
        }
        private void OnBlockClicked(StudentsGradesData data)
        {
            if(_disableGradeInfoPanelCoroutine != null)
                StopCoroutine(_disableGradeInfoPanelCoroutine);
            
            gradeInfoText.text = $"<b>{data.grade}:</b> {data.domain}";
            clusterText.text = $"<b>Cluster:</b> {data.cluster}";
            standardText.text = $"<b>{data.standardid}:</b> {data.standarddescription}";
            gradeInfoPanel.SetActive(true);
        }

        #endregion

    }
}