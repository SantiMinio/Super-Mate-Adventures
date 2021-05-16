using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombitaPolvorita : EnemyDummy
{
    float _countExplode;
    [SerializeField] private float timeToExplode;

    [SerializeField] private float explotionRadius;
    [SerializeField] private LayerMask affectedLayers;

    private bool ticking;
    public bool CanExplode { get; private set; }
    public void Explode()
    {
        var targets = Physics.OverlapSphere(transform.position, explotionRadius, affectedLayers);

        var iHitiables = targets.Where(t => t != null && GetHiteableFromTransform(t.transform) is IHiteable).Select(x => GetHiteableFromTransform(x.transform));
        
        foreach (var hit in iHitiables)
        {
            hit.Hit(this);
        }

        Dead();
        
    }
    
    public void WaitToBomb()
    {
        if (!ticking)
        {
            ticking = true;
            GetAnimator.Play("PrepareToBoom");
        }
        
        _countExplode += Time.deltaTime;

        if (_countExplode >= timeToExplode)
        {
            CanExplode = true;
            GetAnimator.Play("Explode");
        }
        
    }
    
    #region AuxMethods

    IHiteable GetHiteableFromTransform(Transform transform)
    {
        foreach (var component in transform.GetComponents<MonoBehaviour>())
        {
            if (component is IHiteable)
            {
                return component as IHiteable;
            }
        }
        return null;
    }

    #endregion
}
