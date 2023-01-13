using System;
using System.Collections;
using System.Collections.Generic;
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

    void ChangeActiveRoom(Room room)
    {
        //Enemies
        ActiveRoom.ActiveAllEnemies(false);
        ActiveRoom = room;
        ActiveRoom.ActiveAllEnemies(true);


    }
}
