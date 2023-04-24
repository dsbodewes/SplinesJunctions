using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OpenCvSharp;
//using System.Runtime.InteropServices;

using UnityEngine.UI;

// Parallel computation support
using Uk.Org.Adcock.Parallel;
using System;
using System.Runtime.InteropServices;


public class OpenCVTest : MonoBehaviour
{
    public string deviceName;
    [SerializeField] int desiredDevId = 0;
    private int devId = -1;


	// Video parameters
	public Image WebCamImage;
	public Image ProcessedImage;
    private WebCamTexture _webcamTexture;

	// Video size
    private const int imWidth = 640; //1280;
    private const int imHeight = 480; //720;
    private int imFrameRate;

    // OpenCVSharp parameters
    private Mat videoSourceImage;
	private Mat destImage;
    private Texture2D processedTexture;
    private Vec3b[] videoSourceImageData;
	//private byte[] destImageData;
	private Vec3b[] destImageData;
	

    // Frame rate parameter
    //private int updateFrameCount = 0;
    private int textureCount = 0;
    private int displayCount = 0;

	public int Threshold = 40;

    // Use this for initialization
    void Start () {
        WebCamDevice[] devices = WebCamTexture.devices;

        for(int i=0 ; i<devices.Length ; i++ ) {
            print(devices[i].name);

            if ( devices[i].name.CompareTo( deviceName ) == 0 || i == desiredDevId) {
                devId = i;
            }
        }
		
        if (devId >= 0) {
            _webcamTexture = new WebCamTexture(devices[devId].name , imWidth, imHeight, 60);

			// Play the video source
			_webcamTexture.Play();

			// initialize video / image with given size
			videoSourceImage = new Mat(imHeight, imWidth, MatType.CV_8UC3);  //MatType.CV_8UC3
			videoSourceImageData = new Vec3b[imHeight * imWidth];

			destImage = new Mat(imHeight, imWidth, MatType.CV_8UC3); //MatType.CV_8UC1);
            //destImageData = new byte[imHeight * imWidth];
			destImageData = new Vec3b[imHeight * imWidth];

            // create processed video texture as Texture2D object
            processedTexture = new Texture2D(imWidth, imHeight, TextureFormat.RGBA32, true, true);
        }

		// create opencv window to display the original video
        Cv2.NamedWindow("Copy video");
    }

    void Update() {

		WebCamImage.material.mainTexture = _webcamTexture;
		ProcessedImage.material.mainTexture = processedTexture;


		if (_webcamTexture.isPlaying) {

            if (_webcamTexture.didUpdateThisFrame) {

                textureCount++;

                // convert texture of original video to OpenCVSharp Mat object
                TextureToMat();
                // update the opencv window of source video
                //UpdateWindow(videoSourceImage);
                // create the canny edge image out of source image
                ProcessImage(videoSourceImage);
                // convert the OpenCVSharp Mat of canny image to Texture2D
                // the texture will be displayed automatically
                MatToTexture();
				// update the opencv window of processed image
				UpdateWindow(destImage);
            }

        }
        else {
            Debug.Log("Can't find camera!");
        }
    }




	// Convert Unity Texture2D object to OpenCVSharp Mat object
    void TextureToMat() {
        // Color32 array : r, g, b, a
        Color32[] c = _webcamTexture.GetPixels32();
		
        // Parallel for loop
        // convert Color32 object to Vec3b object
        // Vec3b is the representation of pixel for Mat
        Parallel.For(0, imHeight, i => {
            for (var j = 0; j < imWidth; j++) {
                var col = c[j + i * imWidth];
                Vec3b vec3 = new Vec3b {
                    Item0 = col.b,
                    Item1 = col.g,
                    Item2 = col.r
                };
                // set pixel to an array
                videoSourceImageData[j + i * imWidth] = vec3;
            }
        });
        // assign the Vec3b array to Mat
        //videoSourceImage.SetArray(0, 0, videoSourceImageData);
		videoSourceImage.SetArray<Vec3b>(videoSourceImageData);
    }



	// Convert OpenCVSharp Mat object to Unity Texture2D object
    void MatToTexture() {
		//destImage.GetArray<byte>(out destImageData);		//GrayScale
		destImage.GetArray<Vec3b>(out destImageData);

        // create Color32 array that can be assigned to Texture2D directly
        Color32[] c = new Color32[imHeight * imWidth];

        // parallel for loop
        Parallel.For(0, imHeight, i => {
            for (var j = 0; j < imWidth; j++) {
				//byte vec = destImageData[j + i * imWidth];
				Vec3b vec = destImageData[j + i * imWidth];
				var color32 = new Color32 {
					/*r = vec,
                    g = vec,
                    b = vec,*/
					r = vec.Item2,
					g = vec.Item1,
					b = vec.Item0,
                    a = 0
                };
				c[j + i * imWidth] = color32;
			}
        });

        processedTexture.SetPixels32(c);
        // to update the texture, OpenGL manner
        processedTexture.Apply();
    }


