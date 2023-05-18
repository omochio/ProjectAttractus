public interface IWeapon
{
    public void Shot();
    public void Reload();
    public void ResetTimeCount();

    public int AmmoCount
    { get; }
}
