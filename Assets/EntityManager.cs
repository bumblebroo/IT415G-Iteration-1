using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

public class EntityManager : MonoBehaviour, ISortableProvider
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private Transform referenceTransform;

    private List<Entity> entities = new List<Entity>();

    [SerializeField]
    private float entitySpeed;

    [SerializeField]
    private float directionWeight;

    [SerializeField]
    private BoxCollider2D spawnArea;

    public UnityEvent FinishedUpdate;

    [SerializeField]
    private int testStartAmount;

    public ISortable[] GetISortables() {
        return entities.ToArray();
    }

    private void Start() {
        SpawnEntitiesToLimit(testStartAmount);
    }

    private void UpdateDistancesToReference() {
        Profiler.BeginSample("UpdateEntityDistances");
        for (int i = 0; i < entities.Count; i++) {
            entities[i].SortValue = Vector2.Distance(referenceTransform.position, entities[i].transform.position);
        }
        Profiler.EndSample();
    }

    private void MoveEntities() {
        Profiler.BeginSample("MoveEntitiesFunction");
        for (int i = 0; i < entities.Count; i++) {
            if (TouchesSpawnAreaCollider(entities[i].transform.position)) {
                entities[i].dir = (entities[i].dir * directionWeight + new Vector2(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f)).normalized;
            } else {
                entities[i].transform.position = GetRandomSpotWithinSpawnArea();
                entities[i].dir = new Vector2(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f).normalized;
            }

            MoveTransform(entities[i].transform, entities[i].dir * entitySpeed * Time.deltaTime);
        }
        Profiler.EndSample();
    }

    private Vector2 GetRandomSpotWithinSpawnArea() {
        Profiler.BeginSample("RandomSpotGeneration");
        Vector2 result = new Vector2(transform.position.x + UnityEngine.Random.Range(-spawnArea.size.x / 2, spawnArea.size.x / 2), transform.position.y + UnityEngine.Random.Range(-spawnArea.size.y / 2, spawnArea.size.y / 2));
        Profiler.EndSample();
        return result;
    }

    private void MoveTransform(Transform transform, Vector2 delta) {
        Profiler.BeginSample("MoveTransform");
        transform.Translate(delta);
        Profiler.EndSample();
    }

    private bool TouchesSpawnAreaCollider(Vector2 pos) {
        return spawnArea.OverlapPoint(pos);
    }

    private void Update() {
        Profiler.BeginSample("EntityUpdate");
        MoveEntities();
        UpdateDistancesToReference();
        Profiler.EndSample();
        FinishedUpdate?.Invoke();
    }

    public void SpawnEntitiesToLimit(int limit) {
        int amountToSpawn = limit - entities.Count;

        GameObject newInstance;
        SpriteRenderer sr;

        for (int i = 0; i < amountToSpawn; i++) {
            newInstance = Instantiate(prefab, GetRandomSpotWithinSpawnArea(), Quaternion.identity);
            sr = newInstance.GetComponent<SpriteRenderer>();
            entities.Add(new Entity(newInstance.transform, sr));
        }
    }
}

internal class Entity : ISortable {
    public Transform transform;
    public SpriteRenderer sr;
    public Vector2 dir;

    public float SortValue { get; set; }

    public Entity(Transform transform, SpriteRenderer sr) {
        this.transform = transform;
        this.sr = sr;
    }
}
