using UnityEngine;
using System.Collections.Generic;

public class TransparencyController : MonoBehaviour
{
    public Transform cam;
    public Transform player;
    public List<TransparencyState> currenltyInTheWay;
    public List<TransparencyState> alreadyTrasparent;


    private void Awake()
    {
        currenltyInTheWay = new List<TransparencyState>();
        alreadyTrasparent = new List<TransparencyState>();
    }

    void Update()
    {
        GelAllObjectsInTheWay();
        MakeObjectsSolid();
        MakeObjectsTransparent();
    }

    private void GelAllObjectsInTheWay()
    {
        currenltyInTheWay.Clear();

        var cameraToPlayerDistance = Vector3.Magnitude(cam.position - player.position);
        var ray1Forward = new Ray(cam.position, player.position - cam.position);
        var ray1Backward = new Ray(player.position, cam.position - player.position);

        var hits1Forward = Physics.RaycastAll(ray1Forward, cameraToPlayerDistance);
        var hits1Backward = Physics.RaycastAll(ray1Backward, cameraToPlayerDistance);

        //Debug.DrawRay(cam.position, player.position - cam.position, Color.green, 10f);
        //Debug.DrawRay(player.position, cam.position - player.position, Color.red, 10f);

        foreach (var hit in hits1Forward)
        {
            if(hit.collider.gameObject.TryGetComponent(out TransparencyState inTheWay))
            {
                if (!currenltyInTheWay.Contains(inTheWay))
                {
                    currenltyInTheWay.Add(inTheWay);
                }
            }
        }

        foreach (var hit in hits1Backward)
        {
            if (hit.collider.gameObject.TryGetComponent(out TransparencyState inTheWay))
            {
                if (!currenltyInTheWay.Contains(inTheWay))
                {
                    currenltyInTheWay.Add(inTheWay);
                }
            }
        }
    }

    private void MakeObjectsTransparent()
    {
        for (int i = 0; i < currenltyInTheWay.Count; i++)
        {
            TransparencyState inTheWay = currenltyInTheWay[i];

            if (!alreadyTrasparent.Contains(inTheWay))
            {
                inTheWay.ShowTransparent();
                alreadyTrasparent.Add(inTheWay);
            }
        }
    }

    private void MakeObjectsSolid()
    {
        for (int i = alreadyTrasparent.Count - 1; i>= 0; i--)
        {
            TransparencyState wasInTheWay = alreadyTrasparent[i];

            if (!currenltyInTheWay.Contains(wasInTheWay))
            {
                wasInTheWay.ShowSolid();
                alreadyTrasparent.Remove(wasInTheWay);
            }
        }
    }
}
