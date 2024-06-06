using UnityEngine;

public class TransparencyState: MonoBehaviour
{

    public GameObject solidBody;
    public GameObject transparentBody;

    void Awake()
    {
        ShowSolid();   
    }

    public void ShowTransparent()
    {
        solidBody.SetActive(false);
        transparentBody.SetActive(true);
    }

    public void ShowSolid()
    {
        solidBody.SetActive(true);
        transparentBody.SetActive(false);
    }
}
