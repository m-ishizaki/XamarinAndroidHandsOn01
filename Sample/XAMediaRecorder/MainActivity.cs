/*
 * Copyright (C) 2013 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
/*
 * 2017
 * Xamarin.Android のサンプル用に全面的に変更
 */

using System;
using Android.App;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Views;
using Java.IO;
using Android.Media;
using Android.Widget;
using Java.Lang;
using Android.Util;
using System.Collections.Generic;
using String = System.String;

namespace XAMediaRecorder
{
    [Activity(Label = "@string/app_name", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        private Camera mCamera;
        private TextureView mPreview;
        private Android.Media.MediaRecorder mMediaRecorder;
        private File mOutputFile;

        private bool isRecording = false;
        private static readonly string TAG = "Recorder";
        private Button captureButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.sample_main);
            mPreview = (TextureView)FindViewById(Resource.Id.surface_view);
            captureButton = (Button)FindViewById(Resource.Id.button_capture);
        }

        [Java.Interop.Export("onCaptureClick")]
        public void OnCaptureClick(View view)
        {
            if (isRecording)
            {
                // BEGIN_INCLUDE(stop_release_media_recorder)

                // stop recording and release camera
                try
                {
                    mMediaRecorder.Stop();  // stop the recording
                }
                catch (RuntimeException e)
                {
                    // RuntimeException is thrown when stop() is called immediately after start().
                    // In this case the output file is not properly constructed ans should be deleted.
                    Log.Debug(TAG, "RuntimeException: stop() is called immediately after start()");
                    //noinspection ResultOfMethodCallIgnored
                    mOutputFile.Delete();
                }
                releaseMediaRecorder(); // release the MediaRecorder object
                mCamera.Lock();         // take camera access back from MediaRecorder

                // inform the user that recording has stopped
                setCaptureButtonText("Capture");
                isRecording = false;
                releaseCamera();
                // END_INCLUDE(stop_release_media_recorder)

            }
            else
            {

                // BEGIN_INCLUDE(prepare_start_media_recorder)

                new MediaPrepareTask(this).Execute(null, null, null);

                // END_INCLUDE(prepare_start_media_recorder)

            }

        }

        private void setCaptureButtonText(String title)
        {
            captureButton.Text = title;
        }

        protected override void OnPause()
        {
            base.OnPause();
            // if we are using MediaRecorder, release it first
            releaseMediaRecorder();
            // release the camera immediately on pause event
            releaseCamera();
        }

        private void releaseMediaRecorder()
        {
            if (mMediaRecorder != null)
            {
                // clear recorder configuration
                mMediaRecorder.Reset();
                // release the recorder object
                mMediaRecorder.Release();
                mMediaRecorder = null;
                // Lock camera for later use i.e taking it back from MediaRecorder.
                // MediaRecorder doesn't need it anymore and we will release it if the activity pauses.
                mCamera.Lock();
            }
        }

        private void releaseCamera()
        {
            if (mCamera != null)
            {
                // release the camera for other applications
                mCamera.Release();
                mCamera = null;
            }
        }

        private bool prepareVideoRecorder()
        {

            // BEGIN_INCLUDE (configure_preview)
            mCamera = CameraHelper.getDefaultCameraInstance();

            // We need to make sure that our preview and recording video size are supported by the
            // camera. Query camera to find all the sizes and choose the optimal size given the
            // dimensions of our preview surface.
            Camera.Parameters parameters = mCamera.GetParameters();
            IList<Camera.Size> mSupportedPreviewSizes = parameters.SupportedPreviewSizes;
            IList<Camera.Size> mSupportedVideoSizes = parameters.SupportedVideoSizes;
            Camera.Size optimalSize = CameraHelper.getOptimalVideoSize(mSupportedVideoSizes,
                    mSupportedPreviewSizes, mPreview.Width, mPreview.Height);

            // Use the same size for recording profile.
            CamcorderProfile profile = CamcorderProfile.Get(CamcorderQuality.High);
            profile.VideoFrameWidth = optimalSize.Width;
            profile.VideoFrameHeight = optimalSize.Height;

            // likewise for the camera object itself.
            parameters.SetPreviewSize(profile.VideoFrameWidth, profile.VideoFrameHeight);
            mCamera.SetParameters(parameters);
            try
            {
                // Requires API level 11+, For backward compatibility use {@link setPreviewDisplay}
                // with {@link SurfaceView}
                mCamera.SetPreviewTexture(mPreview.SurfaceTexture);
            }
            catch (IOException e)
            {
                Log.Error(TAG, "Surface texture is unavailable or unsuitable" + e.Message);
                return false;
            }
            // END_INCLUDE (configure_preview)


            // BEGIN_INCLUDE (configure_media_recorder)
            mMediaRecorder = new MediaRecorder();

            // Step 1: Unlock and set camera to MediaRecorder
            mCamera.Unlock();
            mMediaRecorder.SetCamera(mCamera);

            // Step 2: Set sources
            mMediaRecorder.SetAudioSource(AudioSource.Default);
            mMediaRecorder.SetVideoSource(VideoSource.Camera);

            // Step 3: Set a CamcorderProfile (requires API Level 8 or higher)
            mMediaRecorder.SetProfile(profile);

            // Step 4: Set output file
            mOutputFile = CameraHelper.getOutputMediaFile(CameraHelper.MEDIA_TYPE_VIDEO);
            if (mOutputFile == null)
            {
                return false;
            }
            mMediaRecorder.SetOutputFile(mOutputFile.Path);
            // END_INCLUDE (configure_media_recorder)

            // Step 5: Prepare configured MediaRecorder
            try
            {
                mMediaRecorder.Prepare();
            }
            catch (IllegalStateException e)
            {
                Log.Debug(TAG, "IllegalStateException preparing MediaRecorder: " + e.Message);
                releaseMediaRecorder();
                return false;
            }
            catch (IOException e)
            {
                Log.Debug(TAG, "IOException preparing MediaRecorder: " + e.Message);
                releaseMediaRecorder();
                return false;
            }
            return true;
        }

        /**
         * Asynchronous task for preparing the {@link android.media.MediaRecorder} since it's a long blocking
         * operation.
         */
        class MediaPrepareTask : AsyncTask<Java.Lang.Void, Java.Lang.Void, bool>
        {
            private readonly WeakReference<MainActivity> _activity;
            public MediaPrepareTask(MainActivity activity)
            {
                _activity = new WeakReference<MainActivity>(activity);
            }
            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] voids)
            {
                MainActivity activity;
                _activity.TryGetTarget(out activity);
                // initialize video camera
                if (activity.prepareVideoRecorder())
                {
                    // Camera is available and unlocked, MediaRecorder is prepared,
                    // now you can start recording
                    activity.mMediaRecorder.Start();

                    activity.isRecording = true;
                }
                else
                {
                    // prepare didn't work, release the camera
                    activity.releaseMediaRecorder();
                    return false;
                }
                return true;
            }
            protected override void OnPostExecute(bool result)
            {
                MainActivity activity;
                _activity.TryGetTarget(out activity);
                if (!result)
                {
                    activity.Finish();
                }
                // inform the user that recording has started
                activity.setCaptureButtonText("Stop");
            }
            protected override bool RunInBackground(params Java.Lang.Void[] @params)
            {
                throw new NotImplementedException();
            }
        }

    }
}

