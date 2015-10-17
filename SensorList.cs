using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SensorList : MonoBehaviour {
    //because we are only adding entities to the list in order of spawning,
    //this is kept in sync with the PlayerSensorSystem. If we do sorting of
    //some kind this will no longer be in sync.
    public LinkedList<Entity> linkedEntityList;

    public GameObject listItem;
    protected List<SensorListItem> sensorListItems;
    public int sensorListMax = 8;
    protected Entity playerTarget;
    protected Text contactText;
    protected Queue<GameObject> inactiveItems;

    public void Start() {
        inactiveItems = new Queue<GameObject>();
        sensorListItems = new List<SensorListItem>();
        contactText = transform.parent.Find("Contact Text").GetComponent<Text>();
        List<Entity> entityList = EntityManager.entities;
        linkedEntityList = new LinkedList<Entity>();

        EventManager.Instance.AddListener<Event_EntitySpawned>(OnEntitySpawned);
        EventManager.Instance.AddListener<Event_EntityDespawned>(OnEntityDespawned);
        EventManager.Instance.AddListener<Event_PlayerTargetChanged>(OnPlayerTargetChanged);

        for(var i = 0; i < sensorListMax; i++) {
            GameObject itemGameObject = Instantiate(listItem) as GameObject;
            SensorListItem item = itemGameObject.GetComponent<SensorListItem>();
            itemGameObject.transform.SetParent(transform, false);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.rotation = Quaternion.identity;
            itemGameObject.SetActive(false);
            inactiveItems.Enqueue(itemGameObject);
        }

        for (int i = 0; i < entityList.Count; i++) {
            AddItemToList(entityList[i]);
        }
    }

    protected void AddItemToList(Entity entity) {
        if (entity == PlayerManager.PlayerEntity) return;
        linkedEntityList.AddLast(entity);
        contactText.text = "Contacts (" + linkedEntityList.Count + ")";
        if(sensorListItems.Count < sensorListMax) {
            GameObject itemGameObject = inactiveItems.Dequeue();
            SensorListItem item = itemGameObject.GetComponent<SensorListItem>();
            itemGameObject.SetActive(true);
            item.SetEntity(entity);
            sensorListItems.Add(item);
        }
    }

    public void OnEntitySpawned(Event_EntitySpawned evt) {
        AddItemToList(evt.entity);
    }

    public void OnEntityDespawned(Event_EntityDespawned evt) {
        linkedEntityList.Remove(evt.entity);
        contactText.text = "Contacts (" + linkedEntityList.Count + ")";
        if(linkedEntityList.Count < sensorListItems.Count) {
            var item = sensorListItems[sensorListItems.Count - 1];
            sensorListItems.Remove(item);
            inactiveItems.Enqueue(item.gameObject);
            SensorListItem selected = GetItemForEntity(playerTarget);
            if (selected) selected.Deselect();
            var node = linkedEntityList.First;
            for(int i = 0; i < sensorListItems.Count; i++) {
                sensorListItems[i].SetEntity(node.Value, node.Value == playerTarget);
                node = node.Next;
            }
            item.gameObject.SetActive(false);
        }
    }

    public void OnPlayerTargetChanged(Event_PlayerTargetChanged evt) {
        SensorListItem selected = GetItemForEntity(playerTarget);
        if (selected) selected.Deselect();

        playerTarget = evt.newTarget;
        //if we dont have a target there is nothing else to do
        if(playerTarget == null) {
            return;
        }

        SensorListItem newTargetItem = GetItemForEntity(playerTarget);
        if(newTargetItem) {
            //already displayed, we can just select it
            newTargetItem.Select();
            return;
        }

        if(selected == null) { //target is not on the list displayed
            sensorListItems[0].SetEntity(playerTarget, true);
            var node = linkedEntityList.Find(playerTarget);
            for(var i = 1; i < sensorListItems.Count; i++) {
                node = node.NextOrFirst();
                sensorListItems[i].SetEntity(node.Value);
            }
            return;
        }

        var currentTargetNode = linkedEntityList.Find(selected.entity);
        if(playerTarget == currentTargetNode.NextOrFirst().Value) {
            ShiftDown();
        }
        else if (playerTarget == currentTargetNode.PreviousOrLast().Value) {
            ShiftUp();
        }
        else {
            sensorListItems[0].SetEntity(playerTarget, true);
            var node = linkedEntityList.Find(playerTarget);
            for (var i = 1; i < sensorListItems.Count; i++) {
                node = node.NextOrFirst();
                sensorListItems[i].SetEntity(node.Value);
            }
        }

    }

    protected void ShiftUp() {
        for (int i = sensorListItems.Count - 1; i > 0; i--) {
            sensorListItems[i].SetEntity(sensorListItems[i - 1].entity);
        }
        sensorListItems[0].SetEntity(playerTarget, true);
    }

    protected void ShiftDown() {
        for(int i = 0; i < sensorListItems.Count - 1; i++) {
            sensorListItems[i].SetEntity(sensorListItems[i + 1].entity);
        }
        sensorListItems[sensorListItems.Count - 1].SetEntity(playerTarget, true);
    }

    protected SensorListItem GetItemForEntity(Entity entity) {
        if (entity == null) return null;
        return sensorListItems.Find((item) => {
            return item.entity == entity;
        });
    }
}
