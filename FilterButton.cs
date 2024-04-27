using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterButton : MonoBehaviour
{
    public Filters filter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {
        filter.bwButton.gameObject.SetActive(true);
        filter.noneButton.gameObject.SetActive(true);
        filter.noneHCButton.gameObject.SetActive(true);
        filter.bwHCButton.gameObject.SetActive(true);
        filter.inputTexture = filter.dispImage.texture as Texture2D;
        filter.psuedoTexture = filter.inputTexture;
        filter.noneButton.texture = filter.inputTexture;
        filter.bwButton.texture = filter.BlackAndWhite(filter.psuedoTexture);
        filter.bwHCButton.texture = filter.IncreaseContrast(filter.psuedoTexture);
        filter.noneHCButton.texture = filter.ColorContrast(filter.psuedoTexture, 2.5f);
    }
}
