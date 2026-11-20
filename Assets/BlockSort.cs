using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Windows;


public class BlockSort : Sorter
{

    // Combines merge sorting with insertion sorting
    // scales memory by n, since its not In-Place and sorts to a results array, could be 1 but oh well
    public override Entity[] Sort(Entity[] input) {
        Entity[] result = new Entity[input.Length];
        int sectionSize = 16;
        int sections = Mathf.CeilToInt((float)input.Length / sectionSize);

        int[] mergeIndexes = new int[sections]; 

        // sort sections
        int start = 0;
        while(start < input.Length) {
            int end = Mathf.Clamp(start + sectionSize, 0, input.Length);
            InsertSort(input, start, end);
            start = end;
        }

        Debug.Log(input[input.Length - 1]);

        // merge sections
        for (int i = 0; i < result.Length; i++) {
            int section = 0;
            Entity entity = input[mergeIndexes[section]];

            // go through sections trying to find an entity with a lower Sort value
            for (int j = 1; j < sections; j++) {
                if (mergeIndexes[j] > sectionSize || j * sectionSize + mergeIndexes[j] > input.Length - 1) {
                    continue;
                }

                int checkEntityIndex = j * sectionSize + mergeIndexes[j];
                //Debug.Log("check");
                if (input[checkEntityIndex].SortValue < entity.SortValue) {
                    //Debug.Log($"Found better! old : {entity.SortValue} new : {input[checkEntityIndex].SortValue}, at section {j}, prev section {section}");
                    section = j;
                    entity = input[checkEntityIndex];
                }
            }
            mergeIndexes[section]++;

            result[i] = entity;

            Debug.Log($"Added: {result[i].SortValue}");
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="start">start inclusive</param>
    /// <param name="end">end exclusive</param>
    /// <returns></returns>
    private Entity[] InsertSort(Entity[] input, int start, int end) {
        for (int i = start + 1; i < end; i++) {
            for (int j = i; start < j && input[j].SortValue < input[j - 1].SortValue; j--) {
                Entity temp = input[j - 1];
                input[j - 1] = input[j];
                input[j] = temp;
            }
        }
        return input;
    }
}
