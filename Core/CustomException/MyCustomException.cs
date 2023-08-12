namespace Domain.CustomException;

public class MyCustomException : Exception
{
    public MyCustomException(string name)
        : base(name)
    {

    }
}
