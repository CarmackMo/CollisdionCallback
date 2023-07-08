using UnityEngine;

public class LogicExecuter : MonoBehaviour
{
    public CollisionCallback callback;
    private bool isCollided = false;

    private void Start()
    {
        callback.AddCallback(CallbackToOther, false, typeof(CollisionObject));
        callback.AddCallback(CallbackToSelf, true, typeof(CollisionObject));
    }

    void CallbackToOther(GameObject other)
    {
        isCollided = true;
        Destroy(other);
    }

    void CallbackToSelf(GameObject self)
    {
        Destroy(self);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 10, 300, 20), "Is LogicExecuter detect collision callback: " + isCollided);
    }
}
