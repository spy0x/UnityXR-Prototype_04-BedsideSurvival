using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ceilingLamp;
    
    private MRUKRoom room;
    private MRUKAnchor ceilingAnchor;
    
    //Setted when mruk has finish creating the room
    
    public void SetRoom()
    {
        room = MRUK.Instance.GetCurrentRoom();
        ceilingAnchor = room.CeilingAnchor;
        PlaceCeilingLamp();
    }

    private void PlaceCeilingLamp()
    {
        if (room.CeilingAnchor)
        {
            Vector3 centerPosition = ceilingAnchor.GetAnchorCenter();
            Instantiate(ceilingLamp, centerPosition, Quaternion.identity);
        }
    }
}
