using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSpeed : CardModel
{
    [SerializeField] GameObject feedback = null;
    [SerializeField] float buffTime = 5;
    [SerializeField] ManaRequirement manaRequirement = new ManaRequirement();
    [SerializeField] DistanceRequirement distanceRequirement = new DistanceRequirement();
    [SerializeField] SpeedRequirement speedRequirement = new SpeedRequirement();
    [SerializeField] ParticleSystem speedFeedback;
    [SerializeField] AudioClip sound = null;
    bool onFeedback;

    private void Awake()
    {
        feedback.SetActive(false);
        speedFeedback.gameObject.SetActive(false);
        myRequirements.Add(manaRequirement);
        myRequirements.Add(distanceRequirement);
        myRequirements.Add(speedRequirement);
        AudioManager.instance.GetSoundPool(sound.name, AudioManager.SoundDimesion.ThreeD, sound);
    }

    public override Requirement GetRequire() => manaRequirement;

    public override void RangeFeedback()
    {
        onFeedback = true;
    }

    public override void ResetCard()
    {
        onFeedback = false;
        feedback.gameObject.SetActive(false);
    }

    protected override bool OnCanUse()
    {
        return true;
    }

    protected override void OnUseCard()
    {
        AudioManager.instance.PlaySound(sound.name);
        onFeedback = false;
        feedback.SetActive(false);
        speedFeedback.transform.position = Main.instance.GetMainCharacter.transform.position;
        speedFeedback.transform.rotation = Main.instance.GetMainCharacter.transform.rotation;
        speedFeedback.transform.parent = Main.instance.GetMainCharacter.transform;
        StartCoroutine(WaitSeconds());
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(buffTime);

        speedFeedback.transform.parent = transform;
        Main.instance.GetMainCharacter.GetComponent<Frano.CharController>().ReturnToInitValues();
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (onFeedback && distanceRequirement.CheckRequirement())
        {
            feedback.SetActive(true);
            feedback.transform.position = Main.instance.GetMainCharacter.transform.position;
            feedback.transform.forward = Main.instance.GetMainCharacter.transform.forward;
        }
        else if(feedback.activeSelf)
        {
            feedback.SetActive(false);
        }
    }
}
