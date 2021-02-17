using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName="Moonstone/New Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
    [SerializeField] private DialogueNode[] dialogueNodes;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
