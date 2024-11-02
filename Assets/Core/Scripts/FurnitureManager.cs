using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class FurnitureManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private GameObject[] _furniturePrefabs;
    [SerializeField] private Button _deleteButton;

    private GameObject _spawnedObject;
    private static List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    void Start()
    {
        _deleteButton.gameObject.SetActive(false);
    }

    public void SelectFurniture(int index)
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        if (_raycastManager.Raycast(screenCenter, _hits, TrackableType.PlaneWithinBounds))
        {
            Pose hitPose = _hits[0].pose;

            DeleteSpawnedObject();

            _spawnedObject = Instantiate(_furniturePrefabs[index], hitPose.position, hitPose.rotation);

            Vector3 cameraDirection = Camera.main.transform.forward;
            cameraDirection.y = 0;
            cameraDirection.x = 45;
            _spawnedObject.transform.rotation = Quaternion.LookRotation(cameraDirection);

            _deleteButton.gameObject.SetActive(true);
        }
    }

    public void DeleteSpawnedObject()
    {
        if (_spawnedObject != null)
        {
            Destroy(_spawnedObject);
            _spawnedObject = null;
            _deleteButton.gameObject.SetActive(false);
        }
    }
}
