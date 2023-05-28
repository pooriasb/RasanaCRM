using System;
using System.Threading.Tasks;
using Web.Models.Entity;
using Web.Models.Repositories;

namespace Web.Insfrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private CRM_DbContext db;
        //private ApplicationUserManager _userManager;
        public UnitOfWork() : this(new CRM_DbContext())
        {

        }




        public UnitOfWork(CRM_DbContext db)
        {
            this.db = db ?? new CRM_DbContext();
        }

        private SiteValueRepository _siteValueRepository;
        private ProductRepository _productRepository;
        private ProductPriceRepository _productPriceRepository;
        private ProductPriceLogRepository _productPriceLogRepository;
        private CRMLogsRepository _crmLogsRepository;
        private FactorCostSetRepository _factorCostSetRepository;
        private FactorRepository _factorRepository;
        private CustomerRepository _customerRepository;
        private CustomersRelationRepository _customerRelationRepository;
        private ProductOptionValueRepository _productOptionValueRepository;
        private WorkFlowRepository _workFlowRepository;
        private UserRepository _userRepository;
        private WorkFlowJobRepository _workFlowJobRepository;
        private PermissionRepository _permissionRepository;
        private FactorItemRepository _factorItemRepository;
        private FactorItemCostRepository _factorItemCostRepository;
        private FactorCostRepository _factorCostRepository;
        private CustomerOptionValueRepository _customerOptionValueRepository;
        private CustomersCategoriesBridgeRepository _customersCategoriesBridgeRepository;
        private CustomersOrganizationBridgeRepository _customersOrganizationBridgeRepository;
        private FactorOptionValueRepository _factorOptionvalueRepository;
        public FactorCostRepository FactorCostRepository
        {
            get
            {
                if(this._factorCostRepository==null)
                {
                    this._factorCostRepository = new FactorCostRepository(db);
                }
                return _factorCostRepository;
            }
        } 
        public FactorItemRepository FactorItemRepository
        {
            get
            {
                if (this._factorItemRepository == null)
                {
                    this._factorItemRepository = new FactorItemRepository(db);
                }
                return _factorItemRepository;
            }
        }

        public FactorItemCostRepository FactorItemCostRepository
        {
            get
            {
                if (this._factorItemCostRepository == null)
                {
                    this._factorItemCostRepository = new FactorItemCostRepository(db);
                }
                return _factorItemCostRepository;
            }
        }
            
        public SiteValueRepository SiteValueRepository
        {
            get
            {
                if (this._siteValueRepository == null)
                {
                    this._siteValueRepository = new SiteValueRepository(db);
                }
                return _siteValueRepository;
            }
        }
        public ProductRepository ProductRepository
        {
            get
            {
                if (this._productRepository == null)
                {
                    this._productRepository = new ProductRepository(db);
                }
                return _productRepository;
            }
        }

        public ProductPriceRepository ProductPriceRepository
        {
            get
            {
                if (this._productPriceRepository == null)
                {
                    this._productPriceRepository = new ProductPriceRepository(db);
                }
                return _productPriceRepository;
            }
        }
        public ProductPriceLogRepository ProductPriceLogRepository
        {
            get
            {
                if (this._productPriceLogRepository == null)
                {
                    this._productPriceLogRepository = new ProductPriceLogRepository(db);
                }
                return _productPriceLogRepository;
            }
        }
        public CRMLogsRepository CrmLogsRepository
        {
            get
            {
                if (this._crmLogsRepository == null)
                {
                    this._crmLogsRepository = new CRMLogsRepository(db);
                }
                return _crmLogsRepository;
            }
        }
        public FactorCostSetRepository FactorCostSetRepository
        {
            get
            {
                if (this._factorCostSetRepository == null)
                {
                    this._factorCostSetRepository = new FactorCostSetRepository(db);
                }
                return _factorCostSetRepository;
            }
        }
        public FactorRepository FactorRepository
        {
            get
            {
                if (this._factorRepository == null)
                {
                    this._factorRepository = new FactorRepository(db);
                }
                return _factorRepository;
            }
        }


        public ProductOptionValueRepository ProductOptionValueRepository
        {
            get
            {
                if (this._productOptionValueRepository==null)
                {
                    this._productOptionValueRepository=new ProductOptionValueRepository(db);
                }
                return this._productOptionValueRepository;
            }
        }

        public CustomerRepository CustomerRepository
        {
            get
            {
                if (this._customerRepository == null)
                {
                    this._customerRepository = new CustomerRepository(db);
                }
                return this._customerRepository;
            }
        }
        public CustomersCategoriesBridgeRepository CustomersCategoriesBridgeRepository
        {
            get
            {
                if (this._customersCategoriesBridgeRepository == null)
                {
                    this._customersCategoriesBridgeRepository = new CustomersCategoriesBridgeRepository(db);
                }
                return this._customersCategoriesBridgeRepository;
            }
        }
        public CustomersOrganizationBridgeRepository CustomersOrganizationBridgeRepository
        {
            get
            {
                if (this._customersOrganizationBridgeRepository == null)
                {
                    this._customersOrganizationBridgeRepository = new CustomersOrganizationBridgeRepository(db);
                }
                return this._customersOrganizationBridgeRepository;
            }
        }

        ////public IQueryable<ApplicationUser> Users { get {
        ////        _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>());
        ////        return _userManager.Users ;
        ////    }
        ////}
        //public ReposUsers Users { get
        //    {
        //        if (_users == null)
        //            _users = new ReposUsers(db);
        //        return _users;
        //    } }
        //public CustomerRepository CustomerRepository
        //{
        //    get
        //    {
        //        if (_customerRepository == null)
        //            _customerRepository = new CustomerRepository(db);
        //        return _customerRepository;
        //    }
        //}
        public CustomersRelationRepository CustomerRelationRepository
        {
            get
            {
                if (_customerRelationRepository == null)
                    _customerRelationRepository = new CustomersRelationRepository(db);
                return _customerRelationRepository;
            }
        }
        public WorkFlowRepository WorkFlowRepository
        {
            get
            {
                if (_workFlowRepository == null)
                    _workFlowRepository = new WorkFlowRepository(db);
                return _workFlowRepository;
            }
        }

        public WorkFlowJobRepository WorkFlowJobRepository
        {
            get
            {
                if (_workFlowJobRepository==null)
                    _workFlowJobRepository = new WorkFlowJobRepository(db);
                return _workFlowJobRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository==null)
                    _userRepository = new UserRepository(db);
                return _userRepository;
            }
        }

        public PermissionRepository PermissionRepository
        {
            get
            {
                if (_permissionRepository == null)
                    _permissionRepository = new PermissionRepository(db);
                return _permissionRepository;
            }
        }
        public CustomerOptionValueRepository CustomerOptionValueRepository
        {
            get
            {
                if (_customerOptionValueRepository == null)
                    _customerOptionValueRepository = new CustomerOptionValueRepository(db);
                return _customerOptionValueRepository;
            }
        }

        public FactorOptionValueRepository FactorOptionValueRepository
        {
            get
            {
                if (_factorOptionvalueRepository==null)
                {
                    this._factorOptionvalueRepository=new FactorOptionValueRepository(db);
                }
                return _factorOptionvalueRepository;
            }
        }


        //public ReposCustomers ReposCustomers => throw new NotImplementedException();

        //public ReposCustomersRelation ReposCustomersRelation => throw new NotImplementedException();
        #region Implement
        public bool Save()
        {
            int saveChange = db.SaveChanges();
            if (saveChange > 0)
                return true;
            else
                return false;
        }
        public Task<int> SaveAsync()
        {
            return db.SaveChangesAsync();
        }
        bool IUnitOfWork.Save()
        {

          return  this.Save();
        }

        Task<int> IUnitOfWork.SaveAsync()
        {
            return db.SaveChangesAsync();
        }


        #endregion
        #region Dispose
        protected bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
                db.Dispose();
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}