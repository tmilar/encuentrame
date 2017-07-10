namespace Encuentrame.Support
{
    public static class DeleteableExtensions
    {
        public static bool IsDeleted(this IDeleteable deleteable)
        {
            return deleteable.DeletedKey.HasValue;
        }

        public static void Delete(this IDeleteable deleteable) 
        {
            if(deleteable is IDeleteAware)
                ((IDeleteAware)deleteable).OnPreDelete();
            
            deleteable.DeletedKey = SystemDateTime.Now;
        }

     

        public static void UnDelete(this IDeleteable deleteable)
        {
            deleteable.DeletedKey = null;
        }

       
    }
}