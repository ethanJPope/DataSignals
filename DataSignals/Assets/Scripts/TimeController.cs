using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class TimeController : MonoBehaviour
{
    [SerializeField] Text text;
    private int time = 0;
    private float gameTime = 0f;
    
    void Update() {
        gameTime += Time.deltaTime;
        time = (int)gameTime;
        text.text = time.ToString();
    }
}