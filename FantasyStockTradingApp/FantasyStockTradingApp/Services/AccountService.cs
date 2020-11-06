using System;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyStockTradingApp.Services
{
    public interface IAccountService
    {

    }

    public class AccountService : IAccountService
    {
        private readonly ISession _sesison;

        public AccountService(ISession session)
        {
            _sesison = session;
        }

    }
}
