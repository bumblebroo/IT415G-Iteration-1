using System;
using UnityEngine;

public class MergeSort : Sorter
{
    public override Entity[] Sort(Entity[] input) {
        if(input.Length > 1) {
            Entity[] leftEntities = SplitLeft(input);
            Entity[] rightEntities = SplitRight(input);

            Sort(leftEntities);
            Sort(rightEntities);

            Merge(leftEntities, rightEntities, input);
        }
        return input;
    }

    private void Merge(Entity[] leftEntities, Entity[] rightEntities, Entity[] input) {
        int left = 0;
        int right = 0;
        for (int i = 0; i < input.Length; i++) {
            if (left < leftEntities.Length && right < rightEntities.Length) {
                if (leftEntities[left].SortValue <= rightEntities[right].SortValue) {
                    input[i] = leftEntities[left];
                    left++;
                    continue;
                }

                input[i] = rightEntities[right];
                right++;
                continue;
            }
            
            if (left < leftEntities.Length) {
                input[i] = leftEntities[left];
                left++;
                continue;
            }
            input[i] = rightEntities[right];
            right++;
        }
    }

    private Entity[] SplitLeft(Entity[] input) {
        int leftLength = input.Length / 2;
        Entity[] leftEntities = new Entity[leftLength];
        for (int i = 0; i < leftLength; i++) {
            leftEntities[i] = input[i];
        }
        return leftEntities;
    }

    private Entity[] SplitRight(Entity[] input) {
        int leftLength = input.Length / 2;
        int rightLength = input.Length - leftLength;
        Entity[] rightEntities = new Entity[rightLength];
        for (int i = 0; i < rightLength; i++) {
            rightEntities[i] = input[i + leftLength];
        }
        return rightEntities;
    }
}
