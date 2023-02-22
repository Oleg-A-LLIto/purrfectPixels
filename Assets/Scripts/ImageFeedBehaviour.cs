using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ImageDownloader))]
public class ImageFeedBehaviour : MonoBehaviour, IScrollable
{
    private ImageDownloader imgDownloader;
    private List<ImagePost> images;
    private ErrorManager errManager;
    [SerializeField] private List<PostManager> postManagers;
    [SerializeField] private string[] uris;
    [SerializeField] private Transform layoutContainer;
    private Vector3 layoutContainerInitPosition;
    private float plannedOffset;
    private int imageToPostOffset;

    void Start()
    {
        layoutContainerInitPosition = layoutContainer.localPosition;
        errManager = FindObjectOfType<ErrorManager>();
        plannedOffset = 0;
        imageToPostOffset = 0;
        images = new List<ImagePost>();
        imgDownloader = GetComponent<ImageDownloader>();
        foreach (string _uri in uris)
        {
            ImagePost _ip = new ImagePost(_uri);
            images.Add(_ip);
            imgDownloader.DownloadPost(_ip);
        }
        for(int i = 0; (i < postManagers.Count) && (i < images.Count); i++)
        {
            images[i].AssignPostManager(postManagers[i]);
        } 
        foreach(PostManager _p in postManagers)
        {
            _p.InjectFeed(this);
        }
    }

    public void AddNewImage(string _uri)
    {
        ImagePost _ip = new ImagePost(_uri);
        images.Add(_ip);
        imgDownloader.DownloadPost(_ip);
        imageToPostOffset = images.Count-1;
        RefreshPosts();
    }

    public void RemoveImage(ImagePost _ip)
    {
        if(images.Count == 1)
        {
            errManager.ShowErrorMessage("This is your very last image, we can't delete it! With our shenanigans, you can infinitely scroll just 1(!) image, but even we can't let you infinitely scroll just 0 of them!");
            return;
        }
        images.Remove(_ip);
        RefreshPosts();
    }

    void ScrollOver()
    {
        Transform _firstPostManager = transform.GetChild(0);
        Transform _lastPostManager = transform.GetChild(transform.childCount-1);
        RectTransform _lastTransform = _lastPostManager.GetComponent<RectTransform>();
        RectTransform _firstTransform = _firstPostManager.GetComponent<RectTransform>();
        if ((_lastTransform.position.y - _lastTransform.rect.height) > 0)
        {
            float _offset = _firstTransform.position.y - Screen.height - _firstTransform.rect.height;
            plannedOffset = _offset;

            _firstPostManager.SetAsLastSibling();
            PostManager _firstManagerComponent = postManagers[0];
            postManagers.Remove(_firstManagerComponent);
            postManagers.Add(_firstManagerComponent);

            imageToPostOffset += 1;
            RefreshPosts();

            layoutContainer.localPosition = new Vector3(layoutContainerInitPosition.x, layoutContainerInitPosition.y + plannedOffset, layoutContainerInitPosition.z); //Temporarily move the container in case the 
        }
        else if(_firstTransform.position.y < Screen.height)
        {
            _lastPostManager.SetAsFirstSibling();
            PostManager _lastManagerComponent = postManagers[postManagers.Count - 1];
            postManagers.Remove(_lastManagerComponent);
            postManagers.Insert(0, _lastManagerComponent);

            imageToPostOffset -= 1;
            
            if (imageToPostOffset < 0)
            {
                imageToPostOffset += images.Count;
            }

            RefreshPosts();

            float _offset = _lastTransform.rect.height;
            plannedOffset = _offset;
            layoutContainer.localPosition = new Vector3(layoutContainerInitPosition.x, layoutContainerInitPosition.y + plannedOffset, layoutContainerInitPosition.z);
        }
        //FirstPostManager.SetParent(null);
    }

    void RefreshPosts()
    {
        for(int i = 0; i < postManagers.Count; i++)
        {
            int _id = (int)(i + imageToPostOffset) % images.Count;
            images[_id].AssignPostManager(postManagers[i]);
        }
    }
    void OnGUI()
    {
        //Reordering on those has to happen after the layout component has finished reordering the elements
        //otherwise everything gets overwritten
        if (Event.current.type == EventType.Layout)
        {
            if(plannedOffset != 0)
            {
                layoutContainer.localPosition = new Vector3(layoutContainerInitPosition.x, layoutContainerInitPosition.y, layoutContainerInitPosition.z);
                Scroll(plannedOffset);
                plannedOffset = 0;
            }
        }
    }

    public void Scroll(float _amount)
    {
        foreach (PostManager _pm in postManagers)
        {
            _pm.transform.position = new Vector3(_pm.transform.position.x, _pm.transform.position.y + _amount, 0);
        }
        ScrollOver();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public interface IScrollable
{
    public void Scroll(float _amount);
}


