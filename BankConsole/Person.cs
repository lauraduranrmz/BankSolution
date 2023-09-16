namespace BankConsole;

public abstract class Person
{
    public abstract string GetName();

    public string GetCountry()
    {
        return "México";
    }

}

public interface IPerson //letra I
{
    string GetName();
    string GetCountry();
}