using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class UpgradableCollector : MonoBehaviour
{
    [System.Serializable]
    public struct Upgrade
    {
        public int Level;
        public Vector3 ColliderSize;
        public GameObject Grab;
    }

    [SerializeField] private BoxCollider _collider;

    [SerializeField] private List<Upgrade> _upgrades;

    public void SetUpgrade(int level)
    {
        if (level > 1)
            _upgrades.Where(upgrade => upgrade.Level == level - 1).First().Grab.SetActive(false);

        Upgrade levelUpgrade = _upgrades.Where(upgrade => upgrade.Level == level).First();
        GameObject grab = levelUpgrade.Grab;

        _collider.size = levelUpgrade.ColliderSize;

        float scale = grab.transform.localScale.x;
        grab.transform.localScale = grab.transform.localScale * 0.8f;
        grab.SetActive(true);
        var anim = grab.transform.DOScale(scale * 1.2f, 0.3f).Play();
        anim.onComplete += () => grab.transform.DOScale(scale, 0.3f).Play();
    }

    private void Awake() =>
        _collider.size = _upgrades[0].ColliderSize;
}
