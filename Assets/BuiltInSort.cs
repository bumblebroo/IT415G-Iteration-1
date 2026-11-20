using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuiltInSort : Sorter {
    public override Entity[] Sort(Entity[] input) {
        List<Entity> result = new List<Entity>(input);
        result.Sort();
        return result.ToArray();
    }
}
