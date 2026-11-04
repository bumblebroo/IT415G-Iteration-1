using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ExperimentData", menuName = "Scriptable Objects/ExperimentData")]
public class ExperimentData : ScriptableObject
{
    [SerializeField]
    private int[] entityCounts;

    private int currentEntityCount;

    private ISortable[] sortables;

    public UnityEvent<int> NewEntityCount;

    public int[] EntityCounts => entityCounts;

    public int CurrentEntityCount => currentEntityCount;

    public void SetEntityCount(int value) {
        currentEntityCount = value;
        NewEntityCount?.Invoke(value);
    }

    public void UpdateSortables(ISortable[] sortables) {
        this.sortables = sortables;
    }
}
