using System;
using System.Threading.Tasks;
using Web.Models.Repositories;

namespace Web.Insfrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        SiteValueRepository SiteValueRepository { get; }
        ProductRepository ProductRepository { get; }
        ProductPriceRepository ProductPriceRepository { get; }
        ProductPriceLogRepository ProductPriceLogRepository { get; }
        CRMLogsRepository CrmLogsRepository { get; }
        FactorCostSetRepository FactorCostSetRepository { get; }
        FactorRepository FactorRepository { get; }
        CustomerRepository CustomerRepository { get; }
        CustomersRelationRepository CustomerRelationRepository { get; }
        CustomerOptionValueRepository CustomerOptionValueRepository { get; }
        CustomersCategoriesBridgeRepository CustomersCategoriesBridgeRepository { get; }
        CustomersOrganizationBridgeRepository CustomersOrganizationBridgeRepository { get; }
        ProductOptionValueRepository ProductOptionValueRepository { get; }
        WorkFlowRepository WorkFlowRepository { get; }
        UserRepository UserRepository { get; }
        WorkFlowJobRepository WorkFlowJobRepository { get; }
        PermissionRepository PermissionRepository { get; }
        FactorItemRepository FactorItemRepository { get; }
        FactorItemCostRepository FactorItemCostRepository { get; }
        FactorCostRepository FactorCostRepository { get; }
        FactorOptionValueRepository FactorOptionValueRepository { get; }
        bool Save();
        Task<int> SaveAsync();
    }
}
