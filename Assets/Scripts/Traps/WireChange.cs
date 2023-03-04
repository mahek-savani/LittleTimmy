using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireChange : MonoBehaviour
{
    [Header("Button Attachment - Only Use 1")]
    public SpikeTrap AttachedSpikeTrap;
    public PitTrap AttachedPitTrap;

    [SerializeField]private Color startingMaterialColor;

    // Start is called before the first frame update
    void Start()
    {
        if(AttachedPitTrap) startingMaterialColor = AttachedPitTrap.transform.GetChild(0).GetComponent<Renderer>().material.color;
        else if (AttachedSpikeTrap) startingMaterialColor = AttachedSpikeTrap.transform.GetChild(0).GetComponent<Renderer>().material.color;

        if(gameObject.CompareTag("Pit"))
        {
            PitTrapButton.PitTrapButtonPushed += ChangeActiveColor;
            PitTrapResetButton.PitTrapResetButtonPushed += ChangeActiveColor;
        }

        if(gameObject.CompareTag("Spike"))
        {
            buttonPress.SpikeTrapButtonPushed += ChangeActiveColor;
            buttonPressReset.SpikeTrapResetPushed += ChangeActiveColor;
        }

        if(gameObject.CompareTag("ExperimentalPit"))
        {
            Experimental_PitTrapButton.ExperimentalPitButtonPushed += ChangeActiveColor;
            Experimental_Reset_Button.ExperimentalResetPushed += ChangeActiveColor;
        }

        if(gameObject.layer == 10) GetComponent<Renderer>().material.color = Color.grey;
        else GetComponent<Renderer>().material.color = startingMaterialColor;
    }

    void ChangeToActiveColor()
    {
        if(gameObject.layer == 10) 
        {
            if(gameObject.CompareTag("Pit") && AttachedPitTrap.trapActive)
                GetComponent<Renderer>().material.color = startingMaterialColor;
        }
    }

    void ChangeActiveColor()
    {
        // Logic for the Reset Wires
        // Reset Wires should be GREY if the trap is ACTIVE
        // Should be COLORED if the trap is NOT ACTIVE
        if(gameObject.layer == 10)
        {
            // Is this a pit trap that is attached?
            if(AttachedPitTrap){
                if(AttachedPitTrap.trapActive){
                    GetComponent<Renderer>().material.color = Color.grey;
                } else {
                    GetComponent<Renderer>().material.color = startingMaterialColor;
                }
            
            // If not, is this an Spike Trap that is attached?
            } else if(AttachedSpikeTrap){
                if(AttachedSpikeTrap.trapActive){
                    GetComponent<Renderer>().material.color = Color.grey;
                } else {
                    GetComponent<Renderer>().material.color = startingMaterialColor;
                }
            }

        // Logic for the Activation Wires
        // Activation Wires should be COLORED if the trap is ACTIVE
        // Should be GREY if the trap is NOT ACTIVE
        } else if(gameObject.layer == 11){
            // Is this a pit trap that is attached?
            if(AttachedPitTrap){
                if(!AttachedPitTrap.trapActive){
                    GetComponent<Renderer>().material.color = Color.grey;
                } else {
                    GetComponent<Renderer>().material.color = startingMaterialColor;
                }
            
            // If not, is this an Spike Trap that is attached?
            } else if(AttachedSpikeTrap){
                if(!AttachedSpikeTrap.trapActive){
                    GetComponent<Renderer>().material.color = Color.grey;
                } else {
                    GetComponent<Renderer>().material.color = startingMaterialColor;
                }
            }
        }
    }
}
