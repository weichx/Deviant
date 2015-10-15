#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ObstabcleSpawner : MonoBehaviour {

    public GameObject[] obstacleTypes;
    public int spawnCount = 10;
    public float spawnRange = 25;
    public float scaleRange = 5f;
    public bool useRandomUniformScale = true;
    public bool spawnAtPlayTime = false;

    void Awake() {
        if (spawnAtPlayTime) {
            GameObject spawnGroup = new GameObject();
            spawnGroup.name = "Spawn Group";
            spawnGroup.transform.parent = transform;
            var random = new System.Random();
            for (var i = 0; i < spawnCount; i++) {
                var typeIndex = random.Next(0, obstacleTypes.Length);
                var obstacle = Instantiate(obstacleTypes[typeIndex]);
                obstacle.transform.parent = spawnGroup.transform;
                obstacle.transform.name = obstacleTypes[typeIndex].transform.name + "Spawned(" + i + ")";
                Randomize(obstacle);
            }
        }
    }

    private void Randomize(GameObject obstacle) {
        obstacle.transform.localRotation = Random.rotation;
        obstacle.transform.localPosition = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));
        if (useRandomUniformScale) {
            var scale = Random.Range(1f, scaleRange);
            obstacle.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void EditorSpawn() {
        var random = new System.Random();
        GameObject spawnGroup = new GameObject();
        spawnGroup.name = "Spawn Group";
        spawnGroup.transform.parent = transform;
        for (var i = 0; i < spawnCount; i++) {
            var typeIndex = random.Next(0, obstacleTypes.Length);
            var obstacle = PrefabUtility.InstantiatePrefab(obstacleTypes[typeIndex]) as GameObject;
            obstacle.transform.parent = spawnGroup.transform;
            obstacle.transform.name = obstacleTypes[typeIndex].transform.name + "Spawned(" + i + ")";
            Randomize(obstacle);
        }
    }
}

[CustomEditor(typeof(ObstabcleSpawner))]
public class ObstacleSpawnEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        ObstabcleSpawner spawner = (ObstabcleSpawner)target;

        if (GUILayout.Button("Spawn")) {
            spawner.EditorSpawn();
        }

    }
}

#endif