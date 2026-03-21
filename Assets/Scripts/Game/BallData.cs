using UnityEditor.ShaderGraph.Drawing.Colors;

public class BallData
{
    private WeaponData _weapon;
    private ChangeBallData _changeBallData;
    private ColorData _colorData;

    public WeaponData Weapon => _weapon;
    public ChangeBallData ChangeBallData => _changeBallData;
    public ColorData ColorData => _colorData;

    public void SetWeapon(WeaponData weapon)
    {
        _weapon = weapon;
    }

    public void SetBall(ChangeBallData changeBallData)
    {
        _changeBallData = changeBallData;
    }

    public void SetColor(ColorData colorData)
    {
        _colorData = colorData;
    }
}
