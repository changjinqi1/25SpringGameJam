using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomCat : MonoBehaviour
{
    public Image targetImage;         
    public Sprite[] catSprites;        

    void Start()
    {
        if (targetImage != null && catSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, catSprites.Length); 
            targetImage.sprite = catSprites[randomIndex];         
        }
        else
        {
            Debug.LogWarning("Image or Sprite array not set on RandomCat.");
        }
    }
}