	// Simple example of canny edge detect
    void ProcessImage(Mat _image) {
		
		
		//Todo/lezen:
			//https://github.com/kaixindelele/OpenCV-real-world-red-cube-detection
			//https://blog.pollithy.com/python/numpy/detect-orientation-of-cube-opencv
			//https://stackoverflow.com/questions/9321351/which-algorithm-can-i-use-for-quadrilater-cube-detection
			//https://www.youtube.com/watch?v=ytvO2dijZ7A
			//https://stackoverflow.com/questions/8667818/opencv-c-obj-c-detecting-a-sheet-of-paper-square-detection/8863060#8863060

	
	

		//--------------------------------------------------------------------------------------------------
		/*
		var gray = new Mat();
		Cv2.CvtColor(_image, gray, ColorConversionCodes.BGR2GRAY);

		var corners = new Mat();
		Cv2.CornerHarris(gray, corners, 2, 3, 0.04);

		var dst_norm = new Mat();
		var dst_norm_scaled = new Mat();

		Cv2.Normalize(corners, dst_norm, 0, 255, NormTypes.MinMax, MatType.CV_32FC1, new Mat());
		Cv2.ConvertScaleAbs(dst_norm, dst_norm_scaled);

		int thresh = 180;


		Cv2.CopyTo(_image, destImage);
		//var newImage = new Mat();
		for( int i = 0; i < dst_norm.Rows ; i++ ){
			for( int j = 0; j < dst_norm.Cols; j++ ){
				if( (int) dst_norm.At<float>(i,j) > thresh ){
					Cv2.Circle(j, i, 5, Scalar.Red, 2, LineTypes.Link8, 0);
				}
			}
		}*/
		//--------------------------------------------------------------------------------------------------



		//--------------------------------------------------------------------------------------------------
		/*var gray = new Mat();
		Cv2.CvtColor(_image, gray, ColorConversionCodes.BGR2GRAY);

		Cv2.BitwiseNot(gray, gray);
		Point[][] contours;
		HierarchyIndex[] hiearachy;
		Cv2.FindContours(gray, out contours, out hiearachy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

		Cv2.CopyTo(_image, destImage);
		Cv2.DrawContours(destImage, contours, 0, Scalar.Red);*/
		//--------------------------------------------------------------------------------------------------



		//--------------------------------------------------------------------------------------------------
		/*Cv2.Flip(_image, _image, FlipMode.X);*/
		//Cv2.CopyTo(_image, destImage);
		//--------------------------------------------------------------------------------------------------



		//--------------------------------------------------------------------------------------------------
		//Example: FAST corner detection algorithm
		//https://elbruno.com/2020/11/13/dotnet-detecting-corners-on-the-%F0%9F%8E%A6-camera-feed-with-fast-algorithm-opencv-and-net5/
		var newImage = new Mat();
		var outputImage = new Mat();

		Cv2.CvtColor(_image, newImage, ColorConversionCodes.BGR2GRAY, 0);
		outputImage = newImage;

		KeyPoint[] keypoints = Cv2.FAST(newImage, Threshold, false);

		/*Cv2.CornerEigenValsAndVecs(newImage, destImage, 2, 3);*/
		Cv2.CornerHarris(newImage, destImage, 2, 3, 0.04);

		Cv2.Normalize(destImage, destImage, 0, 255, NormTypes.MinMax, MatType.CV_32FC1);
		Cv2.ConvertScaleAbs(destImage, destImage);

		for (int i = 0; i < destImage.Rows; i++)
		{
			for (int j = 0; j < destImage.Cols; j++)
			{
				if((int) destImage.At<float>(i,j) > Threshold)
				{
					Cv2.Circle(outputImage, new Point(j, i), 5, Scalar.Red, 2);
				}
			}
		}

		/*Cv2.CvtColor(outputImage, outputImage, ColorConversionCodes.GRAY2BGR);*/

		/*foreach (KeyPoint kp in keypoints){
			_image.Circle((Point)kp.Pt, 3, Scalar.Red, 1, LineTypes.AntiAlias, 0);
		}
		Cv2.CopyTo(_image, destImage);*/
		Cv2.CvtColor(outputImage, outputImage, ColorConversionCodes.GRAY2BGR);

		outputImage.ConvertTo(outputImage, MatType.CV_8UC3);

		//Cv2.DrawKeypoints(_image, keypoints, destImage, Scalar.Red);
		//Cv2.DrawKeypoints(destImage, keypoints, destImage, Scalar.Red);

		destImage = outputImage;

		/*destImage = outputImage;*/

		/*destImage.SetArray<Vec3b>(destImageData);*/
		//--------------------------------------------------------------------------------------------------








		//--------------------------------------------------------------------------------------------------
		//FindChessboardCorners
		//DrawChessboardCorners
		//--------------------------------------------------------------------------------------------------




		//--------------------------------------------------------------------------------------------------
		/*
		//Canny
        Cv2.Flip(_image, _image, FlipMode.X);
        Cv2.Canny(_image, destImage, 100, 100);
		*/
		//--------------------------------------------------------------------------------------------------
	}

	// Display the original video in a opencv window
    void UpdateWindow(Mat _image) {
        Cv2.Flip(_image, _image, FlipMode.X);
        Cv2.ImShow("Copy video", _image);
        displayCount++;
    }

	// close the opencv window
    public void OnDestroy() {
        Cv2.DestroyAllWindows();
    }

}
