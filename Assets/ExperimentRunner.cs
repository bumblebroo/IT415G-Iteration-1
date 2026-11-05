using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ExperimentRunner : MonoBehaviour
{
    [SerializeField]
    private EntityManager entityManager;

    [SerializeField]
    private int entityStepAmount = 100;

    [SerializeField]
    private int entitySteps = 10;

    [SerializeField]
    private Sorter[] sorters;

    private void Start() {
        StartCoroutine(RunExperiment());
    }

    
    private IEnumerator RunExperiment() {
        for (int i = 0; i < sorters.Length; i++) {
            Sorter sorter = sorters[i];
            for (int j = 0; j < 10; j++) {
                entityManager.SetEntityCount(entityStepAmount * j);
                yield return null;
                float startTime = Time.time;
                while (Time.time - startTime < 1f) {
                    yield return null;
                }
                /*
                samples.Clear();
                while (samples.Count < sampleCount) {
                    yield return null;
                }
                double averageTime = Average(samples);
                if (cancelDuration < averageTime) {
                    break;
                }
                */
            }
        }
    }

    private bool CheckSortCorrectness(ISortable[] sortables) {
        for (int i = 0; i < sortables.Length - 1; i++) {
            if (sortables[i].SortValue > sortables[i+1].SortValue) {
                return false;
            }
        }
        return true;
    }
}
