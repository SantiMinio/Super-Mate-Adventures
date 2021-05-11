using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkillshotCard : CardModel
{
    [SerializeField] GameObject feedback = null;
    [SerializeField] float maxRangeFeedback = 8;
    [SerializeField] float damage = 10;

    [SerializeField] Bullet bulletPrefab = null;
    [SerializeField] ManaRequirement manaRequirement = new ManaRequirement();

    private void Awake()
    {
        feedback.SetActive(false);
        myRequirements.Add(manaRequirement);
    }

    protected override bool OnCanUse()
    {
        return true;
    }

    public override void RangeFeedback()
    {
        feedback.SetActive(true);
        feedback.transform.position = Main.instance.GetMainCharacter.transform.position;
    }

    protected override void OnUseCard()
    {
        feedback.SetActive(false);

        var newBullet = Instantiate<Bullet>(bulletPrefab);
        newBullet.transform.position = feedback.transform.position;
        newBullet.Shoot(feedback.transform.forward, damage, Main.instance.GetMainCharacter.transform);

        Destroy(this.gameObject);
    }

    public override void ResetCard()
    {
        feedback.SetActive(false);
    }

    private void Update()
    {
        if (feedback.activeSelf)
        {
            feedback.transform.position = Main.instance.GetMainCharacter.transform.position;
            Vector3 skillShotDir = transform.position - feedback.transform.position;
            skillShotDir.y = 0;
            if (Vector3.zero == skillShotDir) skillShotDir = feedback.transform.forward;
            feedback.transform.forward = skillShotDir.normalized;
            feedback.transform.localScale = new Vector3(feedback.transform.localScale.x, feedback.transform.localScale.y, Mathf.Clamp(skillShotDir.magnitude, 1, maxRangeFeedback));
        }
    }
}
