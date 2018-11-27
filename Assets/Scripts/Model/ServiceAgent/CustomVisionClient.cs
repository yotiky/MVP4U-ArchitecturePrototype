using ArchitecturePrototype.Infrastructure.Entity;
using ArchitecturePrototype.Model.Device;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace ArchitecturePrototype.Model.ServiceAgent.Azure
{
    public class CustomVisionClient
    {
        private const float probabilityThreshold = 0.4f;

        private readonly ReactiveProperty<bool> canPost = new ReactiveProperty<bool>(true);
        public IReadOnlyReactiveProperty<bool> CanPost => canPost;

        public IObservable<CustomVisionResponse> Post(byte[] imageData)
        {
            return Observable.FromCoroutine<CustomVisionResponse>(o => Post(o, imageData));
        }

        private IEnumerator Post(IObserver<CustomVisionResponse> observer, byte[] imageData)//, Matrix4x4 cameraToWorldMatrix, Matrix4x4 pixelToCameraMatrix)
        {
            Debug.Log("Call Custom Vision API...");

            canPost.Value = false;

            using (var req = UnityWebRequest.Post(AzureCognitiveSettings.Instance.CustomVisionUrl, new WWWForm()))
            {
                req.SetRequestHeader("Content-Type", "application/octet-stream");
                req.SetRequestHeader("Prediction-Key", AzureCognitiveSettings.Instance.CustomVisionApiKey);

                req.uploadHandler = new UploadHandlerRaw(imageData);
                req.downloadHandler = new DownloadHandlerBuffer();

                yield return req.SendWebRequest();

                Debug.Log($"Response code:{req.responseCode}");
                if (req.isNetworkError || req.isHttpError)
                {
                    Debug.LogWarning(req.error);
                    observer.OnError(new Exception(req.error));
                    canPost.Value = true;
                    yield break;
                }

                Debug.Log("Response text:" + req.downloadHandler.text);
                var response = JsonUtility.FromJson<CustomVisionResponse>(req.downloadHandler.text);

                observer.OnNext(response);
            }

            observer.OnCompleted();
            canPost.Value = true;
        }


        public Vector3[] Convert(PhotoCamera.ShootingPlan plan, byte[] imageData, CustomVisionResponse response)
        {
            var node = response.predictions.OrderByDescending(x => x.probability).FirstOrDefault();
            if (node != null)
            {
                Debug.Log("Item detect finished...");
                return ConvertToPosition(plan, imageData, response).ToArray();
            }
            else
            {
                Debug.LogError("No Item detected...");
                return new Vector3[0];
            }
        }

        private List<Vector3> ConvertToPosition(PhotoCamera.ShootingPlan plan, byte[] imageData, CustomVisionResponse res)
        {
            const string tag = "Irohasu";


            var resolutionHeight = plan.Resolution.height;
            var resolutionWidth = plan.Resolution.width;
            var reproducedObjects = new List<Vector3>();

            foreach (var result in res.predictions.Where(x => probabilityThreshold < x.probability))
            {
                if (result.tagName == tag)
                {
                    // 画像から矩形を切りとる
                    // 左下原点
                    var rect = result.boundingBox;
                    var height = rect.height * resolutionHeight;
                    var width = rect.width * resolutionWidth;
                    var top = rect.top * resolutionHeight;
                    var left = rect.left * resolutionWidth;

                    // ワールド座標はpixelから変換
                    // 中央原点に再計算
                    top = -((top + height / 2) / resolutionHeight - .5f);
                    left = (left + width / 2) / resolutionWidth - .5f;

                    // 距離に応じて補正
                    var frustumWidthFactor = 2.0f * Mathf.Tan(67 * 0.5f * Mathf.Deg2Rad);
                    left = left * frustumWidthFactor;
                    top = top * frustumWidthFactor;

                    // サイズ、向きの調整
                    var cameraMatrix = plan.PixelToCamera.MultiplyPoint3x4(new Vector3(left, top, 0));
                    Debug.Log("#cameraMatrix" + cameraMatrix);

                    var targetPosition = plan.CameraToWorld.MultiplyPoint3x4(cameraMatrix);
                    Debug.Log("#worldMatrix:" + targetPosition);

                    reproducedObjects.Add(targetPosition);
                }
            }

            if (reproducedObjects.Count < 1)
            {
                Debug.Log("Target object not found.");
            }

            return reproducedObjects;
        }


    }
}