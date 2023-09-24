using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Pool;

namespace Jenga
{
    public class JengaPoolBuilder : MonoBehaviour
    {
        #region EXPOSED_VARIABLES

        [SerializeField] private Transform blockPoolParent;
        [SerializeField] private BlockInfo glassBlock;
        [SerializeField] private BlockInfo woodBlock;
        [SerializeField] private BlockInfo stoneBlock;
        
        [SerializeField] private JengaTowerBuilder towerBuilder;

        #endregion

        #region PRIVATE_VARIABLES

        private const string DataPath = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";
        private const string SixthGradeString = "6th Grade";
        private const string SeventhGradeString = "7th Grade";
        private const string EighthGradeString = "8th Grade";
        
        private StudentsGradesData[] _blockData;
        private List<StudentsGradesData> _sixthGradeDataList = new List<StudentsGradesData>();
        private List<StudentsGradesData> _seventhGradeDataList = new List<StudentsGradesData>();
        private List<StudentsGradesData> _eightGradeDataList = new List<StudentsGradesData>();
        private ObjectPool<BlockInfo> _sixthPool;
        private ObjectPool<BlockInfo> _seventhPool;
        private ObjectPool<BlockInfo> _eighthPool;

        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region UNITY_CALLS

        private IEnumerator Start()
        {
            yield return GetDataRoutine();
            FilterDataByGrade();
            yield return null;
            _sixthPool = new ObjectPool<BlockInfo>(CreateGlassBlock, GetBlock, ReleaseBlock, DestroyBlock, false, _sixthGradeDataList.Count + 10);
            _seventhPool = new ObjectPool<BlockInfo>(CreateWoodBlock, GetBlock, ReleaseBlock, DestroyBlock, true, _seventhGradeDataList.Count + 10);
            _eighthPool = new ObjectPool<BlockInfo>(CreateStoneBlock, GetBlock, ReleaseBlock, DestroyBlock, false, _eightGradeDataList.Count + 10);
            
        }

        #endregion

        #region PRIVATE_METHODS

        private IEnumerator GetDataRoutine()
        {
            UnityWebRequest request = UnityWebRequest.Get(DataPath);
            yield return request.SendWebRequest();
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else if (request.result is UnityWebRequest.Result.Success)
            {
                var jsonString = request.downloadHandler.text;
                _blockData = JsonService.FromJson<StudentsGradesData>(jsonString);
            }
        }
        
        private void FilterDataByGrade()
        {
            if (_blockData == null)
            {
                Debug.LogError($"Block data received is null");
                return;
            }
            
            foreach (var data in _blockData)
            {
                switch (data.grade)
                {
                    case SixthGradeString:
                        _sixthGradeDataList.Add(data);
                        break;
                    case SeventhGradeString:
                        _seventhGradeDataList.Add(data);
                        break;
                    case EighthGradeString:
                        _eightGradeDataList.Add(data);
                        break;
                }
            }
        }

        private BlockInfo CreateGlassBlock()
        {
            var block = Instantiate(glassBlock, blockPoolParent, true);
            block.SetPool(_sixthPool);
            return block;
        }
        
        private BlockInfo CreateWoodBlock()
        {
            var block = Instantiate(woodBlock, blockPoolParent, true);
            block.SetPool(_seventhPool);
            return block;
        }
        
        private BlockInfo CreateStoneBlock()
        {
            var block = Instantiate(stoneBlock, blockPoolParent, true);
            block.SetPool(_eighthPool);
            return block;
        }

        private void GetBlock(BlockInfo block)
        {
            block.gameObject.SetActive(true);
        }
        
        private void ReleaseBlock(BlockInfo block)
        {
            block.gameObject.SetActive(false);
        }
        
        private void DestroyBlock(BlockInfo block)
        {
            Destroy(block.gameObject);
        }
        

        #endregion

        #region PUBLIC_METHODS

        #endregion


    }
}