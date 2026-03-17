public class BallData
{
    private WeaponData _weapon;
    //private BallData _ballData;

    public WeaponData Weapon => _weapon;

    //public BallData ballData => _ballData;

    public void SetWeapon(WeaponData weapon)
    {
        _weapon = weapon;
    }
}
