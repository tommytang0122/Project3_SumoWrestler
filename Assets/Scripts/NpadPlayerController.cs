using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nn.hid;

public class NpadPlayerController : MonoBehaviour
{

    private UnityEngine.UI.Text textComponent;
    private System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

    //Serialize
    [SerializeField]
    GameObject player1;
    [SerializeField]
    GameObject player2;
    [SerializeField]
    GameObject player3;
    [SerializeField]
    GameObject player4;


    // 4 players
    private NpadId[] npadIds = { NpadId.Handheld, NpadId.No1, NpadId.No2, NpadId.No3, NpadId.No4 };
    private NpadState npadState = new NpadState();

    private GameObject[] playerArray;
    Rigidbody[] playerRigidbodyArray;

    private long[] preButtons;
    private ControllerSupportArg controllerSupportArg = new ControllerSupportArg();
    private nn.Result result = new nn.Result();

    // Start is called before the first frame update
    void Start()
    {
        Npad.Initialize();
        Npad.SetSupportedIdType(npadIds);
        NpadJoy.SetHoldType(NpadJoyHoldType.Horizontal);

        // Horizontal single Joy-Con
        Npad.SetSupportedStyleSet(NpadStyle.FullKey | NpadStyle.Handheld | NpadStyle.JoyDual |
            NpadStyle.JoyLeft | NpadStyle.JoyRight);

        for (int i = 1; i < npadIds.Length; i++)
        {
            NpadJoy.SetAssignmentModeSingle(npadIds[i]);
        }

        playerArray = new GameObject[5];
        playerArray[0] = player1;
        playerArray[1] = player1;
        playerArray[2] = player2;
        playerArray[3] = player3;
        playerArray[4] = player4;

        playerRigidbodyArray = new Rigidbody[5];
        playerRigidbodyArray[0] = player1.GetComponent<Rigidbody>();
        playerRigidbodyArray[1] = player1.GetComponent<Rigidbody>();
        playerRigidbodyArray[2] = player2.GetComponent<Rigidbody>();
        playerRigidbodyArray[3] = player3.GetComponent<Rigidbody>();
        playerRigidbodyArray[4] = player4.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        NpadButton onButtons = 0;

        for (int i = 0; i < 5; i++)
        {
            NpadId npadId = npadIds[i];
            NpadStyle npadStyle = Npad.GetStyleSet(npadId);
            if (npadStyle == NpadStyle.None) { continue; }

            Npad.GetState(ref npadState, npadId, npadStyle);

            //bool isPushedEnterButton = false;
            if (npadStyle == NpadStyle.JoyLeft)
            {
                //isPushedEnterButton = (npadState.buttons & NpadButton.Down) != 0;
                if (npadState.GetButton(NpadButton.StickLRight))
                {
                    playerArray[i].transform.forward = new Vector3(-0.1f, 0.0f, 0.0f);
                    playerArray[i].transform.position -= new Vector3(0.1f, 0.0f, 0.0f);
                }
                if (npadState.GetButton(NpadButton.StickLUp))
                {
                    playerArray[i].transform.forward = new Vector3(0.0f, 0.0f, -0.1f);
                    playerArray[i].transform.position -= new Vector3(0.0f, 0.0f, 0.1f);
                }
                if (npadState.GetButton(NpadButton.StickLLeft))
                {
                    playerArray[i].transform.forward = new Vector3(0.1f, 0.0f, 0.0f);
                    playerArray[i].transform.position += new Vector3(0.1f, 0.0f, 0.0f);
                }

                if (npadState.GetButton(NpadButton.StickLDown))
                {
                    playerArray[i].transform.forward = new Vector3(0.0f, 0.0f, 0.1f);
                    playerArray[i].transform.position += new Vector3(0.0f, 0.0f, 0.1f);
                }
                //player rush/dodge
                if (npadState.GetButtonDown(NpadButton.Right))
                {
                    playerRigidbodyArray[i].AddForce(playerArray[i].transform.forward * 1.5f, ForceMode.Impulse);
                }
                //player jump attack
                if (npadState.GetButtonDown(NpadButton.Left) && playerArray[i].GetComponent<JumpAttack>().isJumping == false)
                {
                    playerRigidbodyArray[i].AddForce(playerArray[i].transform.up * 10f, ForceMode.Impulse);
                    playerArray[i].GetComponent<JumpAttack>().isJumping = true;
                }
            }
            else if(npadStyle == NpadStyle.JoyRight)
            {
                if (npadState.GetButton(NpadButton.StickRLeft))
                {
                    playerArray[i].transform.forward = new Vector3(-0.1f, 0.0f, 0.0f);
                    playerArray[i].transform.position -= new Vector3(0.1f, 0.0f, 0.0f);
                }
                if (npadState.GetButton(NpadButton.StickRDown))
                {
                    playerArray[i].transform.forward = new Vector3(0.0f, 0.0f, -0.1f);
                    playerArray[i].transform.position -= new Vector3(0.0f, 0.0f, 0.1f);
                }
                if (npadState.GetButton(NpadButton.StickRRight))
                {
                    playerArray[i].transform.forward = new Vector3(0.1f, 0.0f, 0.0f);
                    playerArray[i].transform.position += new Vector3(0.1f, 0.0f, 0.0f);
                }

                if (npadState.GetButton(NpadButton.StickRUp))
                {
                    playerArray[i].transform.forward = new Vector3(0.0f, 0.0f, 0.1f);
                    playerArray[i].transform.position += new Vector3(0.0f, 0.0f, 0.1f);
                }

                //player rush/dodge
                if (npadState.GetButtonDown(NpadButton.A))
                {
                    playerRigidbodyArray[i].AddForce(playerArray[i].transform.forward * 1.5f, ForceMode.Impulse);
                }
                //player jump attack
                if (npadState.GetButtonDown(NpadButton.Y) && playerArray[i].GetComponent<JumpAttack>().isJumping == false)
                {
                    playerRigidbodyArray[i].AddForce(playerArray[i].transform.up * 10f, ForceMode.Impulse);
                    playerArray[i].GetComponent<JumpAttack>().isJumping = true;
                }
                
            }
        }
    }



    void ShowControllerSupport()
    {
        controllerSupportArg.SetDefault();
        controllerSupportArg.playerCountMax = (byte)(npadIds.Length - 1);

        controllerSupportArg.enableIdentificationColor = true;
        nn.util.Color4u8 color = new nn.util.Color4u8();
        color.Set(255, 128, 128, 255);
        controllerSupportArg.identificationColor[0] = color;
        color.Set(128, 128, 255, 255);
        controllerSupportArg.identificationColor[1] = color;
        color.Set(128, 255, 128, 255);
        controllerSupportArg.identificationColor[2] = color;
        color.Set(224, 224, 128, 255);
        controllerSupportArg.identificationColor[3] = color;

        controllerSupportArg.enableExplainText = true;
        ControllerSupport.SetExplainText(ref controllerSupportArg, "Red", NpadId.No1);
        ControllerSupport.SetExplainText(ref controllerSupportArg, "Blue", NpadId.No2);
        ControllerSupport.SetExplainText(ref controllerSupportArg, "Green", NpadId.No3);
        ControllerSupport.SetExplainText(ref controllerSupportArg, "Yellow", NpadId.No4);

        Debug.Log(controllerSupportArg);
        result = ControllerSupport.Show(controllerSupportArg);
        if (!result.IsSuccess()) { Debug.Log(result); }
    }

}
