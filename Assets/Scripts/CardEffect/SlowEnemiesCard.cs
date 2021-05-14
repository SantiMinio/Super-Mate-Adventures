using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEnemiesCard : CardModel
{
    [SerializeField] GameObject feedback = null;
    [SerializeField] float range = 16;
    [SerializeField] ManaRequirement manaRequirement = new ManaRequirement();
    [SerializeField] float speedDown = 12;
    [SerializeField] float buffTime = 5;
    List<EnemyDummy> dummies = new List<EnemyDummy>();

    private void Awake()
    {
        feedback.SetActive(false);
        myRequirements.Add(manaRequirement);
    }

    protected override bool OnCanUse()
    {
        return true;
    }

    public override Requirement GetRequire() => manaRequirement;

    public override void RangeFeedback()
    {
        feedback.SetActive(true);
        feedback.transform.position = transform.position;
        feedback.transform.localScale = Vector3.one * range;
    }

    protected override void OnUseCard()
    {
        feedback.SetActive(false);
        var overlap = Physics.OverlapSphere(transform.position, range);

        foreach (var item in overlap)
        {
            EnemyDummy hiteable = item.GetComponent<EnemyDummy>();
            if (hiteable != null)
            {
                hiteable.GetMovementHandler.SpeedDown(speedDown);
                dummies.Add(hiteable);
            }
        }

        StartCoroutine(WaitSeconds());
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(buffTime);

        for (int i = 0; i < dummies.Count; i++)
        {
            if(dummies[i] != null)
            {
                dummies[i].GetMovementHandler.ResetSpeed();
            }
        }
        Destroy(this.gameObject);
    }

    public override void ResetCard()
    {
        feedback.SetActive(false);
    }
}
