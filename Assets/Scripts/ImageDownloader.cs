using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageDownloader : MonoBehaviour
{
    public Sprite failedImage;
    public Sprite loadingImage;
    private ErrorManager errManager;

    void Start()
    {
        errManager = FindObjectOfType<ErrorManager>();
    }

    public void DownloadPost(ImagePost _imagePost)
    {
        StartCoroutine(DownloadImage(_imagePost));
    }

    IEnumerator DownloadImage(ImagePost _imagePost)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(_imagePost.URI);
        _imagePost.UpdateImage(loadingImage);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            _imagePost.UpdateImage(failedImage);
            if(errManager != null)
            {
                errManager.ShowErrorMessage("We could not download your image due to the following error: " + www.error + 
                    "\nStill, we won't delete the post from your feed in case you want to keep the URI. We will also show you a cool cat instead of whatever you wanted to view.");
            }
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            _imagePost.UpdateImage(sprite);
        }
    }
}

