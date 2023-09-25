using System;
using UnityEngine;

namespace Jenga
{
    public class CameraController : MonoBehaviour
    {
        #region EXPOSED_VARIABLES

        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private Vector2 zoomClamps = new Vector2( 5f, 20f);
        [SerializeField] private Vector2 verticalClamps = new Vector2( 0.5f, 5.5f);
        [SerializeField] private Vector3 towerFocusOffset = new Vector3(0f, 3.5f, -10f);
        [SerializeField] private float verticalMovementSpeed = 1f;
        
        [Header("References")]
        [SerializeField] private Camera mainCamera = null;
        [SerializeField] private JengaTower[] towers;
        
        #endregion

        #region PRIVATE_VARIABLES
        
        private Transform _currentTarget;
        private int _currentTargetIndex = 0;

        #endregion

        #region UNITY_CALLS

        private void Start()
        {
            SetTarget(towers[0].transform, true);
        }

        public void LateUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                var mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
                mainCamera.transform.RotateAround(_currentTarget.position, Vector3.up, mouseX);
            }
            
            var scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
            var zoomAmount = scrollWheelInput * zoomSpeed;
            
            var zoomDirection = (mainCamera.transform.position - _currentTarget.position).normalized;
            var currentZoomDistance = Vector3.Distance(mainCamera.transform.position, _currentTarget.position);
            var newZoomDistance = Mathf.Clamp(currentZoomDistance + zoomAmount, zoomClamps.x, zoomClamps.y);
            var newCamPosition = _currentTarget.position + zoomDirection * newZoomDistance;
            
            mainCamera.transform.position = newCamPosition;
            
            if(Input.GetMouseButton(1))
            {
                var mouseY = Input.GetAxis("Mouse Y") * verticalMovementSpeed;
                var newPos = mainCamera.transform.position + transform.up * mouseY;
                newPos.y = Mathf.Clamp(newPos.y, verticalClamps.x, verticalClamps.y);
                mainCamera.transform.position = newPos;
            }
        }

        #endregion

        #region PRIVATE_METHODS

        private void FocusCamera()
        {
            mainCamera.transform.position = _currentTarget.position + towerFocusOffset;
            mainCamera.transform.rotation =  Quaternion.identity;
        }

        #endregion

        #region PUBLIC_METHODS
        
        private void SetTarget(Transform target, bool focusCamera)
        {
            _currentTarget = target;
            if(focusCamera)
                FocusCamera();
        }

        public void NextTarget()
        {
            if(_currentTargetIndex >= towers.Length - 1)
            {
                _currentTargetIndex = 0;
            }
            else
            {
                _currentTargetIndex++;
            }
            SetTarget(towers[_currentTargetIndex].transform, true);
        }

        #endregion

        
    }
}