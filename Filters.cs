using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using TMPro;

public class Filters : MonoBehaviour
{
    public PNGToPDFConverter pdf;

    public Texture2D inputTexture; // Assign your input texture here
    public Texture2D psuedoTexture; // Assign your input texture here
    public RawImage dispImage;
    public RawImage bwButton;
    public RawImage noneButton;
    public RawImage noneHCButton;
    public RawImage bwHCButton;
    public TMP_Text text;
    public bool isConvert = true;

    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (dispImage.gameObject.activeSelf&&isConvert)
        {
            text.gameObject.SetActive(true);
            text.text = "no. of. pages: "+pdf.pngImages.Count.ToString();
        }
    }

    public Texture2D BlackAndWhite(Texture2D inputTex)
    {
        // Convert the input texture to a Mat
        Mat inputMat = new Mat(inputTexture.height, inputTexture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(inputTexture, inputMat);

        // Convert the Mat to grayscale
        Mat grayMat = new Mat();
        Imgproc.cvtColor(inputMat, grayMat, Imgproc.COLOR_RGBA2GRAY);

        // Convert the grayscale Mat back to a Texture2D
        Texture2D outputTexture = new Texture2D(grayMat.cols(), grayMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(grayMat, outputTexture);
        return outputTexture;
        // Apply the black and white texture to a GameObject or use it as needed
        //GetComponent<Renderer>().material.mainTexture = outputTexture;
    }
    public Texture2D IncreaseContrast(Texture2D inputTex)
    {
        // Convert the input texture to a Mat
        Mat inputMat = new Mat(inputTex.height, inputTex.width, CvType.CV_8UC4);
        Utils.texture2DToMat(inputTex, inputMat);

        // Convert the Mat to grayscale
        Mat grayMat = new Mat();
        Imgproc.cvtColor(inputMat, grayMat, Imgproc.COLOR_RGBA2GRAY);

        // Apply histogram equalization to increase contrast
        Mat equalizedMat = new Mat();
        Imgproc.equalizeHist(grayMat, equalizedMat);

        // Convert the equalized Mat back to a Texture2D
        Texture2D outputTexture = new Texture2D(equalizedMat.cols(), equalizedMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(equalizedMat, outputTexture);

        return outputTexture;
    }
    public Texture2D ColorContrast(Texture2D inputTex, float contrastFactor)
    {
        Mat inputMat = new Mat(inputTex.height, inputTex.width, CvType.CV_8UC4);
        Utils.texture2DToMat(inputTex, inputMat);

        // Convert the Mat to a Texture2D
        Texture2D outputTexture = new Texture2D(inputMat.cols(), inputMat.rows(), TextureFormat.RGBA32, false);

        // Apply contrast adjustment to each pixel
        for (int y = 0; y < inputMat.rows(); y++)
        {
            for (int x = 0; x < inputMat.cols(); x++)
            {
                // Get the color of the pixel
                Color color = inputTex.GetPixel(x, y);

                // Adjust contrast of each channel
                color.r = (color.r - 0.5f) * contrastFactor + 0.5f;
                color.g = (color.g - 0.5f) * contrastFactor + 0.5f;
                color.b = (color.b - 0.5f) * contrastFactor + 0.5f;

                // Clamp the color values to [0, 1]
                color.r = Mathf.Clamp01(color.r);
                color.g = Mathf.Clamp01(color.g);
                color.b = Mathf.Clamp01(color.b);

                // Set the color of the pixel in the output texture
                outputTexture.SetPixel(x, y, color);
            }
        }

        // Apply changes to the output texture
        outputTexture.Apply();

        return outputTexture;
    }

    public void BlackWhiteApply()
    {
        dispImage.texture = BlackAndWhite(psuedoTexture);
    }
    public void NoneApply()
    {
        dispImage.texture = psuedoTexture;
    }
    public void BWHC()
    {
        dispImage.texture = IncreaseContrast(psuedoTexture);
    }
    public void HC()
    {
        dispImage.texture = ColorContrast(psuedoTexture,2.5f);
    }
    public Texture2D textureToConvert; // Assign your Texture2D object here

    public void Save()
    {
        //ConvertAndSaveTextureToPNG(dispImage.texture as Texture2D, "Assets/output1.png");
        pdf.pngImages.Add(dispImage.texture as Texture2D);
    }

    void ConvertAndSaveTextureToPNG(Texture2D texture, string filePath)
    {
        // Encode the texture to PNG format
        byte[] pngBytes = texture.EncodeToPNG();

        // Save the PNG bytes to a file
        System.IO.File.WriteAllBytes(filePath, pngBytes);

        Debug.Log("Texture saved as PNG: " + filePath);
    }
}
