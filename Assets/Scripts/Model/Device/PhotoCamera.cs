using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.XR.WSA.WebCam;

namespace ArchitecturePrototype.Model.Device
{
    public class PhotoCamera
    {
        public class ShootingPlan
        {
            public Resolution Resolution { get; set; }
            public Vector3 ShootingLocation { get; set; }
            public Matrix4x4 CameraToWorld { get; set; }
            public Matrix4x4 PixelToCamera { get; set; }
        }

        private PhotoCapture photoCapture;
        //private CameraParameters cameraParameters;

        private bool canSave;
        private string filePath;

        private Subject<byte[]> onPhotoCaptured = new Subject<byte[]>();
        public IObservable<byte[]> OnPhotoCaptured => onPhotoCaptured;

        private ReactiveProperty<bool> canTakePhoto = new ReactiveProperty<bool>(true);
        public IReadOnlyReactiveProperty<bool> CanTakePhoto => canTakePhoto;

        //public int CameraResolutionWidth => cameraParameters.cameraResolutionWidth;
        //public int CameraResolutionHeight => cameraParameters.cameraResolutionHeight;

        public Resolution Resolution { get; }
        public ShootingPlan Plan { get; private set; }

        public PhotoCamera()
        {
            Resolution = PhotoCapture.SupportedResolutions.OrderByDescending(r => r.width * r.height).First();
        }

        public void TakePhotoAsync(bool showHolograms = false)
        {
            Plan = null;
            canSave = false;
            TakePhotoCore(showHolograms);
        }
        public void TakePhotoToDiskAsync(bool showHolograms = false)
        {
            canSave = true;
            TakePhotoCore(showHolograms);
        }

        private void TakePhotoCore(bool showHolograms)
        {
            canTakePhoto.Value = false;

            PhotoCapture.CreateAsync(showHolograms, p =>
            {
                photoCapture = p;

                var cameraParameters = new CameraParameters
                {
                    cameraResolutionHeight = Resolution.height,
                    cameraResolutionWidth = Resolution.width,
                    hologramOpacity = showHolograms ? 0.9f : 0,
                    pixelFormat = canSave ? CapturePixelFormat.BGRA32 : CapturePixelFormat.JPEG,
                };

                photoCapture.StartPhotoModeAsync(cameraParameters, onPhotoModeStartedCallback);
            });
        }

        private void onPhotoModeStartedCallback(PhotoCapture.PhotoCaptureResult result)
        {
            if(!result.success)
            {
                Debug.LogError("Unable to start photo mode");
                onPhotoCaptured.OnNext(null);
                photoCapture.StopPhotoModeAsync(onPhotoModeStoppedCallback);
                return;
            }

            if (canSave)
            {
                var filename = string.Format(@"Image_{0}.jpg", Time.time);
                filePath = System.IO.Path.Combine(PictureFileDirectoryPath(), filename);
                Debug.Log($"Photo save to {filePath}");
                photoCapture.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, onCapturedPhotoToDiskCallback);
            }
            else
            {
                photoCapture.TakePhotoAsync(onCapturedPhotoToMemoryCallback);
            }
        }
        private string PictureFileDirectoryPath()
        {
            string directorypath = "";
#if WINDOWS_UWP
            // HoloLens上での動作の場合、LocalAppData/AppName/LocalStateフォルダを参照する
            directorypath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#else
            // Unity上での動作の場合、Assets/StreamingAssetsフォルダを参照する
            directorypath = UnityEngine.Application.streamingAssetsPath;
#endif
            return directorypath;
        }

        private void onCapturedPhotoToDiskCallback(PhotoCapture.PhotoCaptureResult result)
        {
            if(!result.success)
            {
                Debug.LogError("Failed to save photo to disk");
            }

            photoCapture.StopPhotoModeAsync(onPhotoModeStoppedCallback);
        }

        private void onCapturedPhotoToMemoryCallback(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
        {
            if (!result.success)
            {
                Debug.LogError("Failed to take photo");
                onPhotoCaptured.OnNext(null);
                photoCapture.StopPhotoModeAsync(onPhotoModeStoppedCallback);
                return;
            }

            var buffer = new List<byte>();
            photoCaptureFrame.CopyRawImageDataIntoBuffer(buffer);


            // カメラの向きをワールド座標に変換するためのパラメータ保持
            Matrix4x4 cameraToWorldMatrix;
            photoCaptureFrame.TryGetCameraToWorldMatrix(out cameraToWorldMatrix);
            //var cameraRotation = Quaternion.LookRotation(-cameraToWorldMatrix.GetColumn(2), cameraToWorldMatrix.GetColumn(1));

            Matrix4x4 projectionMatrix;
            photoCaptureFrame.TryGetProjectionMatrix(Camera.main.nearClipPlane, Camera.main.farClipPlane, out projectionMatrix);
            var pixelToCameraMatrix = projectionMatrix.inverse;

            Plan = new ShootingPlan
            {
                Resolution = Resolution,
                ShootingLocation = Camera.main.transform.position,
                CameraToWorld = cameraToWorldMatrix,
                PixelToCamera = pixelToCameraMatrix,
            };

            photoCapture.StopPhotoModeAsync(onPhotoModeStoppedCallback);

            var value = buffer.ToArray();
            onPhotoCaptured.OnNext(value);
        }

        private void onPhotoModeStoppedCallback(PhotoCapture.PhotoCaptureResult result)
        {
            photoCapture.Dispose();
            photoCapture = null;

            canSave = false;
            canTakePhoto.Value = true;
        }

    }
}