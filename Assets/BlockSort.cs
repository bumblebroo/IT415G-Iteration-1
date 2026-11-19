using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Windows;


public class BlockSort : Sorter
{
    public override Entity[] Sort(Entity[] input) {
        Entity[] result = new Entity[input.Length];
        int sectionSize = 16;
        int sections = input.Length / sectionSize;

        int[] mergeIndexes = new int[sections]; 

        // sort sections
        int start = 0;
        while(start < input.Length) {
            int end = start + sectionSize;
            InsertSort(input, start, end);
            start = end;
        }

        // merge sections
        for (int i = 0; i < result.Length; i++) {
            int section = 0;
            Entity entity = input[mergeIndexes[section]];
            // go through sections trying to find an entity with a lower Sort value
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
        for (int i = start; i < end; i++) {
            if(i > input.Length) {
                break;
            }

            for (int j = i; 0 < j && input[j].SortValue < input[j - 1].SortValue; j--) {
                Entity temp = input[j - 1];
                input[j - 1] = input[j];
                input[j] = temp;
            }
        }
        return input;
    }
}
