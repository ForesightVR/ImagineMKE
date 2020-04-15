using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class RoomSelection : MonoBehaviour
{
    public static RoomSelection Instance;
    public GameObject roomSpawner;
    public GameObject roomPrefab;
    public List<Room> roomUIs;
    public List<string> possibleRoomNames;
    
    Stack<string> namesToUse;
    int maxNumName;
    int nameLoop = 0;

    [Header("UI")]
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI roomDescription;
    public TextMeshProUGUI roomWarning;

    private void Awake()
    {
        Instance = this;
        maxNumName = possibleRoomNames.Count;
        ResetNames();
    }

    public void UpdateRoomInfo(string _roomName, string _roomDescription)
    {
        roomName.text = _roomName;
        roomDescription.text = _roomDescription;
    }
    public string GetNewRoomName()
    {
        if (namesToUse.Count == 0)
        {
            ResetNames();
        }

        string baseName = namesToUse.Pop();
        return $"{baseName} {nameLoop}";

    }
    public void ReturnRoomName(string roomName)
    {
        //check if the number of the room is different than the current loop
        int.TryParse(roomName.Substring(roomName.Length - 1, 1), out int returnRoomLoop);
        string baseName = roomName.Substring(0, roomName.Length - 2);

        if (returnRoomLoop != nameLoop) //if it is and the namesToUse == possibleRoomNames.Count, decrement the nameLoop and add the this roomName to the list
        {
            if (namesToUse.Count == maxNumName)
            {
                nameLoop--;
                namesToUse.Clear();
                namesToUse.Push(baseName);
            }
        }
        else   //else add it on top of the stack
        {
            namesToUse.Push(baseName);
        }
    }
    public Room CreateNewRoomUI()
    {
        Room room = Instantiate(roomPrefab, roomSpawner.transform).GetComponent<Room>();
        roomUIs.Add(room);
        roomWarning.gameObject.SetActive(false);
        room.roomSelection = this;
        return room;
    }
    public void RemoveRoomUI(Room room)
    {
        roomUIs.Remove(room);
        ReturnRoomName(room.roomName);
        Destroy(room);

        if (roomUIs.Count == 0)
            roomWarning.gameObject.SetActive(true);
    }
    void ResetNames()
    {
        nameLoop++;
        //namesToUse = new Stack<string>(possibleRoomNames);

        //check the room list for the names already being used at the current index
        IEnumerable<string> roomNames = NetworkConnectionManager.Instance?.GetRoomList().Select(x => x.Name) ?? new List<string>();
        var usedNames = roomNames.Where(x => x.Contains(nameLoop.ToString())).ToList();

        //remove names from namesToUse that are being used
        namesToUse = new Stack<string>(possibleRoomNames.Except(usedNames));

        //if all of them are in use, do this method again
        if (namesToUse.Count == 0)
            ResetNames();
    }
    public void ClearRooms()
    {
        roomWarning.gameObject.SetActive(true);
        for (int i = roomUIs.Count; i > 0; i--)
        {
            RemoveRoomUI(roomUIs[i]);
        }
    }
}
