namespace Shared.Bank;

public class DAO<T> where T : class
{
    private readonly MDContext context;
    public DAO(MDContext context)
    {
        this.context = context;
    }

    public IEnumerable<T> List()
    {
        return context.Set<T>().ToList();
    }

    public void Add(T target)
    {
        context.Set<T>().Add(target);
        context.SaveChanges();
    }

    public void Update(T target)
    {
        context.Set<T>().Update(target);
        context.SaveChanges();
    }

    public void Remove(T target)
    {
        context.Set<T>().Remove(target);
        context.SaveChanges();
    }
    public T? RecoverBy(Func<T, bool> condition)
    {
        return context.Set<T>().FirstOrDefault(condition);
    }
}
