using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Jenga
{
    public class JengaPoolBuilder : MonoBehaviour
    {
        #region EXPOSED_VARIABLES

        #endregion

        #region PRIVATE_VARIABLES

        private readonly string _dataPath = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

        #endregion

        #region PUBLIC_VARIABLES

        #endregion

        #region UNITY_CALLS

        private void Start()
        {
            GetData();
        }

        #endregion

        #region PRIVATE_METHODS

        private void GetData()
        {
            StartCoroutine(DataRoutine());
            IEnumerator DataRoutine()
            {
                UnityWebRequest request = UnityWebRequest.Get(_dataPath);
                yield return request.SendWebRequest();
                if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else if (request.result is UnityWebRequest.Result.Success)
                {
                    BlockData[] blockData;
                    var jsonString = request.downloadHandler.text;
                    Debug.Log(jsonString);
                    blockData = JsonService.FromJson<BlockData>(jsonString);
                    Debug.Log(blockData.Length);
                    foreach (var data in blockData)
                    {
                        Debug.Log(data.id + data.subject + data.grade + data.mastery);
                    }
                }
            }
        }
        

        #endregion

        #region PUBLIC_METHODS

        #endregion


    }
}