using UnityEngine;
using System.Collections.Generic;

public class EntityIndicatorManager : MonoBehaviour {

    protected List<EntityIndicator> indicators;
    protected Queue<EntityIndicator> indicatorPool;
    public GameObject prefab;

    public void Start() {
        indicators = new List<EntityIndicator>();
        indicatorPool = new Queue<EntityIndicator>();
        EventManager.Instance.AddListener<Event_EntityVisibilityChanged>(OnEntityVisibilityChanged);
        EventManager.Instance.AddListener<Event_EntityDespawned>(OnEntityDespawned);
        GetComponent<RectTransform>().position = Vector3.zero;
    }

    public void OnEntityVisibilityChanged(Event_EntityVisibilityChanged evt) {
        if(evt.isVisible) {
            SpawnIndicator(evt.entity);
        }
        else {
            DespawnIndicator(evt.entity);
        }
    }

    public void OnEntityDespawned(Event_EntityDespawned evt) {
        DespawnIndicator(evt.entity);
    }

    public void SpawnIndicator(Entity entity) {
        var indicator = indicators.Find((t) => {
            return t.trackedTransform == entity.transform;
        });
        if (indicator) return;
        if(indicatorPool.Count > 0) {
            indicator = indicatorPool.Dequeue();
        }
        else {
            var go = Instantiate(prefab) as GameObject;
            var rt = go.GetComponent<RectTransform>();
            indicator = go.GetComponent<EntityIndicator>();
        }
        indicator.gameObject.SetActive(true);
        indicator.entity = entity;
        indicator.SetTrackedObject(entity);
        indicator.SetColor(FactionManager.GetColor(entity.factionId));
        indicators.Add(indicator);
        indicator.transform.SetParent(transform, true);
    }

    public void DespawnIndicator(Entity entity) {
        var indicator = indicators.Find((t) => {
            return t.trackedTransform == entity.transform;
        });
        if (indicator == null) return;
        indicators.Remove(indicator);
        indicator.Untrack();
        indicator.gameObject.SetActive(false);
        indicator.entity = null;
        indicatorPool.Enqueue(indicator);
    }

}
