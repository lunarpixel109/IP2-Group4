using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{

    [SerializeField] private Sprite[] button_images;
    private Image button_sprite;
    private bool button_slected = false;
    private Gamepad Gamepad;
    private Keyboard Keyboard;

    public void OnPointerEnter(PointerEventData eventData)
    {
        button_sprite.sprite = button_images[1];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button_sprite.sprite = button_images[0];
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(eventData.selectedObject.name);

        if (eventData.selectedObject.name == "BackButton")
        {
            button_sprite.sprite = button_images[1];
            button_slected = true;
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button_sprite = GetComponent<Image>();
        Gamepad = Gamepad.current;
        Keyboard = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (button_slected)
        {
            if (Gamepad.dpad.up.isPressed || Gamepad.leftStick.up.isPressed || Keyboard.upArrowKey.isPressed || Keyboard.wKey.isPressed)
            {
                button_slected = !button_slected;   
            }
        } else
        {
            button_sprite.sprite = button_images[0];
        }
    }

    public void OnPress(bool Clicked)
    {
        if (Clicked)
        {
            SceneManager.UnloadSceneAsync("settings menu");
        }
    }

    
}
