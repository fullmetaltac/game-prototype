using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public Camera cam;

    private Room room;

    private void Awake()
    {
        MapManager.InitMap();
        room = new Room(MapManager.center);
        room.Render();
        cam.transform.position = room.position;
        PlayerController.instance.playerModel.transform.position = room.position;
    }
    private void Start()
    {

    }

    private void Update()
    {
        
    }
}
