using UnityEngine;
public class GlobalManagers : MonoBehaviour
{
    public static GlobalManagers Instance { get; private set; }
    
    
    [SerializeField] private GameObject parentObj; //se agrega manualmente en la UI]
    [field: SerializeField] public NetworkRunnerController networkRunnerController { get; private set; }//se agrega manualmente en la UI]
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            //destroy
            Destroy(parentObj);
        }
    }
}
