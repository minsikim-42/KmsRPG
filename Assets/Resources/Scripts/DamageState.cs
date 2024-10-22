public class DamageState
{
	int DMG;
	bool isCritical;

	public DamageState(int dmg, bool isCri)
	{
		DMG = dmg;
		isCritical = isCri;
	}

	public void setDMG(float per)
	{
		DMG = (int)(DMG * per);
	}

	public int getDMG()
	{
		return DMG;
	}
	public bool getIsCri()
	{
		return isCritical;
	}
}