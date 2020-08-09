using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(VerticalLayoutGroup), typeof(ContentSizeFitter))]
public class ListContentManager : MonoBehaviour
{
    [SerializeField] private GameObject _listElementPrefab;

    public void AddElements(IEnumerable<string> elements)
    {
        foreach (var element in elements)
        {
            var instance = Instantiate(_listElementPrefab, transform);
            instance.GetComponentInChildren<TMP_Text>().text = element;
        }
    }

    public void ClearElements()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
    }
}
