using UnityEngine;

public class PlayerInputHandler : MonoSingleton<PlayerInputHandler>
{
    public bool GunAction { get; private set; }
    public bool PrimaryAction { get; private set; }
    public float Direction { get; private set; }
    public bool IsReversed { get; set; }

    private void Update()
    {
        GunAction = Input.GetButton("Fire2");
        PrimaryAction = Input.GetButton("Fire1");

        var horizontal = Input.GetAxisRaw("Horizontal");
        var mouseX = Input.GetAxisRaw("Mouse X");
        var inputLeft = horizontal < 0 || mouseX < 0;
        var inputRight = horizontal > 0 || mouseX > 0;

        if (inputLeft && !inputRight) Direction = Mathf.Min(horizontal, mouseX);
        else if (inputRight && !inputLeft) Direction = Mathf.Max(horizontal, mouseX);
        else Direction = 0f;

        if (IsReversed) Direction = -Direction;
    }
}