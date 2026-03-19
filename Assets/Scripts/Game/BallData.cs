public class BallData
{
    private WeaponData _weapon;
    private ChangeBallData _ballData;

    public WeaponData Weapon => _weapon;

    public ChangeBallData ballData => _ballData;

    public void SetWeapon(WeaponData weapon)
    {
        _weapon = weapon;
    }

    public void SetBall(ChangeBallData changeBallData)
    {
        _ballData = changeBallData;
    }
}
