using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(UnitInfoScript))]
public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> unitPrefabs;

    private UnitInfoScript unitInfo;
    void Start()
    {
        unitInfo = gameObject.GetComponent<UnitInfoScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeployOnSelf() {
        for(int i = 0; i < unitPrefabs.Count; i++) {
            Deploy(unitInfo.GetLaneIndex(), unitInfo.GetTileIndex(), i);
        }
    }

    public void Deploy(int lane, int tile, int unitIndex) {
        if(unitIndex < 0 || unitIndex >= unitPrefabs.Count) {
            Debug.LogError("Invalid unit index");
            return;
        }
        GameObject unitPrefab = unitPrefabs[unitIndex];
        LaneManager.Instance.Deploy(lane, tile, unitPrefab, unitInfo.player, unitInfo.type=="Building");
    }
}
