using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private InventoryManager inventory;   // Player's InventoryManager
    [SerializeField] private GameObject cardPrefab;        // UpgradeCard prefab
    [SerializeField] private Transform cardParent;         // Content container
    [SerializeField] private int optionsCount = 3;

    // Cache active cards to destroy between rounds
    private readonly List<GameObject> activeCards = new();

    void Awake()
    {
        if (!inventory) inventory = FindObjectOfType<InventoryManager>();
        gameObject.SetActive(false);
    }

    // Called by GameManager when the player levels up
    public void Open()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        // Ask inventory for options
        inventory.OnOfferUpgrades += RenderOptions;
        inventory.OfferUpgrades(optionsCount);
        inventory.OnOfferUpgrades -= RenderOptions; // one-shot
    }

    private void RenderOptions(List<UpgradeOption> options)
    {
        // clear old
        foreach (var c in activeCards) Destroy(c);
        activeCards.Clear();

        foreach (var opt in options)
        {
            var go = Instantiate(cardPrefab, cardParent);
            activeCards.Add(go);

            var icon = go.transform.Find("Icon")?.GetComponent<Image>();
            var title = go.transform.Find("Title")?.GetComponent<Text>();
            var desc = go.transform.Find("Description")?.GetComponent<Text>();
            var btn = go.transform.Find("PickButton")?.GetComponent<Button>();

            if (icon)  icon.sprite = opt.Icon;
            if (title) title.text = opt.Title;
            if (desc)  desc.text = opt.Description;

            if (btn)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    opt.OnChoose?.Invoke();   // apply upgrade
                    Close();
                });
            }
        }
    }

    public void Close()
    {
        foreach (var c in activeCards) Destroy(c);
        activeCards.Clear();
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
