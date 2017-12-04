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
using Android.Hardware;
using System.Collections.Generic;
using Java.IO;
using Environment = Android.OS.Environment;
using Android.Util;
using Java.Text;
using Java.Util;
namespace XAMediaRecorder
{
    /**
     * Camera related utilities.
     */
    public class CameraHelper
    {

        public static readonly int MEDIA_TYPE_IMAGE = 1;
        public static readonly int MEDIA_TYPE_VIDEO = 2;

        /**
         * Iterate over supported camera video sizes to see which one best fits the
         * dimensions of the given view while maintaining the aspect ratio. If none can,
         * be lenient with the aspect ratio.
         *
         * @param supportedVideoSizes Supported camera video sizes.
         * @param previewSizes Supported camera preview sizes.
         * @param w     The width of the view.
         * @param h     The height of the view.
         * @return Best match camera video size to fit in the view.
         */
        public static Camera.Size getOptimalVideoSize(IList<Camera.Size> supportedVideoSizes,
                IList<Camera.Size> previewSizes, int w, int h)
        {
            // Use a very small tolerance because we want an exact match.
            const double ASPECT_TOLERANCE = 0.1;
            double targetRatio = (double)w / h;

            // Supported video sizes list might be null, it means that we are allowed to use the preview
            // sizes
            IList<Camera.Size> videoSizes;
            if (supportedVideoSizes != null)
            {
                videoSizes = supportedVideoSizes;
            }
            else
            {
                videoSizes = previewSizes;
            }
            Camera.Size optimalSize = null;

            // Start with max value and refine as we iterate over available video sizes. This is the
            // minimum difference between view and camera height.
            double minDiff = Double.MaxValue;

            // Target view height
            int targetHeight = h;

            // Try to find a video size that matches aspect ratio and the target view size.
            // Iterate over all available sizes and pick the largest size that can fit in the view and
            // still maintain the aspect ratio.
            foreach (Camera.Size size in videoSizes)
            {
                double ratio = (double)size.Width / size.Height;
                if (Math.Abs(ratio - targetRatio) > ASPECT_TOLERANCE)
                    continue;
                if (Math.Abs(size.Height - targetHeight) < minDiff && previewSizes.Contains(size))
                {
                    optimalSize = size;
                    minDiff = Math.Abs(size.Height - targetHeight);
                }
            }

            // Cannot find video size that matches the aspect ratio, ignore the requirement
            if (optimalSize == null)
            {
                minDiff = Double.MaxValue;
                foreach (Camera.Size size in videoSizes)
                {
                    if (Math.Abs(size.Height - targetHeight) < minDiff && previewSizes.Contains(size))
                    {
                        optimalSize = size;
                        minDiff = Math.Abs(size.Height - targetHeight);
                    }
                }
            }
            return optimalSize;
        }

        /**
         * @return the default camera on the device. Return null if there is no camera on the device.
         */
        public static Camera getDefaultCameraInstance()
        {
            return Camera.Open();
        }


        /**
         * @return the default rear/back facing camera on the device. Returns null if camera is not
         * available.
         */
        public static Camera getDefaultBackFacingCameraInstance()
        {
            return getDefaultCamera((int)Camera.CameraInfo.CameraFacingBack);
        }

        /**
         * @return the default front facing camera on the device. Returns null if camera is not
         * available.
         */
        public static Camera getDefaultFrontFacingCameraInstance()
        {
            return getDefaultCamera((int)Camera.CameraInfo.CameraFacingFront);
        }


        /**
         *
         * @param position Physical position of the camera i.e Camera.CameraInfo.CAMERA_FACING_FRONT
         *                 or Camera.CameraInfo.CAMERA_FACING_BACK.
         * @return the default camera on the device. Returns null if camera is not available.
         */
        [Android.Annotation.TargetApi(Value = (int)Android.OS.Build.VERSION_CODES.Gingerbread)]
        private static Camera getDefaultCamera(int position)
        {
            // Find the total number of cameras available
            int mNumberOfCameras = Camera.NumberOfCameras;

            // Find the ID of the back-facing ("default") camera
            Camera.CameraInfo cameraInfo = new Camera.CameraInfo();
            for (int i = 0; i < mNumberOfCameras; i++)
            {
                Camera.GetCameraInfo(i, cameraInfo);
                if ((int)cameraInfo.Facing == position)
                {
                    return Camera.Open(i);

                }
            }

            return null;
        }

        /**
         * Creates a media file in the {@code Environment.DIRECTORY_PICTURES} directory. The directory
         * is persistent and available to other applications like gallery.
         *
         * @param type Media type. Can be video or image.
         * @return A file object pointing to the newly created file.
         */
        public static File getOutputMediaFile(int type)
        {
            // To be safe, you should check that the SDCard is mounted
            // using Environment.getExternalStorageState() before doing this.
            if (!Environment.ExternalStorageState.Equals(Environment.MediaMounted, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            File mediaStorageDir = new File(Environment.GetExternalStoragePublicDirectory(
                Environment.DirectoryPictures), "CameraSample");
            // This location works best if you want the created images to be shared
            // between applications and persist after your app has been uninstalled.

            // Create the storage directory if it does not exist
            if (!mediaStorageDir.Exists())
            {
                if (!mediaStorageDir.Mkdirs())
                {
                    Log.Debug("CameraSample", "failed to create directory");
                    return null;
                }
            }

            // Create a media file name
            String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss", Locale.Us).Format(new Date());
            File mediaFile;
            if (type == MEDIA_TYPE_IMAGE)
            {
                mediaFile = new File(mediaStorageDir.Path + File.Separator +
                        "IMG_" + timeStamp + ".jpg");
            }
            else if (type == MEDIA_TYPE_VIDEO)
            {
                mediaFile = new File(mediaStorageDir.Path + File.Separator +
                        "VID_" + timeStamp + ".mp4");
            }
            else
            {
                return null;
            }

            return mediaFile;
        }

    }

}
