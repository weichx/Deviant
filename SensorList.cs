using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SensorList : MonoBehaviour {
    //because we are only adding entities to the list in order of spawning,
    //this is kept in sync with the PlayerSensorSystem. If we do sorting of
    //some kind this will no longer be in sync.

    public GameObject listItem;
    protected List<SensorListItem> sensorListItems;
    protected List<Entity> entities;
    protected int sensorListStart = 0;
    protected int sensorListEnd = 0;
    public int sensorListMax = 8;
    protected Entity playerTarget;
    protected Text contactText;

    public void Start() {
        sensorListItems = new List<SensorListItem>();
        contactText = transform.parent.Find("Contact Text").GetComponent<Text>();
        entities = new List<Entity>();
        List<Entity> entityList = EntityManager.entities;
        for (int i = 0; i < entityList.Count; i++) {
            AddItemToList(entityList[i]);
        }
        EventManager.Instance.AddListener<Event_EntitySpawned>(OnEntitySpawned);
        EventManager.Instance.AddListener<Event_EntityDespawned>(OnEntityDespawned);
        EventManager.Instance.AddListener<Event_PlayerTargetChanged>(OnPlayerTargetChanged);
    }

    protected void AddItemToList(Entity entity) {
        if (entity == PlayerManager.PlayerEntity) return;
        entities.Add(entity);
        contactText.text = "Contacts (" + entities.Count + ")";
        if(sensorListItems.Count < sensorListMax) {
            sensorListEnd++;
            GameObject itemGameObject = Instantiate(listItem) as GameObject;
            SensorListItem item = itemGameObject.GetComponent<SensorListItem>();
            item.SetEntity(entity);
            itemGameObject.transform.SetParent(transform, false);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.rotation = Quaternion.identity;
            sensorListItems.Add(item);
        }
    }

    public void OnEntitySpawned(Event_EntitySpawned evt) {
        AddItemToList(evt.entity);
    }

    //todo no longer correct
    public void OnEntityDespawned(Event_EntityDespawned evt) {
        entities.Remove(evt.entity);
        var entityItem = sensorListItems.Find((item) => {
            return item.entity == evt.entity;
        });

        if(entityItem != null) {
            ResetListItems(playerTarget);
        }

        contactText.text = "Contacts (" + entities.Count + ")";
    }

    public void OnPlayerTargetChanged(Event_PlayerTargetChanged evt) {
        playerTarget = evt.newTarget;
        for (int i = 0; i < sensorListItems.Count; i++) {
            var listItem = sensorListItems[i];
            if (listItem.entity == evt.oldTarget) {
                listItem.Deselect();
            }
        }

        //is it visible on current screen
        var selectedListItem = sensorListItems.Find((sensorItem) => {
            return sensorItem.entity == evt.newTarget;
        });          

        int lastIndex = entities.IndexOf(evt.oldTarget);
        int index = entities.IndexOf(evt.newTarget);

        if (Util.Between(sensorListStart, index, sensorListStart + sensorListItems.Count)) {
            selectedListItem.Select();
        }
        else {
            sensorListStart = index;
            ResetListItems(evt.newTarget);
        }       
    }

    protected void ResetListItems(Entity target) {
        int start = sensorListStart;
        for(int i = 0; i < sensorListItems.Count; i++) {
            if (start >= entities.Count) {
                start = 0;
            }
            if (entities.Count >= i) break;
            sensorListItems[i].SetEntity(entities[start], entities[start] == target);
            start++;
        }
    }
}
