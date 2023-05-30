using UnityEngine;

public class IdleTimer : MonoBehaviour
{
    private float _idleTimer;

    [SerializeField]
    private float _idleTime = 300; // 300sec = 5min

    [SerializeField]
    private SceneLoader _sceneLoader;


    void Start()
    {
        _idleTimer = 0.0f;
    }


    void Update()
    {
        if (Input.anyKeyDown)
        {
            // reset counter  
            _idleTimer = 0.0f;
        }
        else
        {
            // increment counter
            _idleTimer += Time.deltaTime;
            Debug.Log(_idleTimer);
        }

        if (_idleTimer > _idleTime)
        {
            Debug.Log(_idleTime + " has passed!");
            _sceneLoader.LoadScene(0);
        }
    }
}