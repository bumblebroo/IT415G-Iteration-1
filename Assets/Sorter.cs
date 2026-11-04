using UnityEngine;

public abstract class Sorter : MonoBehaviour
{
    public abstract ISortable[] Sort(ISortable[] input);
}
