using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PoolControler : MonoBehaviour
{
    [Header("---- POOL CONTROLER TO INIT POOL ----")]
    [Header("Put object pool to list Pool or Resources/Pool")]
    [Header("Preload: Init Poll")]
    [Header("Spawn: Take object from pool")]
    [Header("Despawn: return object to pool")]
    [Header("Collect: return objects type to pool")]
    [Header("CollectAll: return all objects to pool")]
    [Space]
    [Header("Pool")]
    public List<PoolAmount> Pool;

    [Header("Particle")] public ParticleAmount[] Particle;


    public void Awake()
    {
        for (var i = 0; i < Pool.Count; i++)
            SimplePool.Preload(Pool[i].prefab, Pool[i].amount, Pool[i].root, Pool[i].collect);

        for (var i = 0; i < Particle.Length; i++)
        {
            ParticlePool.Preload(Particle[i].prefab, Particle[i].amount, Particle[i].root);
            ParticlePool.Shortcut(Particle[i].particleType, Particle[i].prefab);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PoolControler))]
public class PoolControlerEditor : Editor
{
    private PoolControler pool;

    private void OnEnable()
    {
        pool = (PoolControler)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Quick Root"))
        {
            for (var i = 0; i < pool.Pool.Count; i++)
                if (pool.Pool[i].root == null)
                {
                    var tf = new GameObject(pool.Pool[i].prefab.poolType.ToString()).transform;
                    tf.parent = pool.transform;
                    pool.Pool[i].root = tf;
                }

            for (var i = 0; i < pool.Particle.Length; i++)
                if (pool.Particle[i].root == null)
                {
                    var tf = new GameObject(pool.Particle[i].particleType.ToString()).transform;
                    tf.parent = pool.transform;
                    pool.Particle[i].root = tf;
                }
        }

        if (GUILayout.Button("Get Prefab Resource"))
        {
            var resources = Resources.LoadAll<GameUnit>("Pool");

            for (var i = 0; i < resources.Length; i++)
            {
                var isDuplicate = false;
                for (var j = 0; j < pool.Pool.Count; j++)
                    if (resources[i].poolType == pool.Pool[j].prefab.poolType)
                    {
                        isDuplicate = true;
                        break;
                    }

                if (!isDuplicate)
                {
                    var root = new GameObject(resources[i].name).transform;

                    var newPool = new PoolAmount(root, resources[i], SimplePool.DEFAULT_POOL_SIZE, true);

                    pool.Pool.Add(newPool);
                }
            }
        }
    }
}

#endif

[Serializable]
public class PoolAmount
{
    [Header("-- Pool Amount --")] public Transform root;

    public GameUnit prefab;
    public int amount;
    public bool collect;

    public PoolAmount(Transform root, GameUnit prefab, int amount, bool collect)
    {
        this.root = root;
        this.prefab = prefab;
        this.amount = amount;
        this.collect = collect;
    }
}


[Serializable]
public class ParticleAmount
{
    public Transform root;
    public ParticleType particleType;
    public ParticleSystem prefab;
    public int amount;
}


public enum ParticleType
{
    HitTank = 0,
    HitRocket = 1,
    HitGrenade = 2,
    MuzzleTank = 3,
    MuzzleMissile = 4,
    RocketTrail = 5,
    ExplosionAbility = 6,
    BuyAbility = 7,
    SmokeTrailBattle = 8,
    FlameBattle = 9,
}

public enum PoolType
{
    None = 0,
    HeathBar = 1,
    Enemy = 2,
    BulletTank = 3,
    BulletArmored = 4,
    BulletMissile = 5,
    Grenade = 6,
    BulletTakeDame = 7,
    MetalTag = 8,
    BulletProjectile = 9,
}