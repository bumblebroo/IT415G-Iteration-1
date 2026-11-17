using UnityEngine;

public class BubbleSort : Sorter
{
    public override Entity[] Sort(Entity[] input) {
        Entity temp = null;
        int n = input.Length;
        bool swapped = false;
        for (int i = 0; i < n - 1; i++) {
            swapped = false;
            for (int j = 0; j < n - i - 1; j++) {
                if (input[j].SortValue > input[j + 1].SortValue) {
                    temp = input[j];
                    input[j] = input[j + 1];
                    input[j + 1] = temp;
                    swapped = true;
                }
            }

            if (!swapped) {
                // no items swapped = sorted
                break;
            }
        }

        return input;
    }
}
