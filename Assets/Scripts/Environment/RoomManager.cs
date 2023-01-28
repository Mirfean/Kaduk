using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    Room[] _roomList;
    public Room ActiveRoom;

    public static Action<Room> ChangeRoom;

    // Start is called before the first frame update
    void Start()
    {
        _roomList = gameObject.GetComponentsInChildren<Room>();
        ChangeRoom += ChangeActiveRoom;
    }

    void ChangeActiveRoom(Room newRoom)
    {
        SwitchActiveRoom(false);

        ActiveRoom = newRoom;

        SwitchActiveRoom(true);


    }

    void SwitchActiveRoom(bool status)
    {
        ActiveRoom.Active = status;
        ActiveRoom.ActiveAllEnemies(status);
    }

    void SwitchRoom(Room room, bool status)
    {
        room.Active = status;
        room.ActiveAllEnemies(status);
    }
}
