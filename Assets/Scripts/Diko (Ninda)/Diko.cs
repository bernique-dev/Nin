using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nin/Ninda/Diko")]
public class Diko : ScriptableObject {

    public DateTime lastModificationDate;

    public List<Devinision> devinisions;

    public void Add(Devinision devinision, bool sort = true) {
        devinisions.Add(devinision);
        if (sort) Sort();
        DeleteDuplicates();
        DeleteEmpty();
    }

    public void Sort() {
        DevinisionComparer comparer = new DevinisionComparer();
        devinisions.Sort(comparer);
        DeleteDuplicates();
        DeleteEmpty();
    }

    public void DeleteDuplicates() {
        List<Devinision> duplicates = devinisions.Where(d => devinisions.Count(dBis => string.Compare(d.nindaVersion, dBis.nindaVersion) == 0) > 1).ToList();
        foreach (Devinision duplicate in duplicates) {
            for (int i = 0; i < devinisions.Count(d => string.Compare(d.nindaVersion, duplicate.nindaVersion) == 0) - 1; i++) {
                devinisions.Remove(duplicate);
            }
        }
    }

    public void DeleteEmpty() {
        devinisions = devinisions.Where(d => d.nindaVersion.Length > 0).ToList();
    }

}
