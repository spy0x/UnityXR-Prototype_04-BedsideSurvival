using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ceilingLamp;
    [SerializeField] private GameObject assistantDevice;
    [SerializeField] private Transform player;
    [SerializeField] AnchorPrefabSpawner anchorPrefabSpawner;

    private MRUKRoom room;
    private MRUKAnchor ceilingAnchor;
    private MRUKAnchor screen;

    private void OnEnable()
    {
        anchorPrefabSpawner.onPrefabSpawned.AddListener(OnPrefabAnchorsCreated);
    }

    private void OnDisable()
    {
        anchorPrefabSpawner.onPrefabSpawned.RemoveListener(OnPrefabAnchorsCreated);
    }

    private void OnPrefabAnchorsCreated()
    {
        Dictionary<MRUKAnchor, GameObject> screenAnchors = GetScreenAnchors();
        GameObject tv = GetBiggestScreen(screenAnchors);
        if (tv) tv.GetComponent<TVManager>().enabled = true;
    }

    private GameObject GetBiggestScreen(Dictionary<MRUKAnchor, GameObject> screenAnchors)
    {
        GameObject largestScreen = null;
        if (screenAnchors.Count > 0)
        {
            float maxVolume = 0f;
            foreach (var screen in screenAnchors.Values)
            {
                var meshRenderer = screen.GetComponentInChildren<MeshRenderer>();
                if (meshRenderer != null)
                {
                    float volume = meshRenderer.bounds.size.x * meshRenderer.bounds.size.y * meshRenderer.bounds.size.z;
                    if (volume > maxVolume)
                    {
                        maxVolume = volume;
                        largestScreen = screen;
                    }
                }
            }
        }
        return largestScreen;
    }

    private Dictionary<MRUKAnchor, GameObject> GetScreenAnchors()
    {
        Dictionary<MRUKAnchor, GameObject> screenAnchors = new Dictionary<MRUKAnchor, GameObject>();
        foreach (var anchor in anchorPrefabSpawner.AnchorPrefabSpawnerObjects)
        {
            if (anchor.Key.Label != MRUKAnchor.SceneLabels.SCREEN) continue;
            screenAnchors.Add(anchor.Key, anchor.Value);
        }

        return screenAnchors;
    }

    //Setted when mruk has finish creating the room

    public void SetRoom()
    {
        room = MRUK.Instance.GetCurrentRoom();
        ceilingAnchor = room.CeilingAnchor;
        if (ceilingLamp) PlaceCeilingLamp();
        if (assistantDevice) PlaceAssistantDevice();
    }


    private void PlaceCeilingLamp()
    {
        if (room.CeilingAnchor)
        {
            Vector3 centerPosition = ceilingAnchor.GetAnchorCenter();
            Instantiate(ceilingLamp, centerPosition, Quaternion.identity);
        }
    }

    private void PlaceAssistantDevice()
    {
        if (!room.HasAllLabels(MRUKAnchor.SceneLabels.TABLE)) return;
        LabelFilter filter = new LabelFilter(MRUKAnchor.SceneLabels.TABLE);
        if (room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.FACING_UP, 0.1f, filter, out Vector3 position,
                out Vector3 normal))
        {
            Instantiate(assistantDevice, position, Quaternion.identity);
        }
    }
}