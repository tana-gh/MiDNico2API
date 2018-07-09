namespace MiDCookieGetter
{
    using System.Data;

    public interface ICookieGetter
    {
        ICookieGetter WithHostKey(string hostKey);
        ICookieGetter WithName   (string name);
        DataTable    Do();
    }
}
