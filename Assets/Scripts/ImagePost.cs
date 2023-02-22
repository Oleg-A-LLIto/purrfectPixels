using UnityEngine;

public class ImagePost
{
    public ImagePost(string _uri)
    {
        URI = _uri;
    }
    public Sprite Image { private set; get; }
    public string URI { private set; get; }
    private PostManager Post;

    public void UpdateImage(Sprite _img)
    {
        Image = _img;
        if (Post != null)
        {
            Post.UpdateImage(Image);
        }
    }

    public void AssignPostManager(PostManager _post)
    {
        Post = _post;
        Post.UpdateImagePost(this);
    }

    public void UnassignPostManager(PostManager _post)
    {
        Post = null;
    }
}