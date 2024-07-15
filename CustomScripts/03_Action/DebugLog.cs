using UnityEngine;

public class DebugLog: MonoBehaviour
{
    [SerializeField] OnCollisionEnterEvent onCollisionEnter;

    private void Start()
    {
        // action に View 関数を渡す（引数が一致している必要がある）
        onCollisionEnter.Action += View;
    }
    
    // private void View(Collision collision)
    // {
    //     Debug.Log(collision.gameObject.name);
    // }
    

    private void Awake()
    {
        // Event に View 関数を渡す（引数が一致している必要がある）
        onCollisionEnter.Event.AddListener(View);
    }
    private void View(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}