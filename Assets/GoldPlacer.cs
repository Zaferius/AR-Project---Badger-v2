using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GoldSpawner : MonoBehaviour
{
    public GameObject goldPrefab;
    private ARPlaneManager planeManager;

    public int maxGoldPerPlane = 2;  
    public float eyeHeight = 0.2f;   

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
        planeManager.trackablesChanged.AddListener(OnPlanesChanged);
    }

    void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        foreach (var plane in args.added)
        {
            for (int i = 0; i < maxGoldPerPlane; i++)
            {
                Vector3 randomPoint = new Vector3(
                    Random.Range(-plane.size.x / 2, plane.size.x / 2),
                    0,
                    Random.Range(-plane.size.y / 2, plane.size.y / 2)
                );

                Vector3 worldPos = plane.transform.TransformPoint(plane.center + randomPoint);
                Vector3 spawnPos = worldPos + Vector3.up * eyeHeight;

                Instantiate(goldPrefab, spawnPos, Quaternion.identity);

            }
        }
    }
}