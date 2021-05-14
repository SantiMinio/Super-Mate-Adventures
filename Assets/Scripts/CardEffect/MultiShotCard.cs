using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotCard : CardModel
{
    [SerializeField] GameObject feedback = null;
    [SerializeField] float maxRangeFeedback = 8;
    [SerializeField] float damage = 10;
    [SerializeField] float timeBetweenBullets = 1;
    [SerializeField] float bulletsAmmount = 5;

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

    public override Requirement GetRequire() => manaRequirement;

    public override void RangeFeedback()
    {
        feedback.SetActive(true);
        feedback.transform.position = Main.instance.GetMainCharacter.transform.position;
    }

    protected override void OnUseCard()
    {
        feedback.SetActive(false);

        StartCoroutine(ShotBullets());
    }

    IEnumerator ShotBullets()
    {

        for (int i = 0; i < bulletsAmmount; i++)
        {
            Vector3[] dirs = new Vector3[8]
            {
                Main.instance.GetMainCharacter.transform.forward,
            -Main.instance.GetMainCharacter.transform.forward,
            Main.instance.GetMainCharacter.transform.right,
            -Main.instance.GetMainCharacter.transform.right,
            Main.instance.GetMainCharacter.transform.forward + Main.instance.GetMainCharacter.transform.right,
            -Main.instance.GetMainCharacter.transform.forward + Main.instance.GetMainCharacter.transform.right,
            Main.instance.GetMainCharacter.transform.forward - Main.instance.GetMainCharacter.transform.right,
            -Main.instance.GetMainCharacter.transform.forward - Main.instance.GetMainCharacter.transform.right
            };
            for (int x = 0; x < dirs.Length; x++)
            {
                var newBullet = Instantiate<Bullet>(bulletPrefab);
                newBullet.transform.position = Main.instance.GetMainCharacter.transform.position;
                newBullet.Shoot(dirs[x], damage, Main.instance.GetMainCharacter.transform);
                Debug.Log(dirs.Length);
            }

            yield return new WaitForSeconds(timeBetweenBullets);
        }

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
        }
    }
}
