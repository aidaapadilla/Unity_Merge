using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask grassLayer;
    [SerializeField] LayerMask fovLayer;
    [SerializeField] LayerMask portalLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask gymPortalLayer;

    public static GameLayers i { get; set; }
    
    private void Awake()
    {
        i = this;
    
    }

    public LayerMask InteractableLayer{
        get => interactableLayer;
    }
    public LayerMask GrassLayer{
        get => grassLayer;
    }
    public LayerMask FovLayer{
        get => fovLayer;
    }
    public LayerMask PortalLayer{
        get => portalLayer;
    }
    public LayerMask GymPortalLayer{
        get => gymPortalLayer;
    }
    public LayerMask TriggerableLayers {
        get => grassLayer | fovLayer | portalLayer;
    }
}
