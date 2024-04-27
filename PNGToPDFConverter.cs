using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class PNGToPDFConverter : MonoBehaviour
{
    // List of PNG files to convert
    public List<Texture2D> pngImages;
     public string outputPath;
     public string name;
    public EmailSender email;
    public Filters fil;

    void Start()
    {
        
    }

    void ConvertPNGsToPDF(List<Texture2D> images, string outputFileName)
    {
        // Create a new PDF document
        Document document = new Document();
        outputPath = UnityEngine.Application.persistentDataPath + "/" + outputFileName;
        // Define the output file path


        // Create a PDF writer instance
        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));

        // Open the document for writing
        document.Open();

        // Add each PNG image as a page to the PDF document
        foreach (Texture2D image in images)
        {
            // Convert Texture2D to uncompressed format
            Texture2D uncompressedTexture = image;

            // Convert uncompressed Texture2D to PNG bytes
            byte[] pngBytes = uncompressedTexture.EncodeToPNG();

            // Create iTextSharp Image from PNG bytes
            iTextSharp.text.Image itextImage = iTextSharp.text.Image.GetInstance(pngBytes);

            // Set the page size to match the image dimensions
            document.SetPageSize(new Rectangle(0, 0, uncompressedTexture.width, uncompressedTexture.height));
            document.NewPage();

            // Add the image to the PDF document
            document.Add(itextImage);

            // Destroy the temporary uncompressed Texture2D
            Destroy(uncompressedTexture);
        }

        // Close the document
        document.Close();

        UnityEngine.Debug.Log("PDF file saved at: " + outputPath);
    }

    Texture2D ConvertToUncompressedTexture(Texture2D compressedTexture)
    {
        // Create a new Texture2D with the same dimensions and uncompressed format
        Texture2D uncompressedTexture = new Texture2D(compressedTexture.width, compressedTexture.height, TextureFormat.RGBA32, false);

        // Copy pixel data from compressed texture to uncompressed texture
        uncompressedTexture.SetPixels(compressedTexture.GetPixels());

        // Apply changes
        uncompressedTexture.Apply();

        return uncompressedTexture;
    }

    /*void SetTexturesReadable(List<Texture2D> textures)
    {
        foreach (Texture2D texture in textures)
        {
            string assetPath = AssetDatabase.GetAssetPath(texture);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.isReadable = true;
                AssetDatabase.ImportAsset(assetPath);
            }
            else
            {
                UnityEngine.Debug.LogError("Failed to get TextureImporter for texture: " + texture.name);
            }
        }
    }*/

   public void ConvertOnScreen()
    {
        //SetTexturesReadable(pngImages);

        ConvertPNGsToPDF(pngImages, name);
        fil.text.text = outputPath;
        fil.isConvert = false;
        //email.SendEmail();
        //Application.OpenURL("com.google.android.gm");
    }
}
