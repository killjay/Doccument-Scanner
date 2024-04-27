using UnityEngine;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using Rect = OpenCVForUnity.CoreModule.Rect;
using System.IO;
using UnityEngine.UI;

public class CropOutline : MonoBehaviour
{
    public Texture2D inputTexture; // Assign your input texture here
    public RawImage dispImage;
    public void Img2Start()
    {
        string filePath = Application.persistentDataPath + "output_display_area.png";
        inputTexture = LoadTextureFromFile(filePath);
        Mat inputMat = new Mat(inputTexture.height, inputTexture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(inputTexture, inputMat);

        // Convert to grayscale
        Mat grayMat = new Mat();
        Imgproc.cvtColor(inputMat, grayMat, Imgproc.COLOR_RGBA2GRAY);

        // Apply threshold
        Mat thresholdMat = new Mat();
        Imgproc.threshold(grayMat, thresholdMat, 1, 255, Imgproc.THRESH_BINARY);

        // Find contours
        List<MatOfPoint> contours = new List<MatOfPoint>();
        Mat hierarchy = new Mat();
        Imgproc.findContours(thresholdMat, contours, hierarchy, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);

        // Find the largest contour
        double maxArea = -1;
        int maxContourIdx = -1;
        for (int i = 0; i < contours.Count; i++)
        {
            double area = Imgproc.contourArea(contours[i]);
            if (area > maxArea)
            {
                maxArea = area;
                maxContourIdx = i;
            }
        }

        // Get bounding rectangle of the largest contour
        Rect boundingRect = Imgproc.boundingRect(contours[maxContourIdx]);

        // Crop the original image using the bounding rectangle
        Mat croppedMat = new Mat(inputMat, boundingRect);

        // Convert Mat to Texture2D
        Texture2D croppedTexture = new Texture2D(croppedMat.cols(), croppedMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(croppedMat, croppedTexture);

        // Display the cropped texture, save it, or use it as needed
        dispImage.gameObject.SetActive(true);
        dispImage.texture = croppedTexture;
        dispImage.SetNativeSize();
        if(dispImage.transform.localScale.x == 1)
        {
            dispImage.transform.localScale = new Vector3(dispImage.transform.localScale.x * 1.5f, dispImage.transform.localScale.y * 1.5f, dispImage.transform.localScale.z * 1.5f);

        }
    }
    Texture2D LoadTextureFromFile(string path)
    {
        // Read the bytes of the image file
        byte[] fileData = File.ReadAllBytes(path);

        // Create a new Texture2D and load the image data
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            return texture;
        }
        else
        {
            Debug.LogError("Failed to load image data from file: " + path);
            return null;
        }


    }
}