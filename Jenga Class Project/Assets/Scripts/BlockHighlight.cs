using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jenga
{
    public class BlockHighlight : MonoBehaviour
    {
        #region EXPOSED_VARIABLES
        
        [SerializeField] private Color highlightColor = Color.white;

        #endregion

        #region PRIVATE_VARIABLES

        private bool _clicked;
        private bool _isHighlighted;
        private WaitForSeconds _waitTime;
        private Material _originalMaterial;
        private Color _originalColor;
        private StudentsGradesData _studentsGradesData;

        #endregion

        #region PUBLIC_VARIABLES
        
        public static event Action<StudentsGradesData> OnBlockClicked;
        public static event Action OnBlockExited;
        
        #endregion

        #region UNITY_CALLS
        
        private void Awake()
        {
            _originalMaterial = GetComponent<MeshRenderer>().material;
            _originalColor = _originalMaterial.color;
            _waitTime = new WaitForSeconds(2f);
        }

        private void OnMouseEnter()
        {
            _originalMaterial.color = highlightColor;
            _isHighlighted = true;
        }

        private void OnMouseExit()
        {
            _originalMaterial.color = _originalColor;
            _isHighlighted = false;
            if (!_clicked) return;

            StartCoroutine(WaitAndTurnOffInfo());
            
            IEnumerator WaitAndTurnOffInfo()
            {
                yield return _waitTime;
                _clicked = false;
                OnBlockExited?.Invoke();
            }
        }

        private void OnMouseDown()
        {
            if(!_isHighlighted) return;
            OnBlockClicked?.Invoke(_studentsGradesData);
            _clicked = true;
        }

        #endregion

        #region PRIVATE_METHODS

        #endregion

        #region PUBLIC_METHODS
        
        public void SetData(StudentsGradesData data)
        {
            _studentsGradesData = data;
        }

        #endregion


        
    }
}