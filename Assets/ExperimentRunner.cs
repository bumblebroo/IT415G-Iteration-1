using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;

public class ExperimentRunner : MonoBehaviour
{
    [SerializeField]
    private EntityManager entityManager;

    [SerializeField]
    private int entityStepAmount = 100;

    [SerializeField]
    private int entitySteps = 10;

    [SerializeField]
    private int[] entityAmounts;

    [SerializeField]
    private int sampleCount = 100;

    [SerializeField]
    [Tooltip("In milliseconds")]
    private double cancelDuration;

    [SerializeField]
    private Sorter[] sorters;

    private ExperimentSampler experimentSampler;

    private void OnValidate() {
        entityAmounts = new int[entitySteps];
        for (int i = 0; i < entitySteps; i++) {
            entityAmounts[i] = (i + 1) * entityStepAmount;
        }
    }

    private void Start() {
        experimentSampler = new ExperimentSampler(entityAmounts, sampleCount);
        StartCoroutine(RunExperiment());
    }

    private IEnumerator RunExperiment() {
        for (int i = 0; i < sorters.Length; i++) {
            experimentSampler.StartColumn(sorters[i].name);
            Sorter sorter = sorters[i];
            for (int j = 0; j < entityAmounts.Length; j++) {
                Profiler.BeginSample("Set entity count");
                entityManager.SetEntityCount(entityAmounts[j]);
                Profiler.EndSample();
                yield return null;


                experimentSampler.StartAverage();
                while (!experimentSampler.FinishedSamples) {
                    Profiler.BeginSample("Sampling and sorting");

                    Entity[] toSort = entityManager.GetEntities();

                    experimentSampler.StartSample();
                    
                    sorter.Sort(toSort);
                    
                    experimentSampler.EndSample();
                    
                    SetColour(toSort);
                    Debug.Log(CheckSortCorrectness(toSort));
                    
                    Profiler.EndSample();
                    yield return null;
                }

                double averageTime = experimentSampler.FinishSampleAverage();

                if (cancelDuration < averageTime) {
                    break;
                }
            }

            // trims the entity list to avoid excess memory consumption
            if(i == 0) {
                entityManager.HitMaxAmount();
            }
        }
        experimentSampler.ExportSampleCSV("test.csv");
    }

    private void SetColour(Entity[] sortedEntities) {
        Debug.Log(sortedEntities.Length);
        int halfway = sortedEntities.Length / 2;
        for (int i = 0; i < sortedEntities.Length; i++) {
            if(i < halfway) {
                sortedEntities[i].sr.color = Color.black;
                sortedEntities[i].sr.sortingOrder = 1;
                continue;
            }
            sortedEntities[i].sr.color = Color.white;
            sortedEntities[i].sr.sortingOrder = 0;
        }
    }

    private bool CheckSortCorrectness(Entity[] entities) {
        for (int i = 0; i < entities.Length - 1; i++) {
            if (entities[i].SortValue > entities[i+1].SortValue) {
                return false;
            }
        }
        return true;
    }
}
