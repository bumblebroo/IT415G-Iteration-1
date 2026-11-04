using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Profiling;

public class EntityManager : MonoBehaviour
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

    public UnityEvent<ISortable[]> NewISortablesArray;

    private ObjectPool<Entity> entityPool;

    private void Awake() {
        entityPool = new ObjectPool<Entity>(
                createFunc: CreateEntity,
                actionOnGet: OnGetEntity,
                actionOnRelease: OnReleaseEntity,
                actionOnDestroy: OnDestroyEntity,
                defaultCapacity: 100,
                maxSize: int.MaxValue
            );
    }

    #region Pooling functions
    private Entity CreateEntity() {
        GameObject newInstance = Instantiate(prefab, GetRandomSpotWithinSpawnArea(), Quaternion.identity);
        SpriteRenderer sr = newInstance.GetComponent<SpriteRenderer>();
        return new Entity(newInstance.transform, sr);
    }

    private void OnGetEntity(Entity entity) {
        entity.transform.gameObject.SetActive(true);
    }

    private void OnReleaseEntity(Entity entity) {
        entity.transform.gameObject.SetActive(false);
    }

    private void OnDestroyEntity(Entity entity) {
        Destroy(entity.transform.gameObject);
    }
    #endregion

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

    public void UpdateEntities() {
        Profiler.BeginSample("EntityUpdate");
        MoveEntities();
        UpdateDistancesToReference();
        Profiler.EndSample();
    }

    public void SetEntityCount(int limit) {
        if(entities.Count > limit) {
            int newEntitiesAmount = entities.Count - limit;
            for (int i = 0; i < newEntitiesAmount; i++) {
                entities.Add(entityPool.Get());
            }
        } else if(entities.Count < limit) {
            for (int i = entities.Count; i < limit; i--) {
                Entity entity = entities[i];
                entities.Remove(entity);
                entityPool.Release(entity);
            }
        }

        NewISortablesArray?.Invoke(entities.ToArray());
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
