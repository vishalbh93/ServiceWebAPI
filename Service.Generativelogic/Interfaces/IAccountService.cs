using Service.DaataAccess.ServiceDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Generativelogic.Interfaces
{
    public interface IAccountService
    {
        string GetData();
        AccountDetails AuthenticateUse(string userName, string password);

        string GenerateToken(AccountDetails user);
    }
}
