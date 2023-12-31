﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Jenga
{
    public class JengaTowerBuilder : MonoBehaviour
    {
        #region EXPOSED_VARIABLES

        [Header("Towers")]
        [SerializeField] private JengaTower leftTower;
        [SerializeField] private JengaTower centerTower;
        [SerializeField] private JengaTower rightTower;
        
        [Header("Block Settings")]
        [SerializeField] private float blockSpacing = 0.25f;
        [SerializeField] private int rotationInterval = 3;

        #endregion

        #region PRIVATE_VARIABLES
        
        private JengaPoolBuilder _poolBuilder;

        #endregion

        #region PUBLIC_VARIABLES
        
        public bool Initialized { get; private set; }

        #endregion

        #region UNITY_CALLS

        public IEnumerator InitStart(JengaPoolBuilder poolBuilder)
        {
            _poolBuilder = poolBuilder;
            yield return new WaitForEndOfFrame();
            CreateJengaTowerInstantly(leftTower, _poolBuilder.SixthGradeDataList);
            CreateJengaTowerInstantly(centerTower, _poolBuilder.SeventhGradeDataList);
            CreateJengaTowerInstantly(rightTower, _poolBuilder.EightGradeDataList);
            yield return new WaitForEndOfFrame();
            Initialized = true;
        }

        #endregion

        #region PRIVATE_METHODS

        private void CreateJengaTowerInstantly(JengaTower tower, List<StudentsGradesData> grades)
        {
            tower.SetTowerName(grades[0].grade);
            var spawnPosition = Vector3.zero;
            var rotation = Quaternion.identity;
            var staggerX = false;
            var startRotationIndex = 0;
            for (int i = 0; i < grades.Count; i++)
            {
                var block = GetBlock(grades[i].mastery);
                
                block.SetData(grades[i]);
                block.transform.SetParent(tower.transform);
                if (startRotationIndex == 0)
                {
                    rotation = Quaternion.Euler(0f, staggerX ? 90f : 0f, 0f);
                    if (staggerX)
                    {
                        spawnPosition.z = 0f;
                        spawnPosition.x = (block.transform.localScale.z + blockSpacing) * -1f;
                    }
                    else
                    {
                        spawnPosition.x = 0f;
                        spawnPosition.z = (block.transform.localScale.z + blockSpacing) * -1f;
                    }
                }
                block.transform.localPosition = spawnPosition;
                block.transform.rotation = rotation;
                block.name = $"{block.BlockType} block {i}";
                _poolBuilder.AddToAllBlocksList(block);
                startRotationIndex++;

                if (staggerX)
                {
                    rotation = Quaternion.Euler(0f, 90f, 0f);

                    if(startRotationIndex == 1)
                    {
                        spawnPosition.x = 0f;
                    }
                    if(startRotationIndex == 2)
                    {
                        spawnPosition.x = (block.transform.localScale.z + blockSpacing) * 1f;
                    }
                }
                else
                {
                    rotation = Quaternion.Euler(0f, 0f, 0f);
                    if(startRotationIndex == 1)
                    {
                        spawnPosition.z = 0f;
                    }
                    if(startRotationIndex == 2)
                    {
                        spawnPosition.z = (block.transform.localScale.z + blockSpacing) * 1f;
                    }
                }

                if ((i + 1) % rotationInterval == 0)
                {
                    startRotationIndex = 0;
                    spawnPosition.y += block.transform.localScale.y;
                    staggerX = !staggerX;
                }
            }
        }
        
        private BlockInfo GetBlock(int matery)
        {
            BlockInfo block = null;
            switch (matery)
            {
                case 0:
                    block = _poolBuilder.GlassPool.Get();
                    break; 
                case 1:
                    block = _poolBuilder.WoodPool.Get();
                    break;
                case 2: 
                    block = _poolBuilder.StonePool.Get();
                    break;
            }

            return block;
        }
        
        #endregion
    }
}