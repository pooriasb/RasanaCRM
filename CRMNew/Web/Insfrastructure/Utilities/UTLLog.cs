using System.Net;
using Web.Insfrastructure.UnitOfWork;

namespace Web.Insfrastructure.Utilities
{
    public class UTLLog
    {
        private readonly IUnitOfWork unitOfWork;

        public UTLLog(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public UTLLog()
        {
            this.unitOfWork = new UnitOfWork.UnitOfWork();
        }
        public void AddLog(int type, int document, string action, string enteredData, string resultData,
            string userId, string description)
        {
            unitOfWork.CrmLogsRepository.Insert(type, document, action, enteredData, resultData, userId, description, GetIP());
            unitOfWork.Save();
        }

        private string GetIP()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();

        }
    }
}