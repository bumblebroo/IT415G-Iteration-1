using UnityEngine;

public class InsertSort : Sorter {
    public override Entity[] Sort(Entity[] input) {
        for (int i = 1; i < input.Length; i++) {
            for (int j = i; 0 < j && input[j].SortValue < input[j - 1].SortValue; j--) {
                Entity temp = input[j - 1];
                input[j - 1] = input[j];
                input[j] = temp;
            }
        }
        return input;
    }
}
