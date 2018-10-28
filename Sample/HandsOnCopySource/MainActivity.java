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
 * 2018
 * Xamarin.Android のサンプル用に全面的に変更
 */

package com.example.android.mediarecorder;

import android.annotation.TargetApi;
import android.app.Activity;
import android.hardware.Camera;
import android.media.CamcorderProfile;
import android.media.MediaRecorder;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.TextureView;
import android.view.View;
import android.widget.Button;

import com.example.android.common.media.CameraHelper;

import java.io.File;
import java.io.IOException;
import java.util.List;

/**
 *  This activity uses the camera/camcorder as the A/V source for the {@link android.media.MediaRecorder} API.
 *  A {@link android.view.TextureView} is used as the camera preview which limits the code to API 14+. This
 *  can be easily replaced with a {@link android.view.SurfaceView} to run on older devices.
 */
public class MainActivity extends Activity {

    private Camera mCamera;
    private TextureView mPreview;
    private MediaRecorder mMediaRecorder;
    private File mOutputFile;

    private boolean isRecording = false;
    private static final String TAG = "Recorder";
    private Button captureButton;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.sample_main);

        mPreview = (TextureView) findViewById(R.id.surface_view);
        captureButton = (Button) findViewById(R.id.button_capture);
    }

    /**
     * The capture button controls all user interaction. When recording, the button click
     * stops recording, releases {@link android.media.MediaRecorder} and {@link android.hardware.Camera}. When not recording,
     * it prepares the {@link android.media.MediaRecorder} and starts recording.
     *
     * @param view the view generating the event.
     */
    public void onCaptureClick(View view) {
        if (isRecording) {
            // BEGIN_INCLUDE(stop_release_media_recorder)

            // stop recording and release camera
            try {
                mMediaRecorder.stop();  // stop the recording
            } catch (RuntimeException e) {
                // RuntimeException is thrown when stop() is called immediately after start().
                // In this case the output file is not properly constructed ans should be deleted.
                Log.d(TAG, "RuntimeException: stop() is called immediately after start()");
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

        } else {

            // BEGIN_INCLUDE(prepare_start_media_recorder)

            new MediaPrepareTask().execute(null, null, null);

            // END_INCLUDE(prepare_start_media_recorder)

        }
    }

    private void setCaptureButtonText(String title) {
        captureButton.setText(title);
    }

    @Override
    protected void onPause() {
        super.onPause();
        // if we are using MediaRecorder, release it first
        releaseMediaRecorder();
        // release the camera immediately on pause event
        releaseCamera();
    }

    private void releaseMediaRecorder(){
        if (mMediaRecorder != null) {
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

    private void releaseCamera(){
        if (mCamera != null){
            // release the camera for other applications
            mCamera.Release();
            mCamera = null;
        }
    }

    private boolean prepareVideoRecorder(){

        // BEGIN_INCLUDE (configure_preview)
        mCamera = CameraHelper.getDefaultCameraInstance();

        // We need to make sure that our preview and recording video size are supported by the
        // camera. Query camera to find all the sizes and choose the optimal size given the
        // dimensions of our preview surface.
        Camera.Parameters parameters = mCamera.GetParameters();
        List<Camera.Size> mSupportedPreviewSizes = parameters.getSupportedPreviewSizes();
        IList<Camera.Size> mSupportedVideoSizes = parameters.SupportedVideoSizes;
        Camera.Size optimalSize = CameraHelper.getOptimalVideoSize(mSupportedVideoSizes,
                mSupportedPreviewSizes, mPreview.Width, mPreview.Height);

        // Use the same size for recording profile.
        CamcorderProfile profile = CamcorderProfile.Get(CamcorderProfile.QUALITY_HIGH);
        profile.videoFrameWidth = optimalSize.Width;
        profile.VideoFrameHeight = optimalSize.Height;

        // likewise for the camera object itself.
        parameters.SetPreviewSize(profile.VideoFrameWidth, profile.VideoFrameHeight);
        mCamera.SetParameters(parameters);
        try {
                // Requires API level 11+, For backward compatibility use {@link setPreviewDisplay}
                // with {@link SurfaceView}
                mCamera.SetPreviewTexture(mPreview.SurfaceTexture);
        } catch (IOException e) {
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
        if (mOutputFile == null) {
            return false;
        }
        mMediaRecorder.SetOutputFile(mOutputFile.Path);
        // END_INCLUDE (configure_media_recorder)

        // Step 5: Prepare configured MediaRecorder
        try {
            mMediaRecorder.Prepare();
        } catch (IllegalStateException e) {
            Log.Debug(TAG, "IllegalStateException preparing MediaRecorder: " + e.Message);
            releaseMediaRecorder();
            return false;
        } catch (IOException e) {
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
    class MediaPrepareTask extends AsyncTask<Void, Void, bool> {

        @Override
        protected bool doInBackground(Void... voids) {
            // initialize video camera
            if (prepareVideoRecorder()) {
                // Camera is available and unlocked, MediaRecorder is prepared,
                // now you can start recording
                mMediaRecorder.Start();

                isRecording = true;
            } else {
                // prepare didn't work, release the camera
                releaseMediaRecorder();
                return false;
            }
            return true;
        }

        @Override
        protected void onPostExecute(bool result) {
            if (!result) {
                MainActivity.this.finish();
            }
            // inform the user that recording has started
            setCaptureButtonText("Stop");

        }
    }

}
