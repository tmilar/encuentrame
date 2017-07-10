using System;

namespace Encuentrame.Support
{
    public interface IDeleteable : IIdentifiable
    {
        DateTime? DeletedKey { get; set; }
    }
}