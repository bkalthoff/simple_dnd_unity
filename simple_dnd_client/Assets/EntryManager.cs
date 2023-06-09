using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> entries = new List<GameObject>();
    public GameObject entryPrefab;

    private Vector3 firstPosition = new Vector3(-354, 313, 0);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Sort();
        }
    }

    public void AddEntry(string id, string name = "---")
    {
        print("AddEntry called with id " + id + " and name " + name);
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("List");
        print("Found " + targetObjects.Length + " objects with the tag 'List'.");

        GameObject newObject = Instantiate(entryPrefab);
        newObject.transform.parent = targetObjects[0].transform;

        entries.Add(newObject);

        int index = entries.FindIndex(entry => entry == newObject);

        newObject.transform.localPosition = GetPosition(index);

        UnitEntry entry = newObject.GetComponent<UnitEntry>();
        entry.instanceID = id;
        entry.unitName = name;

        //sort the list of entries
        Sort();
    }

    public void RemoveEntry(string id)
    {
        print("RemoveEntry called with id " + id);
        // Find and remove the unit by ID
        int index = entries.FindIndex(entry => entry.GetComponent<UnitEntry>().instanceID == id);
        if (index >= 0)
        {
            //destroy the entry
            Destroy(entries[index]);
            entries.RemoveAt(index);
            Sort();
        }
    }

    public void ClearEntries()
    {
        print("ClearEntries called");
        // Clear the existing list
        foreach (var entry in entries)
        {
            Destroy(entry);
        }
        entries.Clear();
    }

    public void setName(string id, string name)
    {
        print("SetName called with id " + id + " and name " + name);
        // Find the unit by ID and update its name
        int index = entries.FindIndex(entry => entry.GetComponent<UnitEntry>().instanceID == id);
        if (index >= 0)
        {
            print("Found entry at index " + index);
            entries[index].GetComponent<UnitEntry>().unitName = name;
            Sort();
        }
    }

    void Sort()
    {
        //sort the list of entries by initiative
        entries.Sort((b, a) => a.GetComponent<UnitEntry>().initiative.CompareTo(b.GetComponent<UnitEntry>().initiative));
        //for every entry in the list of entries, move it down
        for (int i = 0; i < entries.Count; i++)
        {
            int initiative = entries[i].GetComponent<UnitEntry>().initiative;
            entries[i].transform.localPosition = GetPosition(i);
        }
    }

    Vector3 GetPosition(int index)
    {
        Vector3 position = firstPosition;
        position.y -= 32 * index;
        return position;
    }
}
