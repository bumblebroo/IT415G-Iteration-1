using System.Collections;
using UnityEngine;

public class ExperimentRunner : MonoBehaviour
{
    [SerializeField]
    private ExperimentData entityData;

    [SerializeField]
    private int entityStepAmount = 100;

    [SerializeField]
    private Sorter[] sorters;

    private void Start() {
        entityData.SetEntityCount(entityStepAmount);
    }

    /*
    private IEnumerator RunExperiment() {
        for (int i = 0; i < sorters.Length; i++) {
            Sorter sorter = sorters[i];
            for (int ci = 0; ci < entityCounts.Count; ci++) {
                newEntitiesAmount.SetEntityCount(100);
                yield return null;
                samples.Clear();
                while (samples.Count < sampleCount) {
                    yield return null;
                }
                double averageTime = Average(samples);
                if (cancelDuration < averageTime) {
                    break;
                }
            }
            Destroy(detection.GameObject);
        }
    }*/

    private bool CheckSortCorrectness(ISortable[] sortables) {
        for (int i = 0; i < sortables.Length - 1; i++) {
            if (sortables[i].SortValue > sortables[i+1].SortValue) {
                return false;
            }
        }
        return true;
    }
}
