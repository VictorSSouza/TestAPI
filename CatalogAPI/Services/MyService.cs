namespace CatalogAPI.Services
{
    public class MyService : IMyService
    {
        public string Greeting(string name)
        {
            return $"Seja bem-vindo, {name} \n\n{DateTime.Now}";
        }
    }
}
