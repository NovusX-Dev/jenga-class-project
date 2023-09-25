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
        [SerializeField] private GlassTowerBreaker glassTowerBreaker;

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
        private ObjectPool<BlockInfo> _glassPool;
        private ObjectPool<BlockInfo> _woodPool;
        private ObjectPool<BlockInfo> _stonePool;
        private List<BlockInfo> _allBlocks = new List<BlockInfo>();

        #endregion

        #region PUBLIC_VARIABLES

        public List<StudentsGradesData> SixthGradeDataList => _sixthGradeDataList;
        public List<StudentsGradesData> SeventhGradeDataList => _seventhGradeDataList;
        public List<StudentsGradesData> EightGradeDataList => _eightGradeDataList;
        public ObjectPool<BlockInfo> GlassPool => _glassPool;
        public ObjectPool<BlockInfo> WoodPool => _woodPool;
        public ObjectPool<BlockInfo> StonePool => _stonePool;
        
        #endregion

        #region UNITY_CALLS

        private IEnumerator Start()
        {
            yield return GetDataRoutine();
            FilterDataByGrade();
            yield return null;
            yield return FullSortRoutine(_sixthGradeDataList);
            yield return FullSortRoutine(_seventhGradeDataList);
            yield return FullSortRoutine(_eightGradeDataList);
            yield return null;
            _glassPool = new ObjectPool<BlockInfo>(CreateGlassBlock, GetBlock, ReleaseBlock, DestroyBlock, false, 50);
            _woodPool = new ObjectPool<BlockInfo>(CreateWoodBlock, GetBlock, ReleaseBlock, DestroyBlock, true, 50);
            _stonePool = new ObjectPool<BlockInfo>(CreateStoneBlock, GetBlock, ReleaseBlock, DestroyBlock, false, 50);
            yield return new WaitForEndOfFrame();
            yield return towerBuilder.InitStart(this);
            yield return new WaitUntil(()=> towerBuilder.Initialized);
            glassTowerBreaker.InitStart(this, _allBlocks);
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
        
        IEnumerator FullSortRoutine(List<StudentsGradesData> data)
        {
            yield return null;
            SortDataByDomainAscending(data);
            yield return null;
            SortDataByClusterAscending(data);
            yield return null;
            SortDataByStandardIdAscending(data);
        }
        
        private void SortDataByDomainAscending(List<StudentsGradesData> data)
        {
            data.Sort((previousData, nextData) => string.Compare(previousData.domain, nextData.domain, StringComparison.Ordinal));
        }
        
        private void SortDataByClusterAscending(List<StudentsGradesData> data)
        {
            data.Sort((previousData, nextData) => string.Compare(previousData.cluster, nextData.cluster, StringComparison.Ordinal));
        }
        
        private void SortDataByStandardIdAscending(List<StudentsGradesData> data)
        {
            data.Sort((previousData, nextData) => string.Compare(previousData.standardid, nextData.standardid, StringComparison.Ordinal));
        }
        

        private BlockInfo CreateGlassBlock()
        {
            var block = Instantiate(glassBlock, blockPoolParent, true);
            block.SetPool(_glassPool);
            block.BlockType = BlockType.Glass;
            return block;
        }
        
        private BlockInfo CreateWoodBlock()
        {
            var block = Instantiate(woodBlock, blockPoolParent, true);
            block.SetPool(_woodPool);
            block.BlockType = BlockType.Wood;
            return block;
        }
        
        private BlockInfo CreateStoneBlock()
        {
            var block = Instantiate(stoneBlock, blockPoolParent, true);
            block.SetPool(_stonePool);
            block.BlockType = BlockType.Stone;
            return block;
        }

        private void GetBlock(BlockInfo block)
        {
            block.gameObject.SetActive(true);
        }
        
        private void ReleaseBlock(BlockInfo block)
        {
            block.transform.SetParent(blockPoolParent);
            block.transform.localPosition = Vector3.zero;
            block.gameObject.SetActive(false);
        }
        
        private void DestroyBlock(BlockInfo block)
        {
            Destroy(block.gameObject);
        }
        
        #endregion

        #region PUBLIC_METHODS
        
        
        
        public void AddToAllBlocksList(BlockInfo block)
        {
            _allBlocks.Add(block);
        }

        #endregion


    }
}