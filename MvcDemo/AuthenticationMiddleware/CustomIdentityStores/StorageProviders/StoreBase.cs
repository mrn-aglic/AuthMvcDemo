using System;
using System.Threading;
using Microsoft.AspNetCore.Identity;

namespace MvcDemo.AuthenticationMiddleware.CustomIdentityStores.StorageProviders
{
    public class StoreBase
    {
        internal IdentityResult GetIdentityResult(int changes, string msg)
        {
            return changes > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError
                {
                    Description = msg
                });
        }

        internal void ThrowCheck(object obj, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (obj == null) throw new ArgumentNullException(nameof(obj));
        }
    }
}