namespace TemplateApp.Shared
{
    public interface IScopedContext
    {
        public string UserName { get; }
        public Guid UserId { get; }
    }
}