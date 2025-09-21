using Microsoft.EntityFrameworkCore;
using Service.DaataAccess.ServiceDomain.Models;

namespace Service.DaataAccess.ServiceDomain.Interface
{
    public interface IODCContextInterface
    {
        DbSet<AccountDetails> AccountDetails { get; set; }
    }
}
