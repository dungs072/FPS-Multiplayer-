using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DISystem : MonoBehaviour
{
    public static DISystem Instance { get; private set; }
    [Header("References")]
    [SerializeField] private DamageIndicator indicatorPrefab = null;
    [SerializeField] private RectTransform holder = null;
    
    private Camera mainCamera = null;
    private Transform player = null;
    private Dictionary<Transform,DamageIndicator> Indicators = new Dictionary<Transform, DamageIndicator>();
    #region Delegates
    public Action<Transform> CreateIndicator = delegate{};
    public Func<Transform,bool> CheckIfObjectInsight = null;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMainCamera(Camera camera)
    {
        this.mainCamera = camera;
    }
    public void SetPlayerTransform(Transform player)
    {
        this.player = player;
    }
    private void OnEnable() {
        CreateIndicator+=Create;
        CheckIfObjectInsight+=InSight;
    }
    private void OnDisable() {
        CreateIndicator-=Create;
        CheckIfObjectInsight-=InSight;
    }

    private void Create(Transform target)
    {
        if(Indicators.ContainsKey(target))
        {
            Indicators[target].Restart();
            return;
        }
        DamageIndicator newIndicator = Instantiate(indicatorPrefab,holder);
        newIndicator.Register(target,player,new Action(()=>{Indicators.Remove(target);}));

        Indicators.Add(target,newIndicator);
    }
    private bool InSight(Transform t)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(t.position);
        return(screenPoint.z>0&&screenPoint.x>0&&screenPoint.x<1&&
            screenPoint.y>0&&screenPoint.y<1);
    }
}
