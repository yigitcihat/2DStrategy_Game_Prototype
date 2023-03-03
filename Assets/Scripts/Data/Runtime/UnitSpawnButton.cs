using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnButton : MonoBehaviour
{
    public GameObject Unit;
    // clicked barracks
    public GameObject spawnerBuild;

    public void Spawn()
    {
        // get a spawn location from the spawner barracks
        GameObject spawnLocation = spawnerBuild.GetComponent<Build>().getSpawnLocation();
        if (spawnLocation == null)
        {
            return;
        }
        // spawn tank
        GameObject tmpUnit = Instantiate(Unit, spawnLocation.transform.position, spawnLocation.transform.rotation);
        tmpUnit.transform.SetParent(spawnLocation.transform);
        tmpUnit.GetComponent<Unit>().index = spawnLocation.GetComponent<GroundTile>().index;
        // set the location flags
        spawnLocation.GetComponent<GroundTile>().isOccupied = true;
        spawnLocation.GetComponent<GroundTile>().hasUnit = true;
    }
}
