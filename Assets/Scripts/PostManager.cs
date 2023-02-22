using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PostManager : MonoBehaviour
{
    [SerializeField] private Image imageContent;
    [SerializeField] private Text uri;
    [SerializeField] private Rect postOffsets;
    private ImageFeedBehaviour feed;
    private ImagePost imagePost;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void InjectFeed(ImageFeedBehaviour _f)
    {
        feed = _f;
    }

    public void UpdateImage(Sprite _image)
    {
        RectTransform RTransform = GetComponent<RectTransform>();
        imageContent.sprite = _image;
        float _height = _image.rect.height / (_image.rect.width / imageContent.rectTransform.rect.width);
        RTransform.sizeDelta = new Vector2(RTransform.sizeDelta.x, _height);
    }

    public void UpdateImagePost(ImagePost _ip)
    {
        imagePost = _ip;
        if(_ip != null)
        {
            uri.text = _ip.URI;
            UpdateImage(_ip.Image);
        }
    }

    public void DeleteImage()
    {
        if (feed != null)
        {
            feed.RemoveImage(imagePost);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
