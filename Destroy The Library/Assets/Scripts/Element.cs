using UnityEngine;

public class Element : MonoBehaviour
{
    [Tooltip("Identify the type of the object")]
    [SerializeField] private Type type;
    
    public enum Type
    {
        Match = 0,
        Book = 1,
        Door = 2
    }

    /// <summary>
    /// Checks if the object type matches.
    /// </summary>
    /// <param name="type">The type to be matched</param>
    /// <returns>True if the object type matches; False, otherwise</returns>
    public bool IsElementType(Element.Type type)
    {
        return this.type == type;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GamePlay.AddObject(type);
        GamePlay.PlayAudio(type);
        Destroy(this.gameObject);
    }
}
